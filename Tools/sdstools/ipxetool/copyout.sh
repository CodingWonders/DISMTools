#!/bin/bash
#
# copyout.sh - Builds the custom iPXE binaries used by DISMTools SDS
#
# Copies the virtual structure (vistruct) over the iPXE source tree, builds
# the requested targets and copies the resulting binaries to ipxeout.
#
# Targets:
#   x86_64-efi  -> snponly.efi (or ipxe.efi if SNPONLY=0)
#   arm64-efi   -> snponly.efi (or ipxe.efi if SNPONLY=0)
#   bios        -> undionly.kpxe (or ipxe.pxe if SNPONLY=0). Best effort:
#                  a failure here does not fail the whole script.
#
# Usage: ./copyout.sh [-f|--force] [--no-deps] [-j N] [target ...]
#   -f, --force   Rebuild targets even if their output already exists
#   --no-deps     Skip the dependency check/installation
#   -j N          Number of parallel make jobs (default: nproc)
#   target        One or more of: x86_64-efi arm64-efi bios (default: all)
#
# Environment overrides: SNPONLY, IPXE_DIR, OUT_DIR
#

set -uo pipefail

# 1 = SNP-only (EFI) / UNDI-only (BIOS) images that use the firmware's NIC
# driver. 0 = full images with all of iPXE's built-in NIC drivers.
SNPONLY=${SNPONLY:-1}

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
VISTRUCT_DIR="$SCRIPT_DIR/vistruct"
IPXE_DIR="${IPXE_DIR:-$(cd "$SCRIPT_DIR/../ipxe" 2>/dev/null && pwd)}"
OUT_DIR="${OUT_DIR:-$SCRIPT_DIR/../ipxeout}"

FORCE=0
CHECK_DEPS=1
JOBS="$(nproc 2>/dev/null || echo 2)"
TARGETS=()
ALL_TARGETS=(x86_64-efi arm64-efi bios)

HOST_ARCH="$(uname -m)"

function info { echo -e "\e[1;34m[INFO]\e[0m $*"; }
function warn { echo -e "\e[1;33m[WARN]\e[0m $*" >&2; }
function fail { echo -e "\e[1;31m[FAIL]\e[0m $*" >&2; exit 1; }

function usage {
	sed -n '2,/^$/p' "${BASH_SOURCE[0]}" | sed 's/^# \{0,1\}//'
	exit "${1:-0}"
}

# --- Target definitions --------------------------------------------------

# Prints the make target (relative to ipxe/src) for a given target name
function maketarget {
	case "$1" in
		x86_64-efi)
			[ "$SNPONLY" -eq 1 ] && echo "bin-x86_64-efi/snponly.efi" || echo "bin-x86_64-efi/ipxe.efi" ;;
		arm64-efi)
			[ "$SNPONLY" -eq 1 ] && echo "bin-arm64-efi/snponly.efi" || echo "bin-arm64-efi/ipxe.efi" ;;
		bios)
			[ "$SNPONLY" -eq 1 ] && echo "bin/undionly.kpxe" || echo "bin/ipxe.pxe" ;;
		*)
			return 1 ;;
	esac
}

# Prints the file name used in ipxeout for a given target name
function outname {
	local base
	base="$(basename "$(maketarget "$1")")"
	case "$1" in
		x86_64-efi) echo "${base%.efi}-x86_64.efi" ;;
		arm64-efi)  echo "${base%.efi}-arm64.efi" ;;
		bios)       echo "$base" ;;
	esac
}

# Prints the CROSS= prefix needed to build a target on this host (if any)
function crossprefix {
	case "$1:$HOST_ARCH" in
		arm64-efi:aarch64|arm64-efi:arm64) echo "" ;;
		arm64-efi:*)                       echo "aarch64-linux-gnu-" ;;
		x86_64-efi:x86_64|bios:x86_64)     echo "" ;;
		x86_64-efi:*|bios:*)               echo "x86_64-linux-gnu-" ;;
	esac
}

# --- Dependencies --------------------------------------------------------

function checkdeps {
	local pkgs=(build-essential git perl liblzma-dev)
	local t missing=()

	for t in "${TARGETS[@]}"; do
		case "$(crossprefix "$t")" in
			aarch64-linux-gnu-) pkgs+=(gcc-aarch64-linux-gnu binutils-aarch64-linux-gnu) ;;
			x86_64-linux-gnu-)  pkgs+=(gcc-x86-64-linux-gnu binutils-x86-64-linux-gnu) ;;
		esac
	done

	if ! command -v dpkg-query >/dev/null 2>&1; then
		warn "dpkg-query not found. This script targets Ubuntu; skipping dependency check."
		return 0
	fi

	for t in $(printf '%s\n' "${pkgs[@]}" | sort -u); do
		if ! dpkg-query -W -f='${Status}' "$t" 2>/dev/null | grep -q "install ok installed"; then
			missing+=("$t")
		fi
	done

	if [ ${#missing[@]} -eq 0 ]; then
		info "All dependencies are installed."
		return 0
	fi

	info "Installing missing dependencies: ${missing[*]}"
	local sudo=""
	if [ "$(id -u)" -ne 0 ]; then
		command -v sudo >/dev/null 2>&1 || fail "sudo is required to install: ${missing[*]}"
		sudo="sudo"
	fi
	$sudo apt-get update || fail "apt-get update failed."
	$sudo env DEBIAN_FRONTEND=noninteractive apt-get install -y "${missing[@]}" \
		|| fail "Could not install dependencies: ${missing[*]}"
}

# --- Steps ---------------------------------------------------------------

function checksource {
	if [ -z "$IPXE_DIR" ] || [ ! -f "$IPXE_DIR/src/Makefile" ]; then
		info "iPXE source not found. Trying to initialize the submodule..."
		git -C "$SCRIPT_DIR/.." submodule update --init ipxe \
			|| fail "Could not initialize the iPXE submodule."
		IPXE_DIR="$(cd "$SCRIPT_DIR/../ipxe" && pwd)"
		[ -f "$IPXE_DIR/src/Makefile" ] || fail "iPXE source not found at $IPXE_DIR."
	fi
	[ -d "$VISTRUCT_DIR" ] || fail "Virtual structure not found at $VISTRUCT_DIR."
}

# Copies the virtual structure over the iPXE source tree. vistruct mirrors the
# layout of the iPXE repository (e.g. vistruct/src/config/local/general.h).
function copyvistruct {
	info "Copying virtual structure to $IPXE_DIR..."
	cp -rv "$VISTRUCT_DIR/." "$IPXE_DIR/" || fail "Could not copy the virtual structure."
}

# Builds a target if its output is not there yet (or if forced).
# Returns 0 on success, 1 on failure.
function buildipxe {
	local target="$1"
	local mt cross out
	mt="$(maketarget "$target")"
	cross="$(crossprefix "$target")"
	out="$IPXE_DIR/src/$mt"

	if [ -f "$out" ] && [ "$FORCE" -eq 0 ]; then
		info "$target is already built ($mt). Use --force to rebuild."
		return 0
	fi

	# Make sure modified vistruct headers are picked up on a rebuild
	[ "$FORCE" -eq 1 ] && rm -f "$out"

	info "Building $target ($mt)${cross:+ with CROSS=$cross}..."
	if ! make -C "$IPXE_DIR/src" -j"$JOBS" ${cross:+CROSS="$cross"} "$mt"; then
		warn "Build of $target failed."
		return 1
	fi
	[ -f "$out" ] || { warn "Build of $target produced no output."; return 1; }
	return 0
}

function copyoutput {
	local target="$1"
	local src dst
	src="$IPXE_DIR/src/$(maketarget "$target")"
	dst="$OUT_DIR/$(outname "$target")"
	mkdir -p "$OUT_DIR"
	cp -v "$src" "$dst" || return 1
}

# --- Main ----------------------------------------------------------------

while [ $# -gt 0 ]; do
	case "$1" in
		-f|--force) FORCE=1 ;;
		--no-deps)  CHECK_DEPS=0 ;;
		-j)         shift; JOBS="${1:?-j needs a number}" ;;
		-j*)        JOBS="${1#-j}" ;;
		-h|--help)  usage 0 ;;
		x86_64-efi|arm64-efi|bios) TARGETS+=("$1") ;;
		*)          warn "Unknown argument: $1"; usage 1 ;;
	esac
	shift
done

[ ${#TARGETS[@]} -eq 0 ] && TARGETS=("${ALL_TARGETS[@]}")
[[ "$SNPONLY" =~ ^[01]$ ]] || fail "SNPONLY must be 0 or 1."

info "Host architecture: $HOST_ARCH; SNPONLY=$SNPONLY; targets: ${TARGETS[*]}"

[ "$CHECK_DEPS" -eq 1 ] && checkdeps
checksource
copyvistruct

FAILED=()
for t in "${TARGETS[@]}"; do
	if buildipxe "$t" && copyoutput "$t"; then
		continue
	fi
	FAILED+=("$t")
done

echo
info "Output directory: $OUT_DIR"
ls -l "$OUT_DIR" 2>/dev/null

# Legacy BIOS is best effort; only the EFI targets are fatal
FATAL=0
for t in "${FAILED[@]}"; do
	if [ "$t" = "bios" ]; then
		warn "Legacy BIOS image could not be built. Continuing without it."
	else
		warn "Target $t failed."
		FATAL=1
	fi
done

[ "$FATAL" -eq 0 ] || fail "One or more EFI targets failed to build."
info "Done."

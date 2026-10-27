const releaseSection = document.querySelector(".dt-home__release");
const releaseDownload = document.querySelector("#dt-home-release-download");
const releaseDownloadLabel = document.querySelector("#dt-home-release-download-label");
const previewReleaseLink = document.querySelector("#dt-home-preview-release");
const previewReleaseLabel = document.querySelector("#dt-home-preview-release-label");
const homeFeatures = document.querySelector(".dt-home__features");
const homeButtons = document.querySelector(".dt-home__custom-buttons");
const homeHeader = document.querySelector(".md-header");

// if NOTASITE exists, we need to update the subtitle
const homeScreenSubtitle = document.querySelector(".dt-home__subtitle");

const githubHeaders = {
	Accept: "application/vnd.github+json",
	"X-GitHub-Api-Version": "2022-11-28",
};

async function siteNoticeFileExists() {
	try {
		const response = await fetch(new URL("NOTASITE", document.baseURI), {
			method: "HEAD",
			cache: "no-store",
		});

		if (response.status === 404) {
			return false;
		}

		if (!response.ok) {
			throw new Error(`Could not check the NOTASITE marker (HTTP ${response.status}).`);
		}

		return true;
	} catch (error) {
		return true;
	}
}

async function showLatestPreviewRelease() {
	if (!previewReleaseLink || !previewReleaseLabel) {
		return;
	}

	try {
		const response = await fetch(
			"https://api.github.com/repos/CodingWonders/DISMTools/releases?per_page=100",
			{ headers: githubHeaders },
		);

		if (!response.ok) {
			throw new Error(`GitHub API request failed with status ${response.status}.`);
		}

		const releases = await response.json();
		const latestPreview = releases
			.filter((release) => release.prerelease && !release.draft && release.html_url && release.published_at)
			.sort((a, b) => Date.parse(b.published_at) - Date.parse(a.published_at))[0];

		if (!latestPreview) {
			throw new Error("GitHub returned no published preview releases.");
		}

		const previewUrl = new URL(latestPreview.html_url);
		if (previewUrl.protocol !== "https:" || previewUrl.hostname !== "github.com") {
			throw new Error("GitHub returned an invalid preview release URL.");
		}

		previewReleaseLink.href = previewUrl.href;
		previewReleaseLabel.textContent = `Download DISMTools Preview ${latestPreview.name}`;
		previewReleaseLink.hidden = false;
	} catch (error) {
		console.error("Could not load the latest DISMTools preview release.", error);
	}
}

if (releaseSection && releaseDownload && releaseDownloadLabel) {
	const latestReleaseApi = "https://api.github.com/repos/CodingWonders/DISMTools/releases/latest";

	async function showLatestStableRelease() {
		try {
			const response = await fetch(latestReleaseApi, {
				headers: githubHeaders,
			});

			if (!response.ok) {
				throw new Error(`GitHub API request failed with status ${response.status}.`);
			}

			const release = await response.json();
			const version = release.name;
			if (release.draft || release.prerelease || !version || !release.html_url) {
				throw new Error("GitHub returned no stable release version or release URL.");
			}

			const releaseUrl = new URL(release.html_url);
			if (releaseUrl.protocol !== "https:" || releaseUrl.hostname !== "github.com") {
				throw new Error("GitHub returned an invalid release URL.");
			}

			releaseDownload.href = releaseUrl.href;
			releaseDownloadLabel.textContent = `Download DISMTools ${version.replace(/^v/, "")}`;
			releaseSection.hidden = false;
		} catch (error) {
			console.error("Could not load the latest stable DISMTools release.", error);
		}
	}

	async function initializeReleaseLinks() {
		try {
			if (await siteNoticeFileExists()) {
				releaseSection.hidden = true;
				homeFeatures?.remove();
				if (previewReleaseLink) {
					previewReleaseLink.hidden = true;
				}
				if (homeScreenSubtitle) {
					homeScreenSubtitle.textContent += " Use one of the buttons below to get started with your journey.";
				}
				return;
			}
			if (homeFeatures) {
				homeFeatures.hidden = false;
			}
			if (homeButtons && homeHeader && "IntersectionObserver" in window) {
				let headerObserver;
				const observeButtonsBehindHeader = () => {
					headerObserver?.disconnect();

					const headerBounds = homeHeader.getBoundingClientRect();
					const topMargin = -headerBounds.top;
					const bottomMargin = -(window.innerHeight - headerBounds.bottom);
					headerObserver = new IntersectionObserver(([entry]) => {
						const buttonsAreBelowHeader = entry.boundingClientRect.top >= entry.rootBounds.bottom;
						document.body.classList.toggle("dt-home--scrolled", entry.isIntersecting || !buttonsAreBelowHeader);
					}, {
						rootMargin: `${topMargin}px 0px ${bottomMargin}px 0px`,
						threshold: 0,
					});

					headerObserver.observe(homeButtons);
				};

				observeButtonsBehindHeader();
				window.addEventListener("resize", observeButtonsBehindHeader);
			}
		} catch (error) {
			console.error("Could not check whether the NOTASITE marker exists.", error);
			homeFeatures?.remove();
		}

		showLatestStableRelease();
		showLatestPreviewRelease();
	}

	initializeReleaseLinks();
}

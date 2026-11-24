#pragma once
#include <windows.h>
#include <commctrl.h>
#include <ShellScalingApi.h>
#include <string>
// ReSharper disable CppUnusedIncludeDirective
#include <vector>
#include <thread>
#include <commdlg.h>
#include <shlobj.h>
#include <iostream>
#include <tchar.h>
#include <fstream>
#include <codecvt>
#include "resource.h"
// ReSharper restore CppUnusedIncludeDirective

#pragma comment(lib, "comctl32.lib")
#pragma comment(lib, "Shcore.lib")
#pragma warning(disable: 4312)

#define IDC_DRIVER_LIST   101
#define IDC_ADD_BUTTON    102
#define IDC_EDIT_BUTTON   103
#define IDC_REMOVE_BUTTON 104
#define IDC_INSTALL_BUTTON 105
#define IDC_EXIT_BUTTON   106
#define IDC_ABOUT_BUTTON  107
#define IDC_ADD_DRV_FILE 108
#define IDC_ADD_DRV_FOLDER 109

// Version Constant
const auto DIM_VERSION = L"0.7.3_msvcv145";
const auto DIM_ABOUT_MESSAGE = L"Driver Installation Module version %s \n(c) 2024-2026 CodingWonders Software.";

// Instruction Message Constants

/// <summary>
/// The initial greeting message
/// </summary>
const auto INSTR_DRIVER_BEGIN = L"Begin by adding drivers to the queue. Click the \"Add\" button.";
/// <summary>
/// The modification instruction message
/// </summary>
const auto INSTR_DRIVER_MODIFY_ADD = L"Modify the driver selection or click the \"Install\" button to add the selected driver(s).";
/// <summary>
/// The driver installation progress instruction message
/// </summary>
const auto* INSTR_DRIVER_INSTALL_PROGRESS = L"Installing driver %d of %d...";
/// <summary>
/// The driver installation summary instruction message
/// </summary>
const auto* INSTR_DRIVER_INSTALL_SUMMARY = L"Out of %d driver(s), %d were installed successfully.";

static float GetDpiMultiplier(HWND window);

LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam);

/// <summary>
/// Adds a driver to the list.
/// </summary>
/// <param name="hwndList">Handle to the list control where the driver will be added.</param>
/// <param name="driverPath">Path to the driver file.</param>
/// <param name="hEditButton">Handle to the Edit button to update its state.</param>
/// <param name="hRemoveButton">Handle to the Remove button to update its state.</param>
/// <param name="hInstallButton">Handle to the Install button to update its state.</param>
void AddDriver(HWND hwndList, const std::wstring& driverPath, HWND hEditButton, HWND hRemoveButton, HWND hInstallButton);

/// <summary>
/// Edits the selected driver in the list.
/// </summary>
/// <param name="hwndList">Handle to the list control containing the drivers.</param>
/// <param name="mainHwnd">Handle to the main application window.</param>
void EditDriver(HWND hwndList, HWND mainHwnd);

/// <summary>
/// Removes the selected driver from the list.
/// </summary>
/// <param name="hwndList">Handle to the list control containing the drivers.</param>
/// <param name="hEditButton">Handle to the Edit button to update its state.</param>
/// <param name="hRemoveButton">Handle to the Remove button to update its state.</param>
/// <param name="hInstallButton">Handle to the Install button to update its state.</param>
void RemoveDriver(HWND hwndList, HWND hEditButton, HWND hRemoveButton, HWND hInstallButton);

/// <summary>
/// Installs the drivers in the list.
/// </summary>
/// <param name="hwndList">Handle to the list control containing the drivers.</param>
/// <param name="mainHwnd">Handle to the main application window.</param>
/// <param name="instructionHwnd">Handle to the instruction label to update messages.</param>
void InstallDrivers(HWND hwndList, HWND mainHwnd, HWND instructionHwnd);

/// <summary>
/// Updates the states of the buttons based on the current selection in the list.
/// </summary>
/// <param name="hwnd">Handle to the main application window.</param>
void UpdateButtonStates(HWND hwnd);

/// <summary>
/// Opens a file dialog to select a driver file.
/// </summary>
/// <param name="hwnd">Handle to the parent window for the dialog.</param>
/// <returns>Path to the selected driver file.</returns>
std::wstring OpenFileDialog(HWND hwnd);

/// <summary>
/// Opens a folder dialog to select a folder containing driver files.
/// </summary>
/// <param name="hwnd">Handle to the parent window for the dialog.</param>
/// <returns>Path to the selected folder.</returns>
std::wstring OpenFolderDialog(HWND hwnd);

/// <summary>
/// Searches a directory for driver files and adds them to the list.
/// </summary>
/// <param name="hwndList">Handle to the list control where drivers will be added.</param>
/// <param name="folderPath">Path to the folder to search for drivers.</param>
/// <param name="hEditButton">Handle to the Edit button to update its state.</param>
/// <param name="hRemoveButton">Handle to the Remove button to update its state.</param>
/// <param name="hInstallButton">Handle to the Install button to update its state.</param>
void SearchDirectoryForDrivers(HWND hwndList, const std::wstring& folderPath, HWND hEditButton, HWND hRemoveButton, HWND hInstallButton);

/// <summary>
/// Retrieves a value from the Windows registry.
/// </summary>
/// <param name="hwnd">Handle to the parent window for error messages.</param>
/// <param name="key">Handle to the registry key.</param>
/// <param name="subKey">Path to the subkey.</param>
/// <param name="valueName">Name of the value to retrieve.</param>
/// <returns>Value retrieved from the registry.</returns>
std::wstring GetRegistryValue(HWND hwnd, HKEY key, const wchar_t* subKey, const wchar_t* valueName);

/// <summary>
/// Updates the instruction label with a new message.
/// </summary>
/// <param name="instructionLabel">Handle to the instruction label control.</param>
/// <param name="message">Message to display in the instruction label.</param>
void UpdateInstructionLabel(HWND instructionLabel, LPCWSTR message);

/// <summary>
/// Sets the cursor for the application window.
/// </summary>
/// <param name="cursor">Cursor resource to set.</param>
void SetWindowCursor(LPCTSTR cursor);
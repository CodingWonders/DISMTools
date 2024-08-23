//                                              ....              
//                                         .'^""""""^.            
//      '^`'.                            '^"""""""^.              
//     .^"""""`'                       .^"""""""^.                ---------------------------------------------------------
//      .^""""""`                      ^"""""""`                  | DISMTools 0.6                                         |
//       ."""""""^.                   `""""""""'           `,`    | The connected place for Windows system administration |
//         '`""""""`.                 """""""""^         `,,,"    ---------------------------------------------------------
//            '^"""""`.               ^""""""""""'.   .`,,,,,^    | PE Helper - Driver Installation Module (DIM)	      |
//              .^"""""`.            ."""""""",,,,,,,,,,,,,,,.    ---------------------------------------------------------
//                .^"""""^.        .`",,"""",,,,,,,,,,,,,,,,'     | (C) 2024 CodingWonders Software                       |
//                  .^"""""^.    '`^^"",:,,,,,,,,,,,,,,,,,".      ---------------------------------------------------------
//                    .^"""""^.`+]>,^^"",,:,,,,,,,,,,,,,`.        
//                      .^""";_]]]?)}:^^""",,,`'````'..           
//                        .;-]]]?(xxxx}:^^^^'                     
//                       `+]]]?(xxxxxxxr},'                       
//                     .`:+]?)xxxxxxxxxxxr<.                      
//                   .`^^^^:(xxxxxxxxxxxxxxr>.                    
//                 .`^^^^^^^^I(xxxxxxxxxxxxxxr<.                  
//               .`^^^^^^^^^^^^I(xxxxxxxxxxxxxxr<.                
//             .`^^^^^^^^^^^^^^^'`[xxxxxxxxxxxxxxr<.              
//           .`^^^^^^^^^^^^^^^'    `}xxxxxxxxxxxxxxr<.            
//          `^^":ll:"^^^^^^^'        `}xxxxxxxxxxxxxxr,           
//         '^^^I-??]l^^^^^'            `[xxxxxxxxxxxxxx.          
//         '^^^,<??~,^^^'                `{xxxxxxxxxxxx.          
//          `^^^^^^^^^'                    `{xxxxxxxxr,           This program is provided AS IS, without any warranty.
//           .'`^^^`'                        `i1jrt[:.            

#include <windows.h>
#include <commctrl.h>
#include <string>
#include <vector>
#include <thread>
#include <commdlg.h>
#include <shlobj.h>
#include <iostream>
#include <tchar.h>
#include <fstream>
#include <codecvt>
#include "resource.h"

#pragma comment(lib, "comctl32.lib")

#define IDC_DRIVER_LIST   101
#define IDC_ADD_BUTTON    102
#define IDC_EDIT_BUTTON   103
#define IDC_REMOVE_BUTTON 104
#define IDC_INSTALL_BUTTON 105
#define IDC_EXIT_BUTTON   106
#define IDC_ADD_DRV_FILE 107
#define IDC_ADD_DRV_FOLDER 108

LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam);
void AddDriver(HWND hwndList, const std::wstring& driverPath, HWND hEditButton, HWND hRemoveButton, HWND hInstallButton);
void EditDriver(HWND hwndList, HWND mainHwnd);
void RemoveDriver(HWND hwndList, HWND hEditButton, HWND hRemoveButton, HWND hInstallButton);
void InstallDrivers(HWND hwndList, HWND mainHwnd);
void UpdateButtonStates(HWND hwnd);
std::wstring OpenFileDialog(HWND hwnd);
std::wstring OpenFolderDialog(HWND hwnd);
void SearchDirectoryForDrivers(HWND hwndList, const std::wstring& folderPath, HWND hEditButton, HWND hRemoveButton, HWND hInstallButton);
std::wstring GetRegistryValue(HWND hwnd, HKEY key, const wchar_t* subKey, const wchar_t* valueName);

enum installationStatus {
	// The device driver has been added to the queue and is ready to be installed
	ReadyToInstall = 0,
	// The device driver is being installed
	Installing = 1,
	// The device driver has been installed
	Installed = 2,
	// The device driver could not be installed
	Failed = 3,
	// The status of the device driver could not be obtained
	StatusUnknown = 4
};

std::wstring GetRegistryValue(HWND hwnd, HKEY key, const wchar_t* subKey, const wchar_t* valueName) {
	HKEY hKey;
	if (RegOpenKeyEx(key, subKey, 0, KEY_READ, &hKey) != ERROR_SUCCESS) {
		MessageBox(hwnd, L"We could not determine the operating environment.", L"Error getting operating system information", MB_OK | MB_ICONERROR);
		return L"";
	}
	DWORD dataSize = 0;
	if (RegQueryValueEx(hKey, valueName, nullptr, nullptr, nullptr, &dataSize) != ERROR_SUCCESS) {
		MessageBox(hwnd, L"We could not determine the operating environment.", L"Error getting operating system information", MB_OK | MB_ICONERROR);
		RegCloseKey(hKey);
		return L"";
	}
	std::wstring result;
	result.resize(dataSize / sizeof(wchar_t));
	if (RegQueryValueEx(hKey, valueName, nullptr, nullptr, reinterpret_cast<LPBYTE>(&result[0]), &dataSize) != ERROR_SUCCESS) {
		MessageBox(hwnd, L"We could not determine the operating environment.", L"Error getting operating system information", MB_OK | MB_ICONERROR);
		result.clear();
	}
	RegCloseKey(hKey);
	return result;
}

int WINAPI wWinMain(HINSTANCE hInstance, HINSTANCE, PWSTR, int nCmdShow) {
    const wchar_t CLASS_NAME[] = L"Driver Installation Module";

    WNDCLASS wc = {};
    wc.lpfnWndProc = WindowProc;
    wc.hInstance = hInstance;
    wc.lpszClassName = CLASS_NAME;

    RegisterClass(&wc);

	POINT cursorPosition;
	GetCursorPos(&cursorPosition);

	HMONITOR hMon = MonitorFromPoint(cursorPosition, MONITOR_DEFAULTTONEAREST);
	MONITORINFO monInfo = { sizeof(monInfo) };
	GetMonitorInfo(hMon, &monInfo);

	int WndX = monInfo.rcMonitor.left + ((monInfo.rcMonitor.right - monInfo.rcMonitor.left) - 600) / 2;
	int WndY = monInfo.rcMonitor.top + ((monInfo.rcMonitor.bottom - monInfo.rcMonitor.top) - 400) / 2;

    HWND hwnd = CreateWindowEx(0, CLASS_NAME, L"Driver Installation Module", 
                               WS_OVERLAPPEDWINDOW & ~(WS_THICKFRAME | WS_MAXIMIZEBOX),
                               WndX, WndY, 600, 400, NULL, NULL, hInstance, NULL);

	HICON icon = LoadIcon(GetModuleHandle(NULL), MAKEINTRESOURCE(IDI_ICON1));
	SendMessage(hwnd, WM_SETICON, ICON_BIG, (LPARAM)icon);

    if (hwnd == NULL) {
        return 0;
    }

    ShowWindow(hwnd, nCmdShow);

    MSG msg = {};
    while (GetMessage(&msg, NULL, 0, 0)) {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    return 0;
}

LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam) {
    static HWND hwndList, hAddButton, hEditButton, hRemoveButton, hInstallButton, hExitButton;

    switch (uMsg) {
        case WM_CREATE: {
            InitCommonControls();

            hwndList = CreateWindow(WC_LISTVIEW, L"",
                                    WS_CHILD | WS_VISIBLE | LVS_REPORT | LVS_EDITLABELS,
                                    10, 10, 560, 300, hwnd, (HMENU)IDC_DRIVER_LIST, NULL, NULL);

            // Enable full row select
            ListView_SetExtendedListViewStyle(hwndList, LVS_EX_FULLROWSELECT);

			// Set listview to not edit labels by double-clicking them
			LONG_PTR style = GetWindowLongPtr(hwndList, GWL_STYLE);
			style &= ~LVS_EDITLABELS;
			SetWindowLongPtr(hwndList, GWL_STYLE, style);

            LVCOLUMN lvColumn;
            lvColumn.mask = LVCF_TEXT | LVCF_WIDTH | LVCF_SUBITEM;
            lvColumn.cx = 384;
            lvColumn.pszText = L"Path";
            ListView_InsertColumn(hwndList, 0, &lvColumn);

            lvColumn.cx = 150;
            lvColumn.pszText = L"Status";
            ListView_InsertColumn(hwndList, 1, &lvColumn);

            hAddButton = CreateWindow(L"BUTTON", L"Add...", WS_TABSTOP | WS_VISIBLE | WS_CHILD | BS_DEFPUSHBUTTON,
                                      10, 320, 100, 30, hwnd, (HMENU)IDC_ADD_BUTTON, NULL, NULL);

            hEditButton = CreateWindow(L"BUTTON", L"Change...", WS_TABSTOP | WS_VISIBLE | WS_CHILD | BS_DEFPUSHBUTTON,
                                       120, 320, 100, 30, hwnd, (HMENU)IDC_EDIT_BUTTON, NULL, NULL);

            hRemoveButton = CreateWindow(L"BUTTON", L"Remove", WS_TABSTOP | WS_VISIBLE | WS_CHILD | BS_DEFPUSHBUTTON,
                                         230, 320, 100, 30, hwnd, (HMENU)IDC_REMOVE_BUTTON, NULL, NULL);

            hInstallButton = CreateWindow(L"BUTTON", L"Install", WS_TABSTOP | WS_VISIBLE | WS_CHILD | BS_DEFPUSHBUTTON,
                                          340, 320, 100, 30, hwnd, (HMENU)IDC_INSTALL_BUTTON, NULL, NULL);

            hExitButton = CreateWindow(L"BUTTON", L"Exit", WS_TABSTOP | WS_VISIBLE | WS_CHILD | BS_DEFPUSHBUTTON,
                                       450, 320, 100, 30, hwnd, (HMENU)IDC_EXIT_BUTTON, NULL, NULL);
            // Initially disable buttons
            EnableWindow(hEditButton, FALSE);
            EnableWindow(hRemoveButton, FALSE);
            EnableWindow(hInstallButton, FALSE);
            break;
        }

		case WM_ERASEBKGND: {
			HDC hdc = (HDC)wParam;
			RECT rect;
			GetClientRect(hwnd, &rect);
			HBRUSH hBrush = CreateSolidBrush(RGB(255, 255, 255)); // Create a white brush
			FillRect(hdc, &rect, hBrush); // Fill the background with the white brush
			DeleteObject(hBrush); // Delete the brush to release resources
			return (LRESULT)1; // Indicate that background has been handled
			break;
		}

        case WM_COMMAND: {
            switch (LOWORD(wParam)) {
                case IDC_ADD_BUTTON:
					{
						HMENU drvMenu = CreatePopupMenu();
						AppendMenu(drvMenu, MF_STRING, IDC_ADD_DRV_FILE, L"Add driver file...");
						AppendMenu(drvMenu, MF_STRING, IDC_ADD_DRV_FOLDER, L"Add driver package folder...");
						RECT rc;
						GetWindowRect(hAddButton, &rc);
						TrackPopupMenu(drvMenu, TPM_RIGHTBUTTON, rc.left, rc.bottom, 0, hwnd, NULL);
						DestroyMenu(drvMenu);
					}
                    break;
				case IDC_ADD_DRV_FILE:
					{
						std::wstring filePath = OpenFileDialog(hwnd);
						if (!filePath.empty()) {
							AddDriver(hwndList, filePath, hEditButton, hRemoveButton, hInstallButton);
						}
						UpdateButtonStates(hwnd);
					}	
					break;
				case IDC_ADD_DRV_FOLDER:
					{
						std::wstring folderPath = OpenFolderDialog(hwnd);
						if (!folderPath.empty()) {
							SearchDirectoryForDrivers(hwndList, folderPath, hEditButton, hRemoveButton, hInstallButton);
						}
						UpdateButtonStates(hwnd);
					}
					break;
                case IDC_EDIT_BUTTON:
                    EditDriver(hwndList, hwnd);
					UpdateButtonStates(hwnd);
                    break;
                case IDC_REMOVE_BUTTON:
                    RemoveDriver(hwndList, hEditButton, hRemoveButton, hInstallButton);
					UpdateButtonStates(hwnd);
                    break;
                case IDC_INSTALL_BUTTON:
					EnableWindow(GetDlgItem(hwnd, IDC_ADD_BUTTON), FALSE);
					EnableWindow(GetDlgItem(hwnd, IDC_EDIT_BUTTON), FALSE);
					EnableWindow(GetDlgItem(hwnd, IDC_REMOVE_BUTTON), FALSE);
					EnableWindow(GetDlgItem(hwnd, IDC_INSTALL_BUTTON), FALSE);
					EnableWindow(GetDlgItem(hwnd, IDC_EXIT_BUTTON), FALSE);
                    std::thread(InstallDrivers, hwndList, hwnd).detach();
                    break;
                case IDC_EXIT_BUTTON:
                    PostQuitMessage(0);
                    break;
            }
            break;
        }

		case WM_NOTIFY: {
			if (((LPNMHDR)lParam)->idFrom == IDC_DRIVER_LIST && ((LPNMHDR)lParam)->code == LVN_ITEMCHANGED) {
				UpdateButtonStates(hwnd);
			}
			break;
		}

        case WM_DESTROY: {
            PostQuitMessage(0);
            return 0;
        }
    }

    return DefWindowProc(hwnd, uMsg, wParam, lParam);
}

std::wstring GetStatusFromEnumeration(installationStatus status) {
	switch (status) {
		case ReadyToInstall: return L"Ready to install";
		case Installing: return L"Installing...";
		case Installed: return L"Successfully installed";
		case Failed: return L"Failed to install";
		case StatusUnknown: return L"Unknown";
	}
	return L"";
}

void AddDriver(HWND hwndList, const std::wstring& driverPath, HWND hEditButton, HWND hRemoveButton, HWND hInstallButton) {
    LVITEM lvItem;
    lvItem.mask = LVIF_TEXT;
    lvItem.iItem = ListView_GetItemCount(hwndList);
    lvItem.iSubItem = 0;
    lvItem.pszText = (LPWSTR)driverPath.c_str();
	std::wstring state = GetStatusFromEnumeration(ReadyToInstall);
    ListView_InsertItem(hwndList, &lvItem);
    ListView_SetItemText(hwndList, lvItem.iItem, 1, (LPWSTR)state.c_str());
    UpdateButtonStates(hwndList);
}

void EditDriver(HWND hwndList, HWND mainHwnd) {
    int iSelected = ListView_GetNextItem(hwndList, -1, LVNI_SELECTED);
    if (iSelected != -1) {
		std::wstring newFilePath = OpenFileDialog(mainHwnd);
		if (!newFilePath.empty()) {
			ListView_SetItemText(hwndList, iSelected, 0, (LPWSTR)newFilePath.c_str());
		}
    }
}

void RemoveDriver(HWND hwndList, HWND hEditButton, HWND hRemoveButton, HWND hInstallButton) {
    // Iterate backwards through the items to avoid issues with changing indices
    for (int i = ListView_GetItemCount(hwndList) - 1; i >= 0; --i) {
        // Check if the item is selected
        if (ListView_GetItemState(hwndList, i, LVIS_SELECTED) & LVIS_SELECTED) {
            // Remove the item
            ListView_DeleteItem(hwndList, i);
        }
    }
    UpdateButtonStates(hwndList);
}

std::wstring GetBootDriveRoot() {
	WCHAR systemDir[MAX_PATH];
	if (GetSystemDirectory(systemDir, MAX_PATH)) {
		std::wstring sysPath(systemDir);
		return sysPath.substr(0, 3);
	}
	return L"C:\\";
}

void AppendToFile(const std::wstring& filePath, const std::wstring& content) {
    try {
        std::wofstream outFile(filePath, std::ios::app);
        if (!outFile.is_open()) {
            std::wcerr << L"Error opening file: " << filePath << std::endl;
            return;
        }
        outFile.imbue(std::locale(outFile.getloc(), new std::codecvt_utf8<wchar_t>));
        outFile << content << std::endl;
        outFile.close();
    } catch (const std::exception& e) {
        std::wcerr << L"Exception: " << e.what() << std::endl;
    }
}

installationStatus GetStatusFromLVI(HWND hwndList, int itemIndex) {
    try {
        WCHAR buffer[256];
        LVITEM lvItem = {0};
        lvItem.iSubItem = 1;
        lvItem.pszText = buffer;
        lvItem.mask = LVIF_TEXT;
        lvItem.cchTextMax = sizeof(buffer) / sizeof(buffer[0]);
        lvItem.iItem = itemIndex;

        // Retrieve the text from the ListView item
        SendMessage(hwndList, LVM_GETITEMTEXT, itemIndex, (LPARAM)&lvItem);

        std::wstring status(buffer);

        if (status == L"Ready to install") return ReadyToInstall;
        if (status == L"Installing...") return Installing;
        if (status == L"Successfully installed") return Installed;
        if (status == L"Failed to install") return Failed;
    } catch (const std::exception& e) {
        std::cerr << "Exception: " << e.what() << std::endl; // Log exception
        return StatusUnknown; // Default return if item text retrieval fails
    }
    return StatusUnknown;
}


void InstallDrivers(HWND hwndList, HWND mainHwnd) {

	// Disable Close button
	HMENU menu;
	menu = GetSystemMenu(mainHwnd, FALSE);
	if (menu != NULL) {
		EnableMenuItem(menu, SC_CLOSE, MF_BYCOMMAND | MF_DISABLED | MF_GRAYED);
	}

	std::wstring state;

	int successfulInstallations = 0;
	int failedInstallations = 0;
	int unknownStatuses = 0;

	// Detect if program is being run on Windows PE
	std::wstring edition = GetRegistryValue(mainHwnd, HKEY_LOCAL_MACHINE, L"Software\\Microsoft\\Windows NT\\CurrentVersion", L"EditionID");
	if (wcscmp(edition.c_str(), L"WindowsPE") == 0) {
		std::wstring bDriveRoot = GetBootDriveRoot();
		std::wstring logPath = bDriveRoot + L"DT_InstDrvs.txt";

		// Install drivers
		for (int i = 0; i < ListView_GetItemCount(hwndList); ++i) {
			installationStatus currentStatus = GetStatusFromLVI(hwndList, i);

			if (currentStatus == Installed) {
				continue;
			}

			state = GetStatusFromEnumeration(Installing);
			ListView_SetItemText(hwndList, i, 1, (LPWSTR)state.c_str());
			// Initiate command
			STARTUPINFO startInfo = { sizeof(startInfo) };
			PROCESS_INFORMATION procInfo;
			TCHAR systemDir[MAX_PATH];
			if (GetSystemDirectory(systemDir, MAX_PATH)) {
				std::wstring appPath = std::wstring(systemDir) + L"\\drvload.exe";
				WCHAR buffer[256];
				LVITEM lvItem = {0};
				lvItem.mask = LVIF_TEXT;
				lvItem.iSubItem = 0;
				lvItem.cchTextMax = 256;
				lvItem.pszText = buffer;
				lvItem.iItem = i;

				if (ListView_GetItem(hwndList, &lvItem)) {
					// Assuming buffer is being populated elsewhere correctly
					std::wstring cmdArgs(buffer);
					std::wstring cmdLine = appPath + L" \"" + cmdArgs + L"\"";

					// Convert cmdLine to a writable array of WCHAR
					std::vector<WCHAR> cmdLineVec(cmdLine.begin(), cmdLine.end());
					cmdLineVec.push_back(0); // Ensure null-termination

					if (CreateProcess(NULL, cmdLineVec.data(), NULL, NULL, FALSE, CREATE_NO_WINDOW, NULL, NULL, &startInfo, &procInfo)) {
						WaitForSingleObject(procInfo.hProcess, INFINITE);
						DWORD exitCode;
						if (GetExitCodeProcess(procInfo.hProcess, &exitCode)) {
							if (exitCode == ERROR_SUCCESS) {
								state = GetStatusFromEnumeration(Installed);
								ListView_SetItemText(hwndList, i, 1, const_cast<LPWSTR>(state.c_str()));
								successfulInstallations += 1;

								// Append to file
								AppendToFile(logPath, cmdArgs);
							} else {
								failedInstallations += 1;
								std::wstring errorMsg = L"The driver: \n\n";
								errorMsg += cmdArgs;
								errorMsg += L"\n\ncould not be installed. Reason: ";
					            // Map exit code to system error message
								LPWSTR errorText = nullptr;
								FormatMessageW(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM,
											   NULL, exitCode, 0, (LPWSTR)&errorText, 0, NULL);
								if (errorText) {
									errorMsg += std::wstring(errorText) + L" ";
									LocalFree(errorText);
								}
								errorMsg += L"\n\nExit code: " + std::to_wstring(exitCode);
								MessageBox(mainHwnd, errorMsg.c_str(), L"Driver Installation Module", MB_OK | MB_ICONERROR);
								state = GetStatusFromEnumeration(Failed);
								ListView_SetItemText(hwndList, i, 1, const_cast<LPWSTR>(state.c_str()));
							}
						} else {
							unknownStatuses += 1;
							// Could not get exit code
							state = GetStatusFromEnumeration(StatusUnknown);
							ListView_SetItemText(hwndList, i, 1, const_cast<LPWSTR>(state.c_str()));
						}

						CloseHandle(procInfo.hProcess);
						CloseHandle(procInfo.hThread);
					} else {
						DWORD hResult = GetLastError();
						_tprintf(_T("CreateProcess failed (%d).\n"), hResult);
						state = GetStatusFromEnumeration(Failed);
					}
				}
			}
		}
		std::wstring resultMsg = L"Driver installation summary:\n\n";
		resultMsg += L"- Drivers that have been installed successfully: " + std::to_wstring(successfulInstallations);
		resultMsg += L"\n- Drivers that were not installed: " + std::to_wstring(failedInstallations);
		resultMsg += L"\n- Drivers that we could not get the status of: " + std::to_wstring(unknownStatuses);
		if (failedInstallations > 0) {
			resultMsg += L"\n\nYou can try to install the drivers that could not be installed again.";
		}
		MessageBox(mainHwnd, resultMsg.c_str(), L"Driver Installation Module", MB_OK | MB_ICONINFORMATION);
	}
	else {
		MessageBox(mainHwnd, L"This program needs to be run in the Windows Preinstallation Environment (PE) to install drivers.", L"Driver Installation Module", MB_OK | MB_ICONERROR);
	}
	EnableWindow(GetDlgItem(mainHwnd, IDC_ADD_BUTTON), TRUE);
	EnableWindow(GetDlgItem(mainHwnd, IDC_EDIT_BUTTON), (ListView_GetSelectedCount(hwndList) == 1));
	EnableWindow(GetDlgItem(mainHwnd, IDC_REMOVE_BUTTON), (ListView_GetSelectedCount(hwndList) > 0));
	EnableWindow(GetDlgItem(mainHwnd, IDC_INSTALL_BUTTON), (ListView_GetItemCount(hwndList) > 0));
	EnableWindow(GetDlgItem(mainHwnd, IDC_EXIT_BUTTON), TRUE);

	// Enable Close button
	menu = GetSystemMenu(mainHwnd, FALSE);
	if (menu != NULL) {
		EnableMenuItem(menu, SC_CLOSE, MF_BYCOMMAND | MF_ENABLED);
	}
}

void UpdateButtonStates(HWND hwnd) {
	HWND listView = GetDlgItem(hwnd, IDC_DRIVER_LIST);
	HWND editButton = GetDlgItem(hwnd, IDC_EDIT_BUTTON);
	HWND removeButton = GetDlgItem(hwnd, IDC_REMOVE_BUTTON);
	HWND installButton = GetDlgItem(hwnd, IDC_INSTALL_BUTTON);

	int selectedCount = ListView_GetSelectedCount(listView);
	if (selectedCount == 1) {
		int selectedIndex = ListView_GetNextItem(listView, -1, LVNI_SELECTED);
		installationStatus status = GetStatusFromLVI(listView, selectedIndex);
		EnableWindow(editButton, TRUE);
	}
	else {
		EnableWindow(editButton, FALSE);
	}

    EnableWindow(installButton, (ListView_GetItemCount(listView) > 0));
    EnableWindow(removeButton, (ListView_GetSelectedCount(listView) > 0));

	// Disable buttons in certain conditions
	if (selectedCount == 1) {
		int selectedIndex = ListView_GetNextItem(listView, -1, LVNI_SELECTED);
		installationStatus status = GetStatusFromLVI(listView, selectedIndex);
		EnableWindow(installButton, (!(status != Installed) && !(status == Installing)));
		EnableWindow(editButton, (!(status == Installed) && !(status == Installing)));
		EnableWindow(removeButton, (status != Installing));
	}
}

std::wstring OpenFileDialog(HWND hwnd) {
	OPENFILENAME ofn;
	wchar_t szFile[260] = {0};
	ZeroMemory(&ofn, sizeof(ofn));
	ofn.lStructSize = sizeof(ofn);
	ofn.hwndOwner = hwnd;
	ofn.lpstrFile = szFile;
	ofn.nMaxFile = sizeof(szFile);
	ofn.lpstrFilter = L"Driver INF Files\0*.inf\0All Files\0*.*\0";;
	ofn.nFilterIndex = 1;
	ofn.lpstrFileTitle = NULL;
	ofn.nMaxFileTitle = 0;
	ofn.lpstrInitialDir = NULL;
	ofn.Flags = OFN_PATHMUSTEXIST | OFN_FILEMUSTEXIST | OFN_NOCHANGEDIR | OFN_EXPLORER;

	if (GetOpenFileName(&ofn)) {
		return szFile;
	}
	return L"";
}

int CALLBACK ComputerCallbackProc(HWND hwnd, UINT uMsg, LPARAM lParam, LPARAM lpData) {
    if (uMsg == BFFM_INITIALIZED) {
        // Set the initial folder to "My Computer"
        ::SendMessage(hwnd, BFFM_SETSELECTION, TRUE, (LPARAM)L"::{20D04FE0-3AEA-1069-A2D8-08002B30309D}");
    }
    return 0;
}

std::wstring OpenFolderDialog(HWND hwnd) {
    BROWSEINFO bi = {0};
    bi.lpszTitle = L"Select a folder containing driver files";
    bi.ulFlags = BIF_RETURNONLYFSDIRS | BIF_NONEWFOLDERBUTTON | BIF_USENEWUI;
    bi.hwndOwner = hwnd;

    bi.lpfn = ComputerCallbackProc;

    PIDLIST_ABSOLUTE pidl = SHBrowseForFolder(&bi);

    if (pidl != 0) {
        wchar_t path[MAX_PATH];
        SHGetPathFromIDList(pidl, path);
        CoTaskMemFree(pidl);
        return path;
    }
    return L"";
}

void SearchDirectoryForDrivers(HWND hwndList, const std::wstring& folderPath, HWND hEditButton, HWND hRemoveButton, HWND hInstallButton) {
    WIN32_FIND_DATA finder;
    HANDLE hFind = FindFirstFile((folderPath + L"\\*").c_str(), &finder);
    if (hFind != INVALID_HANDLE_VALUE) {
        do {
            if (finder.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) {
                // Skip the current and parent directory entries
                if (wcscmp(finder.cFileName, L".") != 0 && wcscmp(finder.cFileName, L"..") != 0) {
                    // Recursively search the subdirectory
                    SearchDirectoryForDrivers(hwndList, folderPath + L"\\" + finder.cFileName, hEditButton, hRemoveButton, hInstallButton);
                }
            } else {
                if ((finder.dwFileAttributes & FILE_ATTRIBUTE_NORMAL || finder.dwFileAttributes & FILE_ATTRIBUTE_ARCHIVE || finder.dwFileAttributes & FILE_ATTRIBUTE_READONLY) &&
                    std::wstring(finder.cFileName).find(L".inf") != std::wstring::npos) {

                    // Concatenate the folder path with the file name to get the full path
                    std::wstring fullPath = folderPath + L"\\" + finder.cFileName;

                    AddDriver(hwndList, fullPath, hEditButton, hRemoveButton, hInstallButton);
                }
            }
        } while (FindNextFile(hFind, &finder) != 0);
        FindClose(hFind);
    }
}
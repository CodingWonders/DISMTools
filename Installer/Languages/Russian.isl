; *** Inno Setup version 6.5.0+ Russian messages for DISMTools ***
;
; To download user-contributed translations of this file, go to:
;   https://jrsoftware.org/files/istrans/
;
; Note: When translating this text, do not add periods (.) to the end of
; messages that didn't have them already, because on those messages Inno
; Setup adds the periods automatically (appending a period would result in
; two periods being displayed).

[LangOptions]
; The following three entries are very important. Be sure to read and
; understand the '[LangOptions] section' topic in the help file.
LanguageName=Русский
LanguageID=$0419
; LanguageCodePage should always be set if possible, even if this file is Unicode
; For English it's set to zero anyway because English only uses ASCII characters
LanguageCodePage=1251
; If the language you are translating to requires special font faces or
; sizes, uncomment any of the following entries and change them accordingly.
;DialogFontName=
;DialogFontSize=9
;DialogFontBaseScaleWidth=7
;DialogFontBaseScaleHeight=15
;WelcomeFontName=Segoe UI
;WelcomeFontSize=14

[Messages]

; *** Application titles
SetupAppTitle=Установка
SetupWindowTitle=Установка: %1
UninstallAppTitle=Удаление
UninstallAppFullTitle=Удаление %1

; *** Misc. common
InformationTitle=Информация
ConfirmTitle=Подтверждение
ErrorTitle=Ошибка

; *** SetupLdr messages
SetupLdrStartupMessage=Будет установлен %1. Продолжить?
LdrCannotCreateTemp=Не удалось создать временный файл. Установка прервана
LdrCannotExecTemp=Не удалось выполнить файл во временной папке. Установка прервана
HelpTextNote=

; *** Startup error messages
LastErrorMessage=%1.%n%nОшибка %2: %3
SetupFileMissing=Файл %1 отсутствует в папке установки. Исправьте проблему или получите новую копию программы.
SetupFileCorrupt=Файлы установки повреждены. Получите новую копию программы.
SetupFileCorruptOrWrongVer=Файлы установки повреждены или несовместимы с этой версией установщика. Исправьте проблему или получите новую копию программы.
InvalidParameter=В командной строке передан недопустимый параметр:%n%n%1
SetupAlreadyRunning=Установщик уже запущен.
WindowsVersionNotSupported=Эта программа не поддерживает версию Windows, установленную на этом компьютере.
WindowsServicePackRequired=Для этой программы требуется %1 Service Pack %2 или новее.
NotOnThisPlatform=Эта программа не запускается на %1.
OnlyOnThisPlatform=Эта программа должна запускаться только на %1.
OnlyOnTheseArchitectures=Эту программу можно установить только в версиях Windows для следующих архитектур процессора:%n%n%1
WinVersionTooLowError=Для этой программы требуется %1 версии %2 или новее.
WinVersionTooHighError=Эту программу нельзя установить на %1 версии %2 или новее.
AdminPrivilegesRequired=Для установки программы необходимо войти в систему с правами администратора.
PowerUserPrivilegesRequired=Для установки программы необходимо войти в систему с правами администратора или участника группы опытных пользователей.
SetupAppRunningError=Установщик обнаружил, что %1 сейчас запущен.%n%nЗакройте все экземпляры программы и нажмите OK для продолжения или Отмена для выхода.
UninstallAppRunningError=Удаление обнаружило, что %1 сейчас запущен.%n%nЗакройте все экземпляры программы и нажмите OK для продолжения или Отмена для выхода.

; *** Startup questions
PrivilegesRequiredOverrideTitle=Выбор режима установки
PrivilegesRequiredOverrideInstruction=Выберите режим установки
PrivilegesRequiredOverrideText1=%1 можно установить для всех пользователей, что требует прав администратора, или только для вас.
PrivilegesRequiredOverrideText2=%1 можно установить только для вас или для всех пользователей, что требует прав администратора.
PrivilegesRequiredOverrideAllUsers=Установить для &всех пользователей
PrivilegesRequiredOverrideAllUsersRecommended=Установить для &всех пользователей, рекомендуется
PrivilegesRequiredOverrideCurrentUser=Установить только для &меня
PrivilegesRequiredOverrideCurrentUserRecommended=Установить только для &меня, рекомендуется

; *** Misc. errors
ErrorCreatingDir=Установщик не смог создать папку "%1"
ErrorTooManyFilesInDir=Не удалось создать файл в папке "%1", потому что в ней слишком много файлов

; *** Setup common messages
ExitSetupTitle=Выход из установщика
ExitSetupMessage=Установка не завершена. Если выйти сейчас, программа не будет установлена.%n%nВы можете запустить установщик позже, чтобы завершить установку.%n%nВыйти из установщика?
AboutSetupMenuItem=&О программе установки...
AboutSetupTitle=О программе установки
AboutSetupMessage=%1 версия %2%n%3%n%nДомашняя страница %1:%n%4
AboutSetupNote=
TranslatorNote=

; *** Buttons
ButtonBack=< &Назад
ButtonNext=&Далее >
ButtonInstall=&Установить
ButtonOK=OK
ButtonCancel=Отмена
ButtonYes=&Да
ButtonYesToAll=Да для &всех
ButtonNo=&Нет
ButtonNoToAll=Н&ет для всех
ButtonFinish=&Готово
ButtonBrowse=&Обзор...
ButtonWizardBrowse=О&бзор...
ButtonNewFolder=&Создать папку

; *** "Select Language" dialog messages
SelectLanguageTitle=Выбор языка установки
SelectLanguageLabel=Выберите язык, который будет использоваться во время установки.

; *** Common wizard text
ClickNext=Нажмите Далее, чтобы продолжить, или Отмена, чтобы выйти из установщика.
BeveledLabel=
BrowseDialogTitle=Выбор папки
BrowseDialogLabel=Выберите папку в списке ниже и нажмите OK.
NewFolderName=Новая папка

; *** "Welcome" wizard page
WelcomeLabel1=Добро пожаловать в мастер установки [name]
WelcomeLabel2=[name/ver] будет установлен на этот компьютер.%n%nПеред продолжением рекомендуется закрыть все остальные приложения.

; *** "Password" wizard page
WizardPassword=Пароль
PasswordLabel1=Эта установка защищена паролем.
PasswordLabel3=Введите пароль и нажмите Далее для продолжения. Регистр букв имеет значение.
PasswordEditLabel=&Пароль:
IncorrectPassword=Введён неверный пароль. Попробуйте ещё раз.

; *** "License Agreement" wizard page
WizardLicense=Лицензионное соглашение
LicenseLabel=Перед продолжением прочитайте следующую важную информацию.
LicenseLabel3=Перед продолжением установки прочитайте лицензионное соглашение и примите его условия.
LicenseAccepted=Я &принимаю соглашение
LicenseNotAccepted=Я &не принимаю соглашение

; *** "Information" wizard pages
WizardInfoBefore=Информация
InfoBeforeLabel=Перед продолжением прочитайте следующую важную информацию.
InfoBeforeClickLabel=Когда будете готовы продолжить установку, нажмите Далее.
WizardInfoAfter=Информация
InfoAfterLabel=Перед продолжением прочитайте следующую важную информацию.
InfoAfterClickLabel=Когда будете готовы продолжить установку, нажмите Далее.

; *** "User Information" wizard page
WizardUserInfo=Информация о пользователе
UserInfoDesc=Введите информацию о себе.
UserInfoName=&Имя пользователя:
UserInfoOrg=&Организация:
UserInfoSerial=&Серийный номер:
UserInfoNameRequired=Необходимо ввести имя.

; *** "Select Destination Location" wizard page
WizardSelectDir=Выбор папки установки
SelectDirDesc=Где установить [name]?
SelectDirLabel3=Установщик установит [name] в следующую папку.
SelectDirBrowseLabel=Чтобы продолжить, нажмите Далее. Чтобы выбрать другую папку, нажмите Обзор.
DiskSpaceGBLabel=Для установки требуется как минимум [gb] ГБ свободного места.
DiskSpaceMBLabel=Для установки требуется как минимум [mb] МБ свободного места.
CannotInstallToNetworkDrive=Установщик не может установить программу на сетевой диск.
CannotInstallToUNCPath=Установщик не может установить программу по UNC пути.
InvalidPath=Необходимо указать полный путь с буквой диска, например:%n%nC:\APP%n%nили UNC путь в формате:%n%n\\server\share
InvalidDrive=Выбранный диск или UNC ресурс не существует либо недоступен. Выберите другой путь.
DiskSpaceWarningTitle=Недостаточно места на диске
DiskSpaceWarning=Для установки нужно как минимум %1 КБ свободного места, но на выбранном диске доступно только %2 КБ.%n%nПродолжить всё равно?
DirNameTooLong=Имя папки или путь слишком длинные.
InvalidDirName=Имя папки недопустимо.
BadDirName32=Имена папок не могут содержать следующие символы:%n%n%1
DirExistsTitle=Папка уже существует
DirExists=Папка:%n%n%1%n%nуже существует. Установить программу в эту папку?
DirDoesntExistTitle=Папка не существует
DirDoesntExist=Папка:%n%n%1%n%nне существует. Создать эту папку?

; *** "Select Components" wizard page
WizardSelectComponents=Выбор компонентов
SelectComponentsDesc=Какие компоненты установить?
SelectComponentsLabel2=Выберите компоненты для установки. Снимите выбор с компонентов, которые устанавливать не нужно. Нажмите Далее для продолжения.
FullInstallation=Полная установка
; if possible don't translate 'Compact' as 'Minimal' (I mean 'Minimal' in your language)
CompactInstallation=Компактная установка
CustomInstallation=Выборочная установка
NoUninstallWarningTitle=Компоненты уже существуют
NoUninstallWarning=Установщик обнаружил, что следующие компоненты уже установлены на этом компьютере:%n%n%1%n%nЕсли снять выбор с этих компонентов, они не будут удалены.%n%nПродолжить всё равно?
ComponentSize1=%1 KB
ComponentSize2=%1 MB
ComponentsDiskSpaceGBLabel=Для текущего выбора требуется как минимум [gb] ГБ свободного места.
ComponentsDiskSpaceMBLabel=Для текущего выбора требуется как минимум [mb] МБ свободного места.

; *** "Select Additional Tasks" wizard page
WizardSelectTasks=Выбор дополнительных задач
SelectTasksDesc=Какие дополнительные задачи выполнить?
SelectTasksLabel2=Выберите дополнительные задачи, которые нужно выполнить при установке [name], затем нажмите Далее.

; *** "Select Start Menu Folder" wizard page
WizardSelectProgramGroup=Выбор папки в меню Пуск
SelectStartMenuFolderDesc=Где установщик должен создать ярлыки программы?
SelectStartMenuFolderLabel3=Установщик создаст ярлыки программы в следующей папке меню Пуск.
SelectStartMenuFolderBrowseLabel=Чтобы продолжить, нажмите Далее. Чтобы выбрать другую папку, нажмите Обзор.
MustEnterGroupName=Необходимо ввести имя папки.
GroupNameTooLong=Имя папки или путь слишком длинные.
InvalidGroupName=Имя папки недопустимо.
BadGroupName=Имя папки не может содержать следующие символы:%n%n%1
NoProgramGroupCheck2=&Не создавать папку в меню Пуск

; *** "Ready to Install" wizard page
WizardReady=Готово к установке
ReadyLabel1=Установщик готов установить [name] на ваш компьютер.
ReadyLabel2a=Нажмите Установить для продолжения или Назад, чтобы проверить либо изменить параметры.
ReadyLabel2b=Нажмите Установить, чтобы продолжить установку.
ReadyMemoUserInfo=Информация о пользователе:
ReadyMemoDir=Папка установки:
ReadyMemoType=Тип установки:
ReadyMemoComponents=Выбранные компоненты:
ReadyMemoGroup=Папка меню Пуск:
ReadyMemoTasks=Дополнительные задачи:

; *** TDownloadWizardPage wizard page and DownloadTemporaryFile
DownloadingLabel2=Загрузка файлов...
ButtonStopDownload=&Остановить загрузку
StopDownload=Остановить загрузку?
ErrorDownloadAborted=Загрузка прервана
ErrorDownloadFailed=Ошибка загрузки: %1 %2
ErrorDownloadSizeFailed=Не удалось получить размер: %1 %2
ErrorProgress=Недопустимый прогресс: %1 из %2
ErrorFileSize=Недопустимый размер файла: ожидалось %1, найдено %2

; *** TExtractionWizardPage wizard page and ExtractArchive
ExtractingLabel=Извлечение файлов...
ButtonStopExtraction=&Остановить извлечение
StopExtraction=Остановить извлечение?
ErrorExtractionAborted=Извлечение прервано
ErrorExtractionFailed=Ошибка извлечения: %1

; *** Archive extraction failure details
ArchiveIncorrectPassword=Неверный пароль
ArchiveIsCorrupted=Архив повреждён
ArchiveUnsupportedFormat=Формат архива не поддерживается

; *** "Preparing to Install" wizard page
WizardPreparing=Подготовка к установке
PreparingDesc=Установщик подготавливает установку [name] на ваш компьютер.
PreviousInstallNotCompleted=Предыдущая установка или удаление программы не были завершены. Необходимо перезапустить компьютер, чтобы завершить эту операцию.%n%nПосле перезапуска компьютера снова запустите установщик, чтобы завершить установку [name].
CannotContinue=Установщик не может продолжить. Нажмите Отмена для выхода.
ApplicationsFound=Следующие приложения используют файлы, которые нужно обновить. Рекомендуется разрешить установщику автоматически закрыть эти приложения.
ApplicationsFound2=Следующие приложения используют файлы, которые нужно обновить. Рекомендуется разрешить установщику автоматически закрыть эти приложения. После завершения установки установщик попытается снова запустить приложения.
CloseApplications=&Автоматически закрыть приложения
DontCloseApplications=&Не закрывать приложения
ErrorCloseApplications=Установщик не смог автоматически закрыть все приложения. Перед продолжением рекомендуется закрыть все приложения, которые используют файлы, требующие обновления.
PrepareToInstallNeedsRestart=Установщик должен перезапустить компьютер. После перезапуска снова запустите установщик, чтобы завершить установку [name].%n%nПерезапустить сейчас?

; *** "Installing" wizard page
WizardInstalling=Установка
InstallingLabel=Подождите, пока [name] устанавливается на ваш компьютер.

; *** "Setup Completed" wizard page
FinishedHeadingLabel=Завершение мастера установки [name]
FinishedLabelNoIcons=Установка [name] на компьютер завершена.
FinishedLabel=Установка [name] на компьютер завершена. Приложение можно запустить через установленные ярлыки.
ClickFinish=Нажмите Готово, чтобы выйти из установщика.
FinishedRestartLabel=Для завершения установки [name] установщик должен перезапустить компьютер. Перезапустить сейчас?
FinishedRestartMessage=Для завершения установки [name] установщик должен перезапустить компьютер.%n%nПерезапустить сейчас?
ShowReadmeCheck=Да, открыть файл README
YesRadio=&Да, перезапустить компьютер сейчас
NoRadio=&Нет, я перезапущу компьютер позже
; used for example as 'Run MyProg.exe'
RunEntryExec=Запустить %1
; used for example as 'View Readme.txt'
RunEntryShellExec=Открыть %1

; *** "Setup Needs the Next Disk" stuff
ChangeDiskTitle=Установщику нужен следующий диск
SelectDiskLabel2=Вставьте диск %1 и нажмите OK.%n%nЕсли файлы на этом диске находятся в другой папке, укажите правильный путь или нажмите Обзор.
PathLabel=&Путь:
FileNotInDir2=Файл "%1" не найден в "%2". Вставьте правильный диск или выберите другую папку.
SelectDirectoryLabel=Укажите расположение следующего диска.

; *** Installation phase messages
SetupAborted=Установка не завершена.%n%nИсправьте проблему и снова запустите установщик.
AbortRetryIgnoreSelectAction=Выбор действия
AbortRetryIgnoreRetry=&Повторить
AbortRetryIgnoreIgnore=&Игнорировать ошибку и продолжить
AbortRetryIgnoreCancel=Отменить установку
RetryCancelSelectAction=Выбор действия
RetryCancelRetry=&Повторить
RetryCancelCancel=Отмена

; *** Installation status messages
StatusClosingApplications=Закрытие приложений...
StatusCreateDirs=Создание папок...
StatusExtractFiles=Извлечение файлов...
StatusDownloadFiles=Загрузка файлов...
StatusCreateIcons=Создание ярлыков...
StatusCreateIniEntries=Создание записей INI...
StatusCreateRegistryEntries=Создание записей реестра...
StatusRegisterFiles=Регистрация файлов...
StatusSavingUninstall=Сохранение информации для удаления...
StatusRunProgram=Завершение установки...
StatusRestartingApplications=Перезапуск приложений...
StatusRollback=Откат изменений...

; *** Misc. errors
ErrorInternal2=Внутренняя ошибка: %1
ErrorFunctionFailedNoCode=Сбой выполнения %1
ErrorFunctionFailed=Сбой выполнения %1, код %2
ErrorFunctionFailedWithMessage=Сбой выполнения %1, код %2.%n%3
ErrorExecutingProgram=Не удалось выполнить файл:%n%1

; *** Registry errors
ErrorRegOpenKey=Ошибка открытия ключа реестра:%n%1\%2
ErrorRegCreateKey=Ошибка создания ключа реестра:%n%1\%2
ErrorRegWriteKey=Ошибка записи в ключ реестра:%n%1\%2

; *** INI errors
ErrorIniEntry=Ошибка создания записи INI в файле "%1".

; *** File copying errors
FileAbortRetryIgnoreSkipNotRecommended=&Пропустить этот файл, не рекомендуется
FileAbortRetryIgnoreIgnoreNotRecommended=&Игнорировать ошибку и продолжить, не рекомендуется
SourceIsCorrupted=Исходный файл повреждён
SourceDoesntExist=Исходный файл "%1" не существует
SourceVerificationFailed=Проверка исходного файла не пройдена: %1
VerificationSignatureDoesntExist=Файл подписи "%1" не существует
VerificationSignatureInvalid=Файл подписи "%1" недействителен
VerificationKeyNotFound=Файл подписи "%1" использует неизвестный ключ
VerificationFileNameIncorrect=Имя файла неверное
VerificationFileTagIncorrect=Тег файла неверный
VerificationFileSizeIncorrect=Размер файла неверный
VerificationFileHashIncorrect=Хеш файла неверный
ExistingFileReadOnly2=Существующий файл не удалось заменить, потому что он помечен как доступный только для чтения.
ExistingFileReadOnlyRetry=&Убрать атрибут только для чтения и повторить
ExistingFileReadOnlyKeepExisting=&Оставить существующий файл
ErrorReadingExistingDest=Произошла ошибка при чтении существующего файла:
FileExistsSelectAction=Выбор действия
FileExists2=Файл уже существует.
FileExistsOverwriteExisting=&Заменить существующий файл
FileExistsKeepExisting=&Оставить существующий файл
FileExistsOverwriteOrKeepAll=&Применить это действие к следующим конфликтам
ExistingFileNewerSelectAction=Выбор действия
ExistingFileNewer2=Существующий файл новее файла, который установщик пытается установить.
ExistingFileNewerOverwriteExisting=&Заменить существующий файл
ExistingFileNewerKeepExisting=&Оставить существующий файл, рекомендуется
ExistingFileNewerOverwriteOrKeepAll=&Применить это действие к следующим конфликтам
ErrorChangingAttr=Произошла ошибка при изменении атрибутов существующего файла:
ErrorCreatingTemp=Произошла ошибка при создании файла в папке назначения:
ErrorReadingSource=Произошла ошибка при чтении исходного файла:
ErrorCopying=Произошла ошибка при копировании файла:
ErrorDownloading=Произошла ошибка при загрузке файла:
ErrorExtracting=Произошла ошибка при извлечении архива:
ErrorReplacingExistingFile=Произошла ошибка при замене существующего файла:
ErrorRestartReplace=Сбой RestartReplace:
ErrorRenamingTemp=Произошла ошибка при переименовании файла в папке назначения:
ErrorRegisterServer=Не удалось зарегистрировать DLL/OCX: %1
ErrorRegSvr32Failed=Сбой RegSvr32 с кодом выхода %1
ErrorRegisterTypeLib=Не удалось зарегистрировать библиотеку типов: %1

; *** Uninstall display name markings
; used for example as 'My Program (32-bit)'
UninstallDisplayNameMark=%1 (%2)
; used for example as 'My Program (32-bit, All users)'
UninstallDisplayNameMarks=%1 (%2, %3)
UninstallDisplayNameMark32Bit=32 битная
UninstallDisplayNameMark64Bit=64 битная
UninstallDisplayNameMarkAllUsers=Все пользователи
UninstallDisplayNameMarkCurrentUser=Текущий пользователь

; *** Post-installation errors
ErrorOpeningReadme=Произошла ошибка при попытке открыть файл README.
ErrorRestartingComputer=Установщик не смог перезапустить компьютер. Сделайте это вручную.

; *** Uninstaller messages
UninstallNotFound=Файл "%1" не существует. Удаление невозможно.
UninstallOpenError=Файл "%1" не удалось открыть. Удаление невозможно
UninstallUnsupportedVer=Файл журнала удаления "%1" имеет формат, который не распознан этой версией деинсталлятора. Удаление невозможно
UninstallUnknownEntry=В журнале удаления обнаружена неизвестная запись (%1)
ConfirmUninstall=Вы действительно хотите полностью удалить %1 и все его компоненты?
UninstallOnlyOnWin64=Эту установку можно удалить только в 64 битной Windows.
OnlyAdminCanUninstall=Эту установку может удалить только пользователь с правами администратора.
UninstallStatusLabel=Подождите, пока %1 удаляется с компьютера.
UninstalledAll=%1 успешно удалён с компьютера.
UninstalledMost=Удаление %1 завершено.%n%nНекоторые элементы удалить не удалось. Их можно удалить вручную.
UninstalledAndNeedsRestart=Чтобы завершить удаление %1, компьютер нужно перезапустить.%n%nПерезапустить сейчас?
UninstallDataCorrupted=Файл "%1" повреждён. Удаление невозможно

; *** Uninstallation phase messages
ConfirmDeleteSharedFileTitle=Удалить общий файл?
ConfirmDeleteSharedFile2=Система указывает, что следующий общий файл больше не используется программами. Удалить этот общий файл?%n%nЕсли какие либо программы всё ещё используют этот файл и он будет удалён, они могут работать неправильно. Если вы не уверены, выберите Нет. Если оставить файл в системе, это не причинит вреда.
SharedFileNameLabel=Имя файла:
SharedFileLocationLabel=Расположение:
WizardUninstalling=Состояние удаления
StatusUninstalling=Удаление %1...

; *** Shutdown block reasons
ShutdownBlockReasonInstallingApp=Установка %1.
ShutdownBlockReasonUninstallingApp=Удаление %1.

; The custom messages below aren't used by Setup itself, but if you make
; use of them in your scripts, you'll want to translate them.

[CustomMessages]

NameAndVersion=%1 версия %2
AdditionalIcons=Дополнительные ярлыки:
CreateDesktopIcon=Создать ярлык на &рабочем столе
CreateQuickLaunchIcon=Создать ярлык в панели &быстрого запуска
ProgramOnTheWeb=%1 в интернете
UninstallProgram=Удалить %1
LaunchProgram=Запустить %1
AssocFileExtension=&Связать %1 с расширением файлов %2
AssocingFileExtension=Связывание %1 с расширением файлов %2...
AutoStartProgramGroupDescription=Автозагрузка:
AutoStartProgram=Автоматически запускать %1
AddonHostProgramNotFound=%1 не найден в выбранной папке.%n%nПродолжить всё равно?
InstallerWord=Установщик
UninstallerWord=Деинсталлятор
AutoReloadTaskDescription=Установить службу автоматической перезагрузки образов
ServicesTaskGroup=Службы
AutoReloadServiceDisplayName=Служба автоматической перезагрузки образов DISMTools
AutoReloadServiceDescription=Эта служба автоматически перезагружает сеансы обслуживания всех смонтированных образов на этом компьютере. Её можно отключить, если она не нужна.

Imports System.Windows.Forms
Imports System.IO
Imports Microsoft.VisualBasic.ControlChars
Imports System.Globalization
Imports Microsoft.Win32

Public Class Options

    Dim DismVersion As FileVersionInfo
    Dim CanExit As Boolean
    Dim SaveLocations() As String = New String(1) {"", ""}
    Dim ColorModes() As String = New String(2) {"", "", ""}
    Dim LogViews() As String = New String(1) {"", ""}
    Dim NotFreqs() As String = New String(1) {"", ""}

    Public SectionNum As Integer = 0

    Private isInitializingForm As Boolean = True
    Private isApplyingLocalizedText As Boolean = False
    Private originalLanguage As String = LocalizationService.DefaultCultureCode

    Public Sub New()
        isInitializingForm = True
        InitializeComponent()
        isInitializingForm = False
    End Sub

    Private AutoReloadServiceInstalled As Boolean
    Private AutoReloadService As WindowsService

    Private Class ProcessorFamilyCategory

        Public Property Beefiness As ProcessorFamilyBeefiness
        Public Property Family As Integer

        Public Sub New(family As Integer, beefiness As ProcessorFamilyBeefiness)
            Me.Family = family
            Me.Beefiness = beefiness
        End Sub

    End Class

    Private Enum ProcessorFamilyBeefiness As Integer
        Potato = 0
        Average = 1
        Beefy = 2
    End Enum

    ' Processor families that we know the performance of. Family numbers derive from
    ' version 3.9 of the SMBIOS specification, from 2025:
    ' https://www.dmtf.org/sites/default/files/standards/documents/DSP0134_3.9.0.pdf
    Private Const CPU_INTEL_PENTIUM_3 As Integer = 17,
                  CPU_INTEL_CELERON_M As Integer = 20,
                  CPU_INTEL_PENTIUM_4_HT As Integer = 21,
                  CPU_INTEL As Integer = 22,
                  CPU_INTEL_CORE_DUO As Integer = 40,
                  CPU_INTEL_CORE_DUO_M As Integer = 41,
                  CPU_INTEL_CORE_SOLO As Integer = 42,
                  CPU_INTEL_ATOM As Integer = 43,
                  CPU_INTEL_CORE_M As Integer = 44,
                  CPU_INTEL_CORE_M3 As Integer = 45,
                  CPU_INTEL_CORE_M5 As Integer = 46,
                  CPU_INTEL_CORE_M7 As Integer = 47,
                  CPU_AMD_TURION_2_ULTRA_DUALCORE_MOBILE_M As Integer = 56,
                  CPU_AMD_TURION_2_DUALCORE_MOBILE_M As Integer = 57,
                  CPU_AMD_ATHLON_2_DUALCORE_M As Integer = 58,
                  CPU_AMD_OPTERON_6100 As Integer = 59,
                  CPU_AMD_OPTERON_4100 As Integer = 60,
                  CPU_AMD_OPTERON_6200 As Integer = 61,
                  CPU_AMD_OPTERON_4200 As Integer = 62,
                  CPU_AMD_FX As Integer = 63,
                  CPU_AMD_C As Integer = 70,
                  CPU_AMD_E As Integer = 71,
                  CPU_AMD_A As Integer = 72,
                  CPU_AMD_G As Integer = 73,
                  CPU_AMD_Z As Integer = 74,
                  CPU_AMD_R As Integer = 75,
                  CPU_AMD_OPTERON_4300 As Integer = 76,
                  CPU_AMD_OPTERON_6300 As Integer = 77,
                  CPU_AMD_OPTERON_3300 As Integer = 78,
                  CPU_AMD_FIREPRO As Integer = 79,
                  CPU_AMD_ATHLON_X4_QUADCORE As Integer = 102,
                  CPU_AMD_OPTERON_X1000 As Integer = 103,
                  CPU_AMD_OPTERON_X2000_APU As Integer = 104,
                  CPU_AMD_OPTERON_A As Integer = 105,
                  CPU_AMD_OPTERON_X3000_APU As Integer = 106,
                  CPU_AMD_ZEN As Integer = 107,
                  CPU_AMD_ATHLON_64 As Integer = 131,
                  CPU_AMD_OPTERON As Integer = 132,
                  CPU_AMD_SEMPRON As Integer = 133,
                  CPU_AMD_TURION_64_MOBILE As Integer = 134,
                  CPU_AMD_OPTERON_DUALCORE As Integer = 135,
                  CPU_AMD_ATHLON_64_X2_DUALCORE As Integer = 136,
                  CPU_AMD_TURION_64_X2_MOBILE As Integer = 137,
                  CPU_AMD_OPTERON_QUADCORE As Integer = 138,
                  CPU_AMD_OPTERON_MK3 As Integer = 139,
                  CPU_AMD_PHENOM_FX_QUADCORE As Integer = 140,
                  CPU_AMD_PHENOM_X4_QUADCORE As Integer = 141,
                  CPU_AMD_PHENOM_X2_DUALCORE As Integer = 142,
                  CPU_AMD_ATHLON_X2_DUALCORE As Integer = 143,
                  CPU_INTEL_XEON_3200_QUADCORE As Integer = 161,
                  CPU_INTEL_XEON_3000_DUALCORE As Integer = 162,
                  CPU_INTEL_XEON_5300_QUADCORE As Integer = 163,
                  CPU_INTEL_XEON_5100_DUALCORE As Integer = 164,
                  CPU_INTEL_XEON_5000_DUALCORE As Integer = 165,
                  CPU_INTEL_XEON_LV_DUALCORE As Integer = 166,
                  CPU_INTEL_XEON_ULV_DUALCORE As Integer = 167,
                  CPU_INTEL_XEON_7100_DUALCORE As Integer = 168,
                  CPU_INTEL_XEON_5400_QUADCORE As Integer = 169,
                  CPU_INTEL_XEON_QUADCORE As Integer = 170,
                  CPU_INTEL_XEON_5200_DUALCORE As Integer = 171,
                  CPU_INTEL_XEON_7200_DUALCORE As Integer = 172,
                  CPU_INTEL_XEON_7300_QUADCORE As Integer = 173,
                  CPU_INTEL_XEON_7400_QUADCORE As Integer = 174,
                  CPU_INTEL_XEON_7400_MULTICORE As Integer = 175,
                  CPU_INTEL_PENTIUM3_XEON As Integer = 176,
                  CPU_INTEL_PENTIUM3_SPEEDSTEP As Integer = 177,
                  CPU_INTEL_PENTIUM4 As Integer = 178,
                  CPU_INTEL_XEON As Integer = 179,
                  CPU_INTEL_XEON_MP As Integer = 181,
                  CPU_AMD_ATHLON_XP As Integer = 182,
                  CPU_AMD_ATHLON_MP As Integer = 183,
                  CPU_INTEL_PENTIUM_M As Integer = 185,
                  CPU_INTEL_CELERON_D As Integer = 186,
                  CPU_INTEL_PENTIUM_D As Integer = 187,
                  CPU_INTEL_PENTIUM_D_EXTREME As Integer = 188,
                  CPU_INTEL_CORE_SOLO_2 As Integer = 189,
                  CPU_INTEL_CORE2_DUO As Integer = 191,
                  CPU_INTEL_CORE2_SOLO As Integer = 192,
                  CPU_INTEL_CORE2_EXTREME As Integer = 193,
                  CPU_INTEL_CORE2_QUAD As Integer = 194,
                  CPU_INTEL_CORE2_EXTREME_M As Integer = 195,
                  CPU_INTEL_CORE2_DUO_M As Integer = 196,
                  CPU_INTEL_CORE2_SOLO_M As Integer = 197,
                  CPU_INTEL_CORE_I7 As Integer = 198,
                  CPU_INTEL_CELERON_DUALCORE As Integer = 199,
                  CPU_INTEL_CORE_I5 As Integer = 205,
                  CPU_INTEL_CORE_I3 As Integer = 206,
                  CPU_INTEL_CORE_I9 As Integer = 207,
                  CPU_INTEL_XEON_D As Integer = 208,
                  CPU_INTEL_XEON_3400_MULTICORE As Integer = 224,
                  CPU_AMD_OPTERON_3000 As Integer = 228,
                  CPU_AMD_SEMPRON_2 As Integer = 229,
                  CPU_AMD_OPTERON_QUADCORE_EMBEDDED As Integer = 230,
                  CPU_AMD_PHENOM_TRICORE As Integer = 231,
                  CPU_AMD_TURION_ULTRA_DUALCORE_MOBILE As Integer = 232,
                  CPU_AMD_TURION_DUALCORE_MOBILE As Integer = 233,
                  CPU_AMD_ATHLON_DUALCORE As Integer = 234,
                  CPU_AMD_SEMPRON_SI As Integer = 235,
                  CPU_AMD_PHENOM_2 As Integer = 236,
                  CPU_AMD_ATHLON_2 As Integer = 237,
                  CPU_AMD_OPTERON_SIXCORE As Integer = 238,
                  CPU_AMD_SEMPRON_M As Integer = 239,
                  CPU_ARMV7 As Integer = 256,
                  CPU_ARMV8 As Integer = 257,
                  CPU_ARMV9 As Integer = 258,
                  CPU_ARMRESERVED As Integer = 259,
                  CPU_INTEL_CORE_3_RAPTORLAKE As Integer = 768,
                  CPU_INTEL_CORE_5_RAPTORLAKE As Integer = 769,
                  CPU_INTEL_CORE_7_RAPTORLAKE As Integer = 770,
                  CPU_INTEL_CORE_9_RAPTORLAKE As Integer = 771,
                  CPU_INTEL_CORE_ULTRA3 As Integer = 772,
                  CPU_INTEL_CORE_ULTRA5 As Integer = 773,
                  CPU_INTEL_CORE_ULTRA7 As Integer = 774,
                  CPU_INTEL_CORE_ULTRA9 As Integer = 775

    Private SpecialProcessorFamilies As New List(Of ProcessorFamilyCategory) From {
        New ProcessorFamilyCategory(CPU_INTEL_PENTIUM_3, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_CELERON_M, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_PENTIUM_4_HT, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_DUO, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_DUO_M, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_SOLO, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_ATOM, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_M, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_M3, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_M5, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_M7, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_TURION_2_ULTRA_DUALCORE_MOBILE_M, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_TURION_2_DUALCORE_MOBILE_M, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_ATHLON_2_DUALCORE_M, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_6100, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_4100, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_6200, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_4200, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_FX, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_C, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_E, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_A, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_G, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_Z, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_R, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_4300, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_6300, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_3300, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_FIREPRO, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_ATHLON_X4_QUADCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_X1000, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_X2000_APU, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_A, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_X3000_APU, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_ZEN, ProcessorFamilyBeefiness.Beefy),
        New ProcessorFamilyCategory(CPU_AMD_ATHLON_64, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_SEMPRON, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_TURION_64_MOBILE, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_DUALCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_ATHLON_64_X2_DUALCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_TURION_64_X2_MOBILE, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_QUADCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_MK3, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_PHENOM_FX_QUADCORE, ProcessorFamilyBeefiness.Beefy),
        New ProcessorFamilyCategory(CPU_AMD_PHENOM_X4_QUADCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_PHENOM_X2_DUALCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_ATHLON_X2_DUALCORE, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_3200_QUADCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_3000_DUALCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_5300_QUADCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_5100_DUALCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_5000_DUALCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_LV_DUALCORE, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_ULV_DUALCORE, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_7100_DUALCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_5400_QUADCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_QUADCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_5200_DUALCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_7200_DUALCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_7300_QUADCORE, ProcessorFamilyBeefiness.Beefy),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_7400_QUADCORE, ProcessorFamilyBeefiness.Beefy),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_7400_MULTICORE, ProcessorFamilyBeefiness.Beefy),
        New ProcessorFamilyCategory(CPU_INTEL_PENTIUM3_XEON, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_PENTIUM3_SPEEDSTEP, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_PENTIUM4, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_XEON, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_MP, ProcessorFamilyBeefiness.Beefy),
        New ProcessorFamilyCategory(CPU_AMD_ATHLON_XP, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_ATHLON_MP, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_PENTIUM_M, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_CELERON_D, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_PENTIUM_D, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_PENTIUM_D_EXTREME, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_SOLO_2, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_CORE2_DUO, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_CORE2_SOLO, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_CORE2_EXTREME, ProcessorFamilyBeefiness.Beefy),
        New ProcessorFamilyCategory(CPU_INTEL_CORE2_QUAD, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_CORE2_EXTREME_M, ProcessorFamilyBeefiness.Beefy),
        New ProcessorFamilyCategory(CPU_INTEL_CORE2_DUO_M, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_CORE2_SOLO_M, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_I7, ProcessorFamilyBeefiness.Beefy),
        New ProcessorFamilyCategory(CPU_INTEL_CELERON_DUALCORE, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_I5, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_I3, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_I9, ProcessorFamilyBeefiness.Beefy),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_D, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_XEON_3400_MULTICORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_3000, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_SEMPRON_2, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_QUADCORE_EMBEDDED, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_PHENOM_TRICORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_TURION_ULTRA_DUALCORE_MOBILE, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_TURION_DUALCORE_MOBILE, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_ATHLON_DUALCORE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_SEMPRON_SI, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_AMD_PHENOM_2, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_ATHLON_2, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_AMD_OPTERON_SIXCORE, ProcessorFamilyBeefiness.Beefy),
        New ProcessorFamilyCategory(CPU_AMD_SEMPRON_M, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_ARMV7, ProcessorFamilyBeefiness.Potato),
        New ProcessorFamilyCategory(CPU_ARMV8, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_ARMV9, ProcessorFamilyBeefiness.Beefy),
        New ProcessorFamilyCategory(CPU_ARMRESERVED, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_3_RAPTORLAKE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_5_RAPTORLAKE, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_7_RAPTORLAKE, ProcessorFamilyBeefiness.Beefy),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_9_RAPTORLAKE, ProcessorFamilyBeefiness.Beefy),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_ULTRA3, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_ULTRA5, ProcessorFamilyBeefiness.Average),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_ULTRA7, ProcessorFamilyBeefiness.Beefy),
        New ProcessorFamilyCategory(CPU_INTEL_CORE_ULTRA9, ProcessorFamilyBeefiness.Beefy)
    }

    Private Sub DetermineSettingValidity()
        DynaLog.LogMessage("Validating settings...")
        If TextBox1.Text = "" Then
            DynaLog.LogMessage("No DISM executable has been specified.")
            CanExit = False
            GiveErrorExplanation(1)
        Else
            DynaLog.LogMessage("A DISM executable was specified.")
            DynaLog.LogMessage("Provided DISM executable: " & Quote & TextBox1.Text & Quote)
            DynaLog.LogMessage("Checking if provided DISM executable exists...")
            If File.Exists(TextBox1.Text) Then
                DynaLog.LogMessage("The DISM executable exists.")
                CanExit = True
            Else
                DynaLog.LogMessage("The DISM executable does not exist.")
                CanExit = False
                GiveErrorExplanation(2)
            End If
        End If
        If TextBox2.Text = "" Then
            DynaLog.LogMessage("No log file has been specified.")
            CanExit = False
            GiveErrorExplanation(3)
        Else
            DynaLog.LogMessage("A log file was specified.")
            DynaLog.LogMessage("Provided log file: " & Quote & TextBox2.Text & Quote)
            DynaLog.LogMessage("Checking if provided log file exists...")
            If File.Exists(TextBox2.Text) Then
                DynaLog.LogMessage("The log file exists.")
                CanExit = True
            Else
                DynaLog.LogMessage("The log file does not exist. Attempting to create it...")
                Try
                    If Not Directory.Exists(Path.GetDirectoryName(TextBox2.Text)) Then
                        DynaLog.LogMessage("The parent directory of the provided log file does not exist. Attempting to create it...")
                        Directory.CreateDirectory(Path.GetDirectoryName(TextBox2.Text))
                    End If
                    File.Create(TextBox2.Text)
                    CanExit = True
                Catch ex As Exception
                    DynaLog.LogMessage("Could not create log file. Error message: " & ex.Message)
                    CanExit = False
                    GiveErrorExplanation(4)
                End Try
            End If
        End If
        If CheckBox4.Checked Then
            DynaLog.LogMessage("Checking scratch directory settings...")
            If RadioButton3.Checked Then
                DynaLog.LogMessage("The scratch directory provided by the program or the project will be used. Skipping check...")
                CanExit = True
                Exit Sub
            End If
            If TextBox3.Text = "" Then
                DynaLog.LogMessage("No scratch directory has been specified.")
                CanExit = False
                GiveErrorExplanation(5)
            Else
                DynaLog.LogMessage("A scratch directory was specified.")
                DynaLog.LogMessage("Provided scratch directory: " & Quote & TextBox3.Text & Quote)
                DynaLog.LogMessage("Checking if provided scratch directory exists...")
                If Directory.Exists(TextBox3.Text) Then
                    DynaLog.LogMessage("The scratch directory exists.")
                    CanExit = True
                Else
                    DynaLog.LogMessage("The scratch directory does not exist. Attempting to create it...")
                    Try
                        Directory.CreateDirectory(TextBox3.Text)
                        CanExit = True
                    Catch ex As Exception
                        DynaLog.LogMessage("Could not create scratch directory. Error message: " & ex.Message)
                        CanExit = False
                        GiveErrorExplanation(6)
                    End Try
                End If
            End If
        End If
        If Not CanExit Then
            DynaLog.LogMessage("We cannot continue until all options are correctly set.")
            Exit Sub
        End If
    End Sub

    Private Sub ApplyProgSettings()
        DynaLog.LogMessage("Beginning application of user settings...")
        DynaLog.LogMessage("Determining whether or not settings are valid...")
        DetermineSettingValidity()
        DynaLog.LogMessage("Continuing setting application...")
        MainForm.DismExe = TextBox1.Text
        Select Case ComboBox1.SelectedIndex
            Case 0
                MainForm.SaveOnSettingsIni = True
            Case 1
                MainForm.SaveOnSettingsIni = False
        End Select
        MainForm.ColorMode = ComboBox2.SelectedIndex
        MainForm.LanguageCode = GetSelectedLanguageCode(ComboBox3, MainForm.LanguageCode)
        MainForm.LogFont = ComboBox4.Text
        MainForm.LogFontSize = NumericUpDown1.Value
        If Toggle1.Checked Then
            MainForm.LogFontIsBold = True
        Else
            MainForm.LogFontIsBold = False
        End If
        If RadioButton5.Checked Then
            MainForm.ProgressPanelStyle = 1
        Else
            MainForm.ProgressPanelStyle = 0
        End If
        MainForm.LogFile = TextBox2.Text
        MainForm.LogLevel = TrackBar1.Value + 1
        If CheckBox2.Checked Then
            MainForm.QuietOperations = True
        Else
            MainForm.QuietOperations = False
        End If
        If CheckBox3.Checked Then
            MainForm.SysNoRestart = True
        Else
            MainForm.SysNoRestart = False
        End If
        MainForm.NoNTSamMappings = Not CheckBox23.Checked
        If CheckBox4.Checked Then
            MainForm.UseScratch = True
        Else
            MainForm.UseScratch = False
        End If
        If RadioButton3.Checked Then
            MainForm.AutoScrDir = True
        Else
            MainForm.AutoScrDir = False
        End If
        MainForm.ScratchDir = TextBox3.Text
        If CheckBox5.Checked Then
            MainForm.EnglishOutput = True
        Else
            MainForm.EnglishOutput = False
        End If
        Dim ti As TextInfo = New CultureInfo("en-US", False).TextInfo
        MainForm.AllCaps = CheckBox9.Checked
        If CheckBox9.Checked Then
            MainForm.FileToolStripMenuItem.Text = MainForm.FileToolStripMenuItem.Text.ToUpper()
            MainForm.ProjectToolStripMenuItem.Text = MainForm.ProjectToolStripMenuItem.Text.ToUpper()
            MainForm.CommandsToolStripMenuItem.Text = MainForm.CommandsToolStripMenuItem.Text.ToUpper()
            MainForm.ToolsToolStripMenuItem.Text = MainForm.ToolsToolStripMenuItem.Text.ToUpper()
            MainForm.HelpToolStripMenuItem.Text = MainForm.HelpToolStripMenuItem.Text.ToUpper()
        Else
            MainForm.FileToolStripMenuItem.Text = ti.ToTitleCase(MainForm.FileToolStripMenuItem.Text.ToLower())
            MainForm.ProjectToolStripMenuItem.Text = ti.ToTitleCase(MainForm.ProjectToolStripMenuItem.Text.ToLower())
            MainForm.CommandsToolStripMenuItem.Text = ti.ToTitleCase(MainForm.CommandsToolStripMenuItem.Text.ToLower())
            MainForm.ToolsToolStripMenuItem.Text = ti.ToTitleCase(MainForm.ToolsToolStripMenuItem.Text.ToLower())
            MainForm.HelpToolStripMenuItem.Text = ti.ToTitleCase(MainForm.HelpToolStripMenuItem.Text.ToLower())
        End If
        MainForm.ReportView = ComboBox5.SelectedIndex
        MainForm.DarkThemeIndex = DarkThemesCB.SelectedIndex
        MainForm.LightThemeIndex = LightThemesCB.SelectedIndex
        MainForm.ChangePrgColors(MainForm.ColorMode)
        MainForm.ApplyLanguage(MainForm.LanguageCode)
        If MountedImgMgr.Visible Then
            MountedImgMgr.Close()
            MountedImgMgr.Show()
        End If
        If BGProcDetails.Visible Then
            BGProcDetails.Visible = False
            BGProcDetails.Visible = True
        End If
        If MainForm.isProjectLoaded Then
            MainForm.UnpopulateProjectTree()
            MainForm.PopulateProjectTree(MainForm.prjName)
        End If
        If CheckBox6.Checked Then
            MainForm.NotificationShow = True
        Else
            MainForm.NotificationShow = False
        End If
        MainForm.NotificationFrequency = ComboBox6.SelectedIndex
        MainForm.StartupRemount = CheckBox12.Checked
        MainForm.StartupUpdateCheck = CheckBox13.Checked
        MainForm.AutoCleanMounts = CheckBox22.Checked
        MainForm.AutoLogs = CheckBox10.Checked
        MainForm.SystemEditor = TextBox5.Text
        If MainForm.IsImageMounted Then MainForm.DetectVersions(FileVersionInfo.GetVersionInfo(MainForm.DismExe), MainForm.CurrentImage.ImageVersion)
        MainForm.SkipQuestions = CheckBox14.Checked
        MainForm.AutoCompleteInfo(0) = CheckBox15.Checked
        MainForm.AutoCompleteInfo(1) = CheckBox16.Checked
        MainForm.AutoCompleteInfo(2) = CheckBox17.Checked
        MainForm.AutoCompleteInfo(3) = CheckBox18.Checked
        MainForm.AutoCompleteInfo(4) = CheckBox19.Checked
        MainForm.StatusStrip.BackColor = CurrentTheme.AccentColors(1)
        MainForm.ExpandedProgressPanel = CheckBox7.Checked
        MainForm.ShowDateAndTime = CheckBox21.Checked
        MainForm.TimeLabel.Visible = CheckBox21.Checked

        MainForm.SearchEngineName = ComboBox7.SelectedItem
        If SearchEngineHelper.GetAllSearchEngines().ElementAt(ComboBox7.SelectedIndex).AIPermission > ComboBox9.SelectedIndex Then
            MainForm.SearchEngineAITolerance = SearchEngineHelper.GetAllSearchEngines().ElementAt(ComboBox7.SelectedIndex).AIPermission
        End If

        MainForm.AppxDisplayNameFormatOnRemoval = ComboBox8.SelectedIndex
        MainForm.PreventSystemFromSleeping = CheckBox8.Checked
        MainForm.HumanizeDates = CheckBox1.Checked
        MainForm.LockUnlockedVolumes = CheckBox25.Checked

        MainForm.PEHelper_MaxConcurrentISO = NumericUpDown2.Value
    End Sub

    Private Sub GiveErrorExplanation(ErrorCode As Integer)
        DynaLog.LogMessage("Error Code: " & ErrorCode)
        Select Case ErrorCode
            Case 1
                MsgBox(LocalizationService.ForSection("Options.Messages")("Dismexecutable.Path.Message"), MsgBoxStyle.Critical, "DISMTools")
            Case 2
                MsgBox(LocalizationService.ForSection("Options.Messages")("DISM.Executable.Message"), MsgBoxStyle.Critical, "DISMTools")
            Case 3
                MsgBox(LocalizationService.ForSection("Options.Messages")("Log.File.Label"), MsgBoxStyle.Critical, "DISMTools")
            Case 4
                MsgBox(LocalizationService.ForSection("Options.Messages")("Tried.Create.Message"), MsgBoxStyle.Critical, "DISMTools")
            Case 5
                MsgBox(LocalizationService.ForSection("Options.Messages")("ScratchDir.Message"), MsgBoxStyle.Critical, "DISMTools")
            Case 6
                MsgBox(LocalizationService.ForSection("Options.Messages")("Tried.Scratch.Message"), MsgBoxStyle.Critical, "DISMTools")
        End Select
    End Sub

    Private Function DetectFileAssociations(RootClass As String) As Boolean
        DynaLog.LogMessage("Detecting file associations...")
        Try
            DynaLog.LogMessage("Getting values from root class " & Quote & RootClass & Quote & "...")
            Dim AssocCmd As String = FileAssociationHelper.GetFileAssociationCmdline(RootClass)
            DynaLog.LogMessage("Command-line of association: " & Quote & AssocCmd & Quote)

            ' Separate each part of the command-line to get the application path
            Dim CmdlineParts As String() = AssocCmd.Replace(Quote, "").Split(" ")
            Dim AssocCmdPath As String = ""
            For i = 0 To CmdlineParts.Length - 1
                AssocCmdPath &= " " & CmdlineParts(i)
                If File.Exists(AssocCmdPath) Then Exit For
            Next
            AssocCmd = AssocCmdPath
            Return File.Exists(AssocCmd)
        Catch ex As Exception
            DynaLog.LogMessage("Could not detect file associations. Error message: " & ex.Message)
            Return False
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Manages the file associations for files with the ".dtproj" extension
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ManageAssociations()
        Dim DtProjUseCustomIcon As Boolean = CheckBox11.Checked,
            DtssUseCustomIcon As Boolean = CheckBox24.Checked

        DynaLog.LogMessage("Setting file associations...")
        DynaLog.LogMessage("- Use a custom icon (DTPROJ)? " & If(DtProjUseCustomIcon, "Yes", "No"))
        DynaLog.LogMessage("- Use a custom icon (DTSS)? " & If(DtssUseCustomIcon, "Yes", "No"))

        If DTProjAssocCB.Checked Then
            FileAssociationHelper.SetFileAssociation(".dtproj", "DISMTools.Project", String.Format("{0}{1}{0} {0}/load={0}%1{0}{0}", Quote, Path.Combine(Application.StartupPath, "DISMTools.exe")),
                                                     "DISMTools Project", If(DtProjUseCustomIcon, Path.Combine(Application.StartupPath, "resources", "dtproj.ico"), ""), Not DtProjUseCustomIcon)
        Else
            FileAssociationHelper.RemoveFileAssociation(".dtproj", "DISMTools.Project")
        End If
        If DTSSEditAssocCB.Checked Then
            FileAssociationHelper.SetFileAssociation(".dtss", "DTSSEdit.StarterScript", String.Format("{0}{1}{0} /dtss={0}%1{0}", Quote, Path.Combine(Application.StartupPath, "tools", "StarterScriptEditor", "StarterScript.exe")),
                                                     "DISMTools Starter Script", If(DtssUseCustomIcon, Path.Combine(Application.StartupPath, "tools", "StarterScriptEditor", "DTSSIcon.ico"), ""), Not DtssUseCustomIcon)
        Else
            FileAssociationHelper.RemoveFileAssociation(".dtss", "DTSSEdit.StarterScript")
        End If

        DynaLog.LogMessage("Checking file associations one more time...")

        DTProjAssocCB.Checked = DetectFileAssociations("DISMTools.Project")
        CheckBox11.Checked = FileAssociationHelper.GetFileAssociationIconPath("DISMTools.Project") <> ""
        DTSSEditAssocCB.Checked = DetectFileAssociations("DTSSEdit.StarterScript")
        CheckBox24.Checked = FileAssociationHelper.GetFileAssociationIconPath("DTSSEdit.StarterScript") <> ""
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DynaLog.LogMessage("Applying program settings...")
        ApplyProgSettings()
        If CanExit Then
            DynaLog.LogMessage("Saving program settings...")
            MainForm.SaveDTSettings()
            DynaLog.LogMessage("We can close the Options dialog.")
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        MainForm.LanguageCode = LocalizationService.NormalizeCultureCode(originalLanguage)
        MainForm.ApplyLanguage(MainForm.LanguageCode)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub GetAIRServiceInformation()
        AutoReloadService = WindowsServiceHelper.GetOnlineSystemServiceInformationByName("DT_AutoReload")

        Label80.Text = If(AutoReloadService IsNot Nothing, LocalizationService.ForSection("Options.AIRServiceInfo")("Yes.Button"), LocalizationService.ForSection("Options.AIRServiceInfo")("No.Button"))
        Button7.Enabled = AutoReloadService Is Nothing
        Button13.Enabled = AutoReloadService IsNot Nothing

        If AutoReloadService IsNot Nothing Then
            Label82.Text = AutoReloadService.ImagePath

            ' Let's do some more checks, even though a service can be in a system, its corresponding binary may not be in there.
            If Not String.IsNullOrEmpty(AutoReloadService.ImagePath) AndAlso File.Exists(AutoReloadService.ImagePath) Then
                Button11.Enabled = AutoReloadService.StartType = WindowsService.ServiceStartType.Disabled
                Button12.Enabled = AutoReloadService.StartType < WindowsService.ServiceStartType.Disabled

                Try
                    Dim airSvcFvi As FileVersionInfo = FileVersionInfo.GetVersionInfo(AutoReloadService.ImagePath)
                    Label82.Text &= String.Format(" (version {0})", airSvcFvi.ProductVersion)
                Catch ex As Exception
                    ' don't grab the version then
                End Try
            Else
                Label80.Text = LocalizationService.ForSection("Options.AIRServiceInfo")("No.Button")
                Label82.Text = ""
                Button11.Enabled = False
                Button12.Enabled = False
                Button7.Enabled = True
                Button13.Enabled = False
            End If
        Else
            Label80.Text = LocalizationService.ForSection("Options.AIRServiceInfo")("No.Button")
            Label82.Text = ""
            Button11.Enabled = False
            Button12.Enabled = False
        End If

    End Sub


    Private Sub RestoreComboBoxIndex(comboBox As ComboBox, selectedIndex As Integer)
        If comboBox.Items.Count = 0 Then Return
        If selectedIndex < 0 Then Return
        comboBox.SelectedIndex = Math.Min(selectedIndex, comboBox.Items.Count - 1)
    End Sub

    Private Function GetSelectedLanguageCode(comboBox As ComboBox, fallbackCultureCode As String) As String
        If comboBox.SelectedItem IsNot Nothing AndAlso TypeOf comboBox.SelectedItem Is LocalizationLanguageInfo Then
            Return DirectCast(comboBox.SelectedItem, LocalizationLanguageInfo).Code
        End If

        If comboBox.SelectedValue IsNot Nothing Then
            Return comboBox.SelectedValue.ToString()
        End If

        Return LocalizationService.NormalizeCultureCode(fallbackCultureCode)
    End Function

    Private Sub PopulateLanguageComboBox(comboBox As ComboBox, selectedCultureCode As String)
        Dim normalizedCultureCode As String = LocalizationService.NormalizeCultureCode(selectedCultureCode)
        comboBox.Items.Clear()

        For Each languageInfo As LocalizationLanguageInfo In LocalizationService.GetAvailableLanguages()
            comboBox.Items.Add(languageInfo)
        Next

        Dim selectedIndex As Integer = -1
        For index As Integer = 0 To comboBox.Items.Count - 1
            Dim languageInfo As LocalizationLanguageInfo = TryCast(comboBox.Items(index), LocalizationLanguageInfo)
            If languageInfo IsNot Nothing AndAlso languageInfo.Code.Equals(normalizedCultureCode, StringComparison.OrdinalIgnoreCase) Then
                selectedIndex = index
                Exit For
            End If
        Next

        If selectedIndex < 0 Then
            For index As Integer = 0 To comboBox.Items.Count - 1
                Dim languageInfo As LocalizationLanguageInfo = TryCast(comboBox.Items(index), LocalizationLanguageInfo)
                If languageInfo IsNot Nothing AndAlso languageInfo.Code.Equals(LocalizationService.DefaultCultureCode, StringComparison.OrdinalIgnoreCase) Then
                    selectedIndex = index
                    Exit For
                End If
            Next
        End If

        If selectedIndex >= 0 Then comboBox.SelectedIndex = selectedIndex
    End Sub

    Private Sub ApplyLocalizedText()
        Dim selectedSaveLocation As Integer = ComboBox1.SelectedIndex
        Dim selectedColorMode As Integer = ComboBox2.SelectedIndex
        Dim selectedLanguageCode As String = GetSelectedLanguageCode(ComboBox3, MainForm.LanguageCode)
        Dim selectedLogView As Integer = ComboBox5.SelectedIndex
        Dim selectedNotificationFrequency As Integer = ComboBox6.SelectedIndex
        Dim selectedSearchEngine As Object = ComboBox7.SelectedItem

        isApplyingLocalizedText = True
        Try
        DynaLog.LogMessage("Resetting values to add translated resources...")
        ComboBox1.Items.Clear()
        ComboBox2.Items.Clear()
        ComboBox3.Items.Clear()
        ComboBox5.Items.Clear()
        ComboBox6.Items.Clear()
        ComboBox7.Items.Clear()
        ComboBox1.SelectedText = ""
        ComboBox2.SelectedText = ""
        ComboBox3.SelectedText = ""
        ComboBox5.SelectedText = ""
        ComboBox6.SelectedText = ""
        Text = LocalizationService.ForSection("Options")("Title.Label")
        ImageTaskHeader1.ItemText = Text
        Label49.Text = LocalizationService.ForSection("Options")("Program.Label")
        Label50.Text = LocalizationService.ForSection("Options")("Personalization.Label")
        Label51.Text = LocalizationService.ForSection("Options")("Logs.Label")
        Label52.Text = LocalizationService.ForSection("Options")("ImageOperations.Label")
        Label53.Text = LocalizationService.ForSection("Options")("ScratchDirectory.Label")
        Label54.Text = LocalizationService.ForSection("Options")("ProgramOutput.Label")
        Label55.Text = LocalizationService.ForSection("Options")("BgProcesses.Label")
        Label57.Text = LocalizationService.ForSection("Options")("FileAssociations.Label")
        Label58.Text = LocalizationService.ForSection("Options")("StartupOptions.Label")
        Label34.Text = LocalizationService.ForSection("Options")("ShutdownOptions.Label")
        Label2.Text = LocalizationService.ForSection("Options")("Dismexecutable.Path.Label")
        Label3.Text = LocalizationService.ForSection("Options")("Version.Label")
        Label5.Text = LocalizationService.ForSection("Options")("SaveSettings.Label")
        Label7.Text = LocalizationService.ForSection("Options")("ColorMode.Label")
        Label8.Text = LocalizationService.ForSection("Options")("Language.Label")
        Label9.Text = LocalizationService.ForSection("Options")("Settings.Log.Required.Label")
        Label10.Text = LocalizationService.ForSection("Options")("Log.Window.Font.Label")
        Label11.Text = LocalizationService.ForSection("Options")("Preview.Label")
        Label12.Text = LocalizationService.ForSection("Options")("Operation.Log.File.Label")
        Label13.Text = LocalizationService.ForSection("Options")("Image.Ops.Message")
        Label14.Text = LocalizationService.ForSection("Options")("Log.File.Level.Label")
        Label18.Text = LocalizationService.ForSection("Options")("QuietOperations.Message")
        Label19.Text = LocalizationService.ForSection("Options")("Checked.Computer.Message")
        Label20.Text = LocalizationService.ForSection("Options")("Scratch.Dir.Required.Label")
        Label21.Text = LocalizationService.ForSection("Options")("ScratchDirectory.Input.Label")
        Label22.Text = LocalizationService.ForSection("Options")("Space.Left.Selected.Label")
        Label25.Text = LocalizationService.ForSection("Options")("LogView.Label")
        Label26.Text = LocalizationService.ForSection("Options")("ExampleReport.Label")
        Label27.Text = LocalizationService.ForSection("Options")("Reports.Allow.Shown.Label")
        Label28.Text = LocalizationService.ForSection("Options")("Notify.Label")
        Label29.Text = LocalizationService.ForSection("Options")("Uses.Bg.Procs.Message")
        Label40.Text = LocalizationService.ForSection("Options")("Manage.File.Assoc.Label")
        Label43.Text = LocalizationService.ForSection("Options")("Behavior.OnStartup.Label")
        Label44.Text = LocalizationService.ForSection("Options")("Scratch.Dir.Message")
        Label45.Text = LocalizationService.ForSection("Options")("Secondary.Progress.Label")
        Label46.Text = LocalizationService.ForSection("Options")("Settings.Aren.Label")
        Label47.Text = LocalizationService.ForSection("Options")("Font.Readable.Log.Message")
        Label48.Text = LocalizationService.ForSection("Options")("SettingsConsider.Label")
        Button1.Text = LocalizationService.ForSection("Options")("Browse.Button")
        Button2.Text = LocalizationService.ForSection("Options")("View.DISM.Button")
        Button3.Text = LocalizationService.ForSection("Options")("Browse.Button")
        Button4.Text = LocalizationService.ForSection("Options")("Browse.Button")
        Button9.Text = LocalizationService.ForSection("Options")("Set.File.Assoc.Button")
        Button10.Text = LocalizationService.ForSection("Options")("AdvancedSettings.Button")
        Cancel_Button.Text = LocalizationService.ForSection("Options")("Cancel.Button")
        OK_Button.Text = LocalizationService.ForSection("Options")("Ok.Button")
        PrefReset.Text = LocalizationService.ForSection("Options")("ResetPreferences.Label")
        CheckBox2.Text = LocalizationService.ForSection("Options")("Quietly.Image.Ops.CheckBox")
        CheckBox3.Text = LocalizationService.ForSection("Options")("Skip.System.Restart.CheckBox")
        CheckBox4.Text = LocalizationService.ForSection("Options")("Scratch.Dir.CheckBox")
        CheckBox5.Text = LocalizationService.ForSection("Options")("Show.Command.Output.CheckBox")
        CheckBox6.Text = LocalizationService.ForSection("Options")("Notify.Me.CheckBox")
        CheckBox7.Text = LocalizationService.ForSection("Options")("Show.Log.View.CheckBox")
        CheckBox9.Text = LocalizationService.ForSection("Options")("Uppercase.Menus.CheckBox")
        CheckBox10.Text = LocalizationService.ForSection("Options")("Auto.Create.Logs.CheckBox")
        CheckBox11.Text = LocalizationService.ForSection("Options")("Set.Custom.File.CheckBox")
        CheckBox12.Text = LocalizationService.ForSection("Options")("Remount.Mounted.CheckBox")
        CheckBox13.Text = LocalizationService.ForSection("Options")("CheckUpdates.CheckBox")
        CheckBox14.Text = LocalizationService.ForSection("Options")("Always.Save.CheckBox")
        CheckBox15.Text = LocalizationService.ForSection("Options")("Installed.Packages.CheckBox")
        CheckBox16.Text = LocalizationService.ForSection("Options")("Features.CheckBox")
        CheckBox17.Text = LocalizationService.ForSection("Options")("Installed.AppX.CheckBox")
        CheckBox18.Text = LocalizationService.ForSection("Options")("Capabilities.CheckBox")
        CheckBox19.Text = LocalizationService.ForSection("Options")("InstalledDrivers.CheckBox")
        CheckBox22.Text = LocalizationService.ForSection("Options")("Automatically.Clean.CheckBox")
        DismOFD.Title = LocalizationService.ForSection("Options")("Dismexecutable.Title")
        Label59.Text = LocalizationService.ForSection("Options")("LogCustomization.Label")
        Label60.Text = LocalizationService.ForSection("Options")("Behavior.OnClose.Label")
        Label61.Text = LocalizationService.ForSection("Options")("Preview.Label")
        Label9.Text = LocalizationService.ForSection("Options")("Saving.Image.Item")
        LinkLabel1.Text = LocalizationService.ForSection("Options")("Enable.Disable.Message")
        LinkLabel1.LinkArea = LocalizationService.GetLinkArea(LinkLabel1.Text, LocalizationService.ForSection("Options")("Going.Affect.My"))
        LinkLabel2.Text = LocalizationService.ForSection("Options")("Learn.Background.Link")
        LogSFD.Title = LocalizationService.ForSection("Options")("Location.Log.File.Title")
        RadioButton3.Text = LocalizationService.ForSection("Options")("Project.Scratch.RadioButton")
        RadioButton4.Text = LocalizationService.ForSection("Options")("Custom.Scratch.RadioButton")
        RadioButton5.Text = LocalizationService.ForSection("Options")("Modern.RadioButton")
        RadioButton6.Text = LocalizationService.ForSection("Options")("Classic.RadioButton")
        ScratchFBD.Description = LocalizationService.ForSection("Options")("ScratchDir.Description")
        Label62.Text = LocalizationService.ForSection("Options")("Dyna.Log.Logging.Message") & LocalizationService.ForSection("Options")("Disable.Logging.Only.Message")
        Label63.Text = LocalizationService.ForSection("Options")("Default.Op.Logs.Message")
        Label64.Text = LocalizationService.ForSection("Options")("Dyna.Log.Logging.Label")
        Label65.Text = LocalizationService.ForSection("Options")("Editor.Open.Log.Label")
        Label66.Text = LocalizationService.ForSection("Options")("SystemEditor.Label")
        Button5.Text = LocalizationService.ForSection("Options")("Browse.Button")
        EditorOFD.Title = LocalizationService.ForSection("Options")("Editor.Title")
        LinkLabel3.Text = LocalizationService.ForSection("Options")("Show.Me.Logs.Link")
        LogPreview.Text = LocalizationService.ForSection("Options.LogPreview")("Packages.Add.Message")
        ApplySecondaryProgressPreview()
        CheckBox20.Text = LocalizationService.ForSection("Options")("Disable.Dyna.Log.CheckBox")

        Dim DesignerOptions = LocalizationService.ForSection("Designer.Options")
        DismOFD.Filter = DesignerOptions("DISM.Executable.Filter")
        LogSFD.Filter = DesignerOptions("LogSFD.Filter")
        EditorOFD.Filter = DesignerOptions("ProgramsEXE.Filter")
        DTSSEditAssocCB.Text = DesignerOptions("Open.Starter.Scripts.Label")
        DTProjAssocCB.Text = DesignerOptions("Open.My.Projects.Label")
        LinkLabel4.Text = DesignerOptions("Difference.Between.Link")
        Label72.Text = DesignerOptions("PackageName.Label")
        Label73.Text = DesignerOptions("RaymanJungle.Label")
        Label74.Text = DesignerOptions("DisplayName.Label")
        Label71.Text = DesignerOptions("Example.Label")
        Label70.Text = DesignerOptions("Remove.AppX.Label")
        Label32.Text = DesignerOptions("Only.Available.Message")
        CheckBox23.Text = DesignerOptions("Map.System.Accounts.CheckBox")
        CheckBox1.Text = DesignerOptions("Show.Dates.Human.CheckBox")
        CheckBox8.Text = DesignerOptions("PreventSleep.CheckBox")
        LinkLabel5.Text = DesignerOptions("Help.Me.Understand.Link")
        Label76.Text = DesignerOptions("AIFeature.Label")
        Label69.Text = DesignerOptions("Search.Engine.Web.Label")
        Label67.Text = DesignerOptions("Searching.Image.Online.Label")
        Label68.Text = DesignerOptions("Learn.Message")
        Button14.Text = DesignerOptions("RunNow.Button")
        Button7.Text = DesignerOptions("InstallService.Button")
        Button11.Text = DesignerOptions("EnableService.Button")
        Button12.Text = DesignerOptions("DisableService.Button")
        Button13.Text = DesignerOptions("DeleteService.Button")
        GroupBox2.Text = DesignerOptions("ServiceStatus.Group")
        Label79.Text = DesignerOptions("Installed.Label")
        Label81.Text = DesignerOptions("InstallationPath.Label")
        Label77.Text = DesignerOptions("Automatic.Image.Reload.Label")
        Label83.Text = DesignerOptions("Still.See.Standard.Message")
        Label78.Text = DesignerOptions("Automatic.Image.Message")
        GroupBox1.Text = DesignerOptions("ColorThemes.Group")
        Button6.Text = DesignerOptions("DesignThemes.Button")
        Label30.Text = DesignerOptions("LightMode.Label")
        Label33.Text = DesignerOptions("Own.Themes.Label")
        Label31.Text = DesignerOptions("Change.Color.Theme.Label")
        Label17.Text = DesignerOptions("DarkMode.Label")
        CheckBox21.Text = DesignerOptions("Show.Date.Time.CheckBox")
        CheckBox24.Text = DesignerOptions("Set.Custom.CheckBox")

        SaveLocations(0) = LocalizationService.ForSection("Options")("SettingsFile.Item")
        SaveLocations(1) = LocalizationService.ForSection("Options")("Registry.Item")
        ColorModes(0) = LocalizationService.ForSection("Options")("System.Setting.Item")
        ColorModes(1) = LocalizationService.ForSection("Options")("LightMode.Item")
        ColorModes(2) = LocalizationService.ForSection("Options")("DarkMode.Item")
        LogViews(0) = LocalizationService.ForSection("Options")("List.Item")
        LogViews(1) = LocalizationService.ForSection("Options")("Table.Item")
        NotFreqs(0) = LocalizationService.ForSection("Options")("Every.Time.Project.Item")
        NotFreqs(1) = LocalizationService.ForSection("Options")("Freqs1.Item")
        ComboBox1.Items.AddRange(SaveLocations)
        ComboBox2.Items.AddRange(ColorModes)
        PopulateLanguageComboBox(ComboBox3, selectedLanguageCode)
        ComboBox5.Items.AddRange(LogViews)
        ComboBox6.Items.AddRange(NotFreqs)
        ComboBox7.Items.AddRange(SearchEngineHelper.GetAllSearchEngines().Select(Function(engine) engine.Name).ToArray())
        DynaLog.LogMessage("Checking if portable marker exists...")
        If File.Exists(Application.StartupPath & "\portable") Then ComboBox1.Items.RemoveAt(1)
            RestoreComboBoxIndex(ComboBox1, selectedSaveLocation)
            RestoreComboBoxIndex(ComboBox2, selectedColorMode)
            PopulateLanguageComboBox(ComboBox3, selectedLanguageCode)
            RestoreComboBoxIndex(ComboBox5, selectedLogView)
            RestoreComboBoxIndex(ComboBox6, selectedNotificationFrequency)
            If selectedSearchEngine IsNot Nothing AndAlso ComboBox7.Items.Contains(selectedSearchEngine) Then
                ComboBox7.SelectedItem = selectedSearchEngine
            End If
        Finally
            isApplyingLocalizedText = False
        End Try
    End Sub

    Private Sub Options_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        originalLanguage = LocalizationService.NormalizeCultureCode(MainForm.LanguageCode)
        ApplyLocalizedText()

        DynaLog.LogMessage("Getting system fonts...")
        GetSystemFonts()
        ' Set default values before loading custom ones
        TextBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
        DismVersion = FileVersionInfo.GetVersionInfo(TextBox1.Text)
        Label4.Text = DismVersion.ProductVersion
        TextBox2.Text = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\Windows\Logs\DISM\DISM.log"
        DynaLog.LogMessage("Gathering custom settings...")
        GatherCustomSettings()

        ' Set program colors
        ImageTaskHeader1.SetColors()
        BackColor = CurrentTheme.SectionBackgroundColor
        ForeColor = CurrentTheme.ForegroundColor
        TextBox1.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox1.ForeColor = CurrentTheme.ForegroundColor
        TextBox2.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox2.ForeColor = CurrentTheme.ForegroundColor
        TextBox3.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox3.ForeColor = CurrentTheme.ForegroundColor
        TextBox4.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox4.ForeColor = CurrentTheme.ForegroundColor
        TextBox5.BackColor = CurrentTheme.SectionBackgroundColor
        TextBox5.ForeColor = CurrentTheme.ForegroundColor
        LogPreview.BackColor = CurrentTheme.SectionBackgroundColor
        LogPreview.ForeColor = CurrentTheme.ForegroundColor
        ComboBox1.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox1.ForeColor = CurrentTheme.ForegroundColor
        ComboBox2.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox2.ForeColor = CurrentTheme.ForegroundColor
        ComboBox3.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox3.ForeColor = CurrentTheme.ForegroundColor
        ComboBox4.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox4.ForeColor = CurrentTheme.ForegroundColor
        ComboBox5.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox5.ForeColor = CurrentTheme.ForegroundColor
        ComboBox6.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox6.ForeColor = CurrentTheme.ForegroundColor
        ComboBox7.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox7.ForeColor = CurrentTheme.ForegroundColor
        ComboBox8.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox8.ForeColor = CurrentTheme.ForegroundColor
        ComboBox9.BackColor = CurrentTheme.SectionBackgroundColor
        ComboBox9.ForeColor = CurrentTheme.ForegroundColor
        DarkThemesCB.BackColor = CurrentTheme.SectionBackgroundColor
        DarkThemesCB.ForeColor = CurrentTheme.ForegroundColor
        LightThemesCB.BackColor = CurrentTheme.SectionBackgroundColor
        LightThemesCB.ForeColor = CurrentTheme.ForegroundColor
        NumericUpDown1.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown1.ForeColor = CurrentTheme.ForegroundColor
        NumericUpDown2.BackColor = CurrentTheme.SectionBackgroundColor
        NumericUpDown2.ForeColor = CurrentTheme.ForegroundColor
        GroupBox1.ForeColor = CurrentTheme.ForegroundColor
        GroupBox2.ForeColor = CurrentTheme.ForegroundColor
        TrackBar1.BackColor = CurrentTheme.SectionBackgroundColor
        PictureBox10.Image = GetGlyphResource("options_program")
        PictureBox11.Image = GetGlyphResource("options_personalization")
        PictureBox12.Image = GetGlyphResource("options_logs")
        PictureBox13.Image = GetGlyphResource("image_glyph")
        PictureBox14.Image = GetGlyphResource("options_scratch")
        PictureBox15.Image = GetGlyphResource("options_output")
        PictureBox16.Image = GetGlyphResource("options_bgprocs")
        DarkThemesCB.Items.Clear()
        LightThemesCB.Items.Clear()
        For Each LoadedTheme In ThemeHelper.GetThemes()
            DarkThemesCB.Items.Add(LoadedTheme.Name)
            LightThemesCB.Items.Add(LoadedTheme.Name)
        Next
        Try
            DarkThemesCB.SelectedIndex = MainForm.DarkThemeIndex
            LightThemesCB.SelectedIndex = MainForm.LightThemeIndex
        Catch ex As Exception

        End Try
        Dim handle As IntPtr = WindowHelper.GetWindowHandle(Me)
        WindowHelper.ToggleDarkTitleBar(handle, CurrentTheme.IsDark)
        ThemeHelper.UpdateLinkLabelColors(Me, Color.DodgerBlue, CurrentTheme.AccentColors(0))
        If Not File.Exists(Application.StartupPath & "\portable") Then
            Panel2.Enabled = False
            Panel3.Visible = True
        Else
            Panel2.Enabled = True
            Panel3.Visible = False
        End If
        ChangeSections(SectionNum)
        FlowLayoutPanel1.BackColor = ImageTaskHeader1.BackColor

        If SplitContainer1.SplitterDistance = 256 Then
            SplitContainer1.SplitterDistance = WindowHelper.ScaleLogical(SplitContainer1.SplitterDistance)
        End If

        DTProjAssocCB.Checked = DetectFileAssociations("DISMTools.Project")
        DTSSEditAssocCB.Checked = DetectFileAssociations("DTSSEdit.StarterScript")
        GetAIRServiceInformation()
        ImageTaskHeader1.HideWindowTitle(handle)
    End Sub

    Sub GetSystemFonts()
        ComboBox4.Items.Clear()
        For Each fntFamily As FontFamily In FontFamily.Families
            ComboBox4.Items.Add(fntFamily.Name)
        Next
        DynaLog.LogMessage(ComboBox4.Items.Count & " font(s) have been detected on this system.")
        If ComboBox4.SelectedItem = Nothing Then ComboBox4.SelectedItem = "Consolas"
    End Sub

    Sub GatherCustomSettings()
        DynaLog.LogMessage("Getting custom settings...")
        TextBox1.Text = MainForm.DismExe
        DismVersion = FileVersionInfo.GetVersionInfo(TextBox1.Text)
        Label4.Text = DismVersion.ProductVersion
        If MainForm.SaveOnSettingsIni Then
            ComboBox1.SelectedIndex = 0
        Else
            ComboBox1.SelectedIndex = 1
        End If
        Select Case MainForm.ColorMode
            Case 0
                ComboBox2.SelectedIndex = 0
            Case 1
                ComboBox2.SelectedIndex = 1
            Case 2
                ComboBox2.SelectedIndex = 2
        End Select
        PopulateLanguageComboBox(ComboBox3, MainForm.LanguageCode)
        ComboBox4.Text = MainForm.LogFont
        NumericUpDown1.Value = MainForm.LogFontSize
        If MainForm.LogFontIsBold Then
            Toggle1.Checked = True
        Else
            Toggle1.Checked = False
        End If
        Select Case MainForm.ProgressPanelStyle
            Case 0
                RadioButton5.Checked = False
                RadioButton6.Checked = True
            Case 1
                RadioButton5.Checked = True
                RadioButton6.Checked = False
        End Select
        TextBox2.Text = MainForm.LogFile
        TrackBar1.Value = If(MainForm.LogLevel = TrackBar1.Minimum, MainForm.LogLevel, MainForm.LogLevel - 1)
        If MainForm.QuietOperations Then
            CheckBox2.Checked = True
        Else
            CheckBox2.Checked = False
        End If
        If MainForm.SysNoRestart Then
            CheckBox3.Checked = True
        Else
            CheckBox3.Checked = False
        End If
        If MainForm.UseScratch Then
            CheckBox4.Checked = True
            TextBox3.Text = MainForm.ScratchDir
        Else
            CheckBox4.Checked = False
            TextBox3.Text = ""
        End If
        If MainForm.AutoScrDir Then
            RadioButton3.Checked = True
            RadioButton4.Checked = False
        Else
            RadioButton3.Checked = False
            RadioButton4.Checked = True
        End If
        If MainForm.EnglishOutput Then
            CheckBox5.Checked = True
        Else
            CheckBox5.Checked = False
        End If
        Select Case MainForm.ReportView
            Case 0
                ComboBox5.SelectedIndex = 0
            Case 1
                ComboBox5.SelectedIndex = 1
        End Select
        If MainForm.NotificationShow Then
            CheckBox6.Checked = True
        Else
            CheckBox6.Checked = False
        End If
        Select Case MainForm.NotificationFrequency
            Case 0
                ComboBox6.SelectedIndex = 0
            Case 1
                ComboBox6.SelectedIndex = 1
        End Select
        GetRootSpace(TextBox3.Text)
        CheckBox10.Checked = MainForm.AutoLogs
        CheckBox20.Checked = Not MainForm.EnableDynaLog
        TextBox5.Text = MainForm.SystemEditor
        CheckBox12.Checked = MainForm.StartupRemount
        CheckBox13.Checked = MainForm.StartupUpdateCheck
        CheckBox9.Checked = MainForm.AllCaps
        CheckBox14.Checked = MainForm.SkipQuestions
        CheckBox15.Checked = MainForm.AutoCompleteInfo(0)
        CheckBox16.Checked = MainForm.AutoCompleteInfo(1)
        CheckBox17.Checked = MainForm.AutoCompleteInfo(2)
        CheckBox18.Checked = MainForm.AutoCompleteInfo(3)
        CheckBox19.Checked = MainForm.AutoCompleteInfo(4)
        CheckBox22.Checked = MainForm.AutoCleanMounts
        CheckBox7.Checked = MainForm.ExpandedProgressPanel
        CheckBox21.Checked = MainForm.ShowDateAndTime
        CheckBox23.Checked = Not MainForm.NoNTSamMappings

        ComboBox9.SelectedIndex = MainForm.SearchEngineAITolerance      ' This goes first because we need to configure the tolerance; otherwise we get a dialog on launch
        ComboBox7.SelectedItem = MainForm.SearchEngineName

        ComboBox8.SelectedIndex = MainForm.AppxDisplayNameFormatOnRemoval
        CheckBox8.Checked = MainForm.PreventSystemFromSleeping
        CheckBox1.Checked = MainForm.HumanizeDates
        CheckBox25.Checked = MainForm.LockUnlockedVolumes

        NumericUpDown2.Value = MainForm.PEHelper_MaxConcurrentISO
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        If isInitializingForm OrElse isApplyingLocalizedText Then Return
        If ComboBox3.SelectedIndex < 0 Then Return

        Dim previousLanguageCode As String = MainForm.LanguageCode
        Dim selectedLanguageCode As String = GetSelectedLanguageCode(ComboBox3, previousLanguageCode)
        Dim validationMessage As String = ""
        If Not LocalizationService.ValidateLanguage(selectedLanguageCode, validationMessage) Then
            MessageBox.Show(validationMessage,
                            "Incompatible or invalid DISMTools language file",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            isApplyingLocalizedText = True
            Try
                PopulateLanguageComboBox(ComboBox3, previousLanguageCode)
            Finally
                isApplyingLocalizedText = False
            End Try
            Return
        End If

        MainForm.LanguageCode = selectedLanguageCode
        LocalizationService.SetLanguageByCultureCode(MainForm.LanguageCode)
        ApplyLocalizedText()
        MainForm.ApplyLanguage(MainForm.LanguageCode)
        ChangeSections(SectionNum)
        ImageTaskHeader1.ItemText = Text
    End Sub

    Private Sub ComboBox5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox5.SelectedIndexChanged
        Select Case ComboBox5.SelectedIndex
            Case 0
                TextBox4.Text = LocalizationService.ForSection("Options")("Image.Version.Message")
            Case 1
                TextBox4.Text = LocalizationService.ForSection("Options")("LogPreview.Message")
        End Select
    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged
        DynaLog.LogMessage("Changing bold font status. Toggle checked? " & If(Toggle1.Checked, "Yes", "No"))
        If Toggle1.Checked Then
            LogPreview.Font = New Font(ComboBox4.Text, NumericUpDown1.Value, FontStyle.Bold)
        Else
            LogPreview.Font = New Font(ComboBox4.Text, NumericUpDown1.Value, FontStyle.Regular)
        End If
        Panel4.Visible = Not IsMonospacedFont(ComboBox4.Text)
    End Sub

    Function IsMonospacedFont(ftName As String) As Boolean
        DynaLog.LogMessage("Detecting if font " & Quote & ftName & Quote & " is monospaced...")
        Using testFont As Font = New Font(ftName, 10)
            Dim widthI As Decimal = MeasureCharacterWidth(testFont, "i")
            Dim widthW As Decimal = MeasureCharacterWidth(testFont, "w")
            DynaLog.LogMessage("Width of character " & Quote & "i" & Quote & ": " & widthI)
            DynaLog.LogMessage("Width of character " & Quote & "W" & Quote & ": " & widthW)
            DynaLog.LogMessage("Are widths equal? " & If(widthI = widthW, "Yes", "No"))
            Return widthI = widthW
        End Using
        Return False
    End Function

    Function MeasureCharacterWidth(ft As Font, character As Char) As Decimal
        Using bmp As Bitmap = New Bitmap(1, 1)
            Using g As Graphics = Graphics.FromImage(bmp)
                Dim size As SizeF = g.MeasureString(character.ToString(), ft)
                Return size.Width
            End Using
        End Using
        Return 0
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DynaLog.LogMessage("Checking if a system DISM folder exists...")
        If Not Directory.Exists(Path.Combine(Path.GetDirectoryName(TextBox1.Text), "dism")) Then
            DynaLog.LogMessage("Said folder does not exist on the file system.")
            Dim msg As String = ""
            msg = LocalizationService.ForSection("Options.Actions")("DISM.Components.Message")
            MsgBox(msg, vbOKOnly + vbExclamation, ImageTaskHeader1.ItemText)
            Exit Sub
        End If
        DynaLog.LogMessage("Showing component information...")
        DismComponents.ShowDialog(Me)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        DismOFD.ShowDialog(Me)
    End Sub

    Private Sub DismOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles DismOFD.FileOk
        TextBox1.Text = DismOFD.FileName
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text = "" Or Not File.Exists(TextBox1.Text) Then
            TextBox1.Text = Environment.GetFolderPath(Environment.SpecialFolder.Windows) & "\system32\dism.exe"
        End If
        DynaLog.LogMessage("Detecting product version of " & Quote & TextBox1.Text & Quote)
        DismVersion = FileVersionInfo.GetVersionInfo(TextBox1.Text)
        DynaLog.LogMessage("Product version: " & DismVersion.ProductVersion)
        Label4.Text = DismVersion.ProductVersion
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        LogSFD.ShowDialog(Me)
    End Sub

    Private Sub LogSFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles LogSFD.FileOk
        DynaLog.LogMessage("Log file path: " & Quote & LogSFD.FileName & Quote)
        TextBox2.Text = LogSFD.FileName
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ScratchFBD.ShowDialog(Me)
        If DialogResult.OK And ScratchFBD.SelectedPath <> "" Then
            TextBox3.Text = ScratchFBD.SelectedPath
        End If
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        DynaLog.LogMessage("Log level (trackbar value + 1): " & (TrackBar1.Value + 1))
        Select Case TrackBar1.Value
            Case 0
                Label15.Text = LocalizationService.ForSection("Options.LogLevel")("Level1.Label")
                Label16.Text = LocalizationService.ForSection("Options.LogLevel")("Errors.Description.Label")
            Case 1
                Label15.Text = LocalizationService.ForSection("Options.LogLevel")("Level2.Item")
                Label16.Text = LocalizationService.ForSection("Options.LogLevel")("Level2.Description.Item")
            Case 2
                Label15.Text = LocalizationService.ForSection("Options.LogLevel")("Level2Messages.Item")
                Label16.Text = LocalizationService.ForSection("Options.LogLevel")("Level3.Description.Message")
            Case 3
                Label15.Text = LocalizationService.ForSection("Options.LogLevel")("Level2Debug.Item")
                Label16.Text = LocalizationService.ForSection("Options.LogLevel")("Level4.Description.Message")
        End Select
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        If RadioButton3.Checked Then
            Label20.Enabled = False
            Label21.Enabled = False
            TextBox3.Enabled = False
            Button4.Enabled = False
            Label22.Enabled = False
            Label23.Enabled = False
            Label24.Enabled = False
            PictureBox5.Enabled = False
            Label44.Enabled = True
        Else
            Label20.Enabled = True
            Label21.Enabled = True
            TextBox3.Enabled = True
            Button4.Enabled = True
            Label22.Enabled = True
            Label23.Enabled = True
            Label24.Enabled = True
            PictureBox5.Enabled = True
            Label44.Enabled = False
        End If
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        DynaLog.LogMessage("Getting space of drive containing folder " & Quote & TextBox3.Text & Quote & "...")
        GetRootSpace(TextBox3.Text)
    End Sub

    ''' <summary>
    ''' Gets the space of the drive which contains the scratch directory (referred to as the root drive)
    ''' </summary>
    ''' <param name="SourceDir">The source scratch directory</param>
    ''' <remarks></remarks>
    Sub GetRootSpace(SourceDir As String)
        If SourceDir = "" Then
            Label23.Text = LocalizationService.ForSection("Options.GetRootSpace")("Scratch.Dir.Required.Label")
            Label24.Visible = False
            PictureBox5.Visible = False
            PictureBox5.Image = New Bitmap(My.Resources.info_16px)
            Label24.Text = LocalizationService.ForSection("Options.GetRootSpace")("EnoughSpace.Label")
        Else
            Try
                Dim drInfo As New DriveInfo(Path.GetPathRoot(SourceDir))
                Dim FreeSpace As Double = drInfo.AvailableFreeSpace / (1024 ^ 3)
                Label23.Text = LocalizationService.ForSection("Options.GetRootSpace").Format("GB.Item", Math.Round(FreeSpace, 2))
                Select Case Math.Round(FreeSpace, 0)
                    Case Is < 5
                        Label24.Visible = True
                        PictureBox5.Visible = True
                        PictureBox5.Image = New Bitmap(My.Resources.error_16px)
                        Label24.Text = LocalizationService.ForSection("Options.GetRootSpace")("Enough.Message")
                    Case 5 To 19.989999999999998
                        Label24.Visible = True
                        PictureBox5.Visible = True
                        PictureBox5.Image = New Bitmap(My.Resources.warning_16px)
                        Label24.Text = LocalizationService.ForSection("Options.GetRootSpace")("EnoughSpace.SomeOps.Item")
                    Case Is >= 20
                        Label24.Visible = False
                        PictureBox5.Visible = False
                        PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                        Label24.Text = LocalizationService.ForSection("Options.GetRootSpace")("EnoughSpace.Directory.Item")
                End Select
            Catch ex As Exception
                Label23.Text = LocalizationService.ForSection("Options.GetRootSpace")("Free.Unavailable.Item")
                Label24.Visible = False
                PictureBox5.Visible = False
                PictureBox5.Image = New Bitmap(My.Resources.info_16px)
                Label24.Text = LocalizationService.ForSection("Options.GetRootSpace")("Have.Enough.Item")
                Exit Sub
            End Try
        End If
    End Sub

    Private Sub Toggle1_CheckedChanged(sender As Object, e As EventArgs) Handles Toggle1.CheckedChanged
        DynaLog.LogMessage("Changing bold font status. Toggle checked? " & If(Toggle1.Checked, "Yes", "No"))
        If Toggle1.Checked Then
            LogPreview.Font = New Font(ComboBox4.Text, NumericUpDown1.Value, FontStyle.Bold)
        Else
            LogPreview.Font = New Font(ComboBox4.Text, NumericUpDown1.Value, FontStyle.Regular)
        End If
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        DynaLog.LogMessage("Changing bold font status. Toggle checked? " & If(Toggle1.Checked, "Yes", "No"))
        If Toggle1.Checked Then
            LogPreview.Font = New Font(ComboBox4.Text, NumericUpDown1.Value, FontStyle.Bold)
        Else
            LogPreview.Font = New Font(ComboBox4.Text, NumericUpDown1.Value, FontStyle.Regular)
        End If
    End Sub

    Private Sub CheckBox10_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox10.CheckedChanged
        If CheckBox10.Checked Then
            Label12.Enabled = False
            TextBox2.Enabled = False
            Button3.Enabled = False
        Else
            Label12.Enabled = True
            TextBox2.Enabled = True
            Button3.Enabled = True
        End If
    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        Panel12.Enabled = CheckBox6.Checked
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        DynaLog.LogMessage("Toggling state of file associations...")
        ManageAssociations()
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        BGProcsAdvSettings.ShowDialog(Me)
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.Checked Then
            RadioButton3.Enabled = True
            RadioButton4.Enabled = True
            If RadioButton3.Checked Then
                Label20.Enabled = False
                Label21.Enabled = False
                TextBox3.Enabled = False
                Button4.Enabled = False
                Label22.Enabled = False
                Label23.Enabled = False
                Label24.Enabled = False
                PictureBox5.Enabled = False
                Label44.Enabled = True
            Else
                Label20.Enabled = True
                Label21.Enabled = True
                TextBox3.Enabled = True
                Button4.Enabled = True
                Label22.Enabled = True
                Label23.Enabled = True
                Label24.Enabled = True
                PictureBox5.Enabled = True
                Label44.Enabled = False
            End If
        Else
            RadioButton3.Enabled = False
            RadioButton4.Enabled = False
            Label20.Enabled = False
            Label21.Enabled = False
            TextBox3.Enabled = False
            Button4.Enabled = False
            Label22.Enabled = False
            Label23.Enabled = False
            Label24.Enabled = False
            PictureBox5.Enabled = False
            Label44.Enabled = False
        End If
    End Sub


    Private Sub ApplySecondaryProgressPreview()
        Dim previewText As String = LocalizationService.ForSection("Options.ProgressPreview")("ImageIndexes.Message")
        Dim waitText As String = LocalizationService.ForSection("Options.ProgressPreview")("Wait.Label")
        SecProgressStylePreview.Image = RenderSecondaryProgressPreview(RadioButton5.Checked, waitText, previewText)
    End Sub

    Private Function RenderSecondaryProgressPreview(modernStyle As Boolean, waitText As String, previewText As String) As Bitmap
        Dim image As Bitmap = New Bitmap(If(modernStyle, My.Resources.secprogress_modern, My.Resources.secprogress_classic))

        Using graphics As Graphics = Graphics.FromImage(image)
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit

            Using backgroundBrush As New SolidBrush(Color.FromArgb(32, 32, 32))
                If modernStyle Then
                    graphics.FillRectangle(backgroundBrush, 1, 1, image.Width - 2, image.Height - 2)
                Else
                    graphics.FillRectangle(backgroundBrush, 55, 1, image.Width - 56, image.Height - 2)
                End If
            End Using

            Using textBrush As New SolidBrush(Color.White)
                If modernStyle Then
                    Using previewFont As New Font("Segoe UI", 9.0F, FontStyle.Regular)
                        Using format As New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center}
                            graphics.DrawString(previewText, previewFont, textBrush, New RectangleF(0, 0, image.Width, image.Height), format)
                        End Using
                    End Using
                Else
                    Using waitFont As New Font("Segoe UI", 8.25F, FontStyle.Bold)
                        Using previewFont As New Font("Segoe UI", 8.25F, FontStyle.Regular)
                            graphics.DrawString(waitText, waitFont, textBrush, New PointF(56.0F, 13.0F))
                            graphics.DrawString(previewText, previewFont, textBrush, New PointF(56.0F, 29.0F))
                        End Using
                    End Using
                End If
            End Using
        End Using

        Return image
    End Function

    Private Sub RadioButton5_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton5.CheckedChanged
        ApplySecondaryProgressPreview()
    End Sub

    Private Sub PrefReset_Click(sender As Object, e As EventArgs) Handles PrefReset.Click
        DynaLog.LogMessage("Preparing to reset settings... It will be done if the user wants to do so")
        SettingsResetDlg.ShowDialog(Me)
        If SettingsResetDlg.DialogResult = Windows.Forms.DialogResult.OK Then
            DynaLog.LogMessage("Proceeding to reset settings - User accepted the question.")
            MainForm.ResetDTSettings()
            Cancel_Button.PerformClick()
        End If
    End Sub

    Private Sub CheckBox14_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox14.CheckedChanged
        TableLayoutPanel2.Enabled = CheckBox14.Checked
    End Sub

#Region "Section functionality"

    Sub ChangeSections(Number As Integer)
        DynaLog.LogMessage("Changing visible section in the settings. Section number index: " & Number)
        Options_Program.Visible = Number = 0
        Options_Personalization.Visible = Number = 1
        Options_Logs.Visible = Number = 2
        Options_ImgOps.Visible = Number = 3
        Options_Scratch.Visible = Number = 4
        Options_Output.Visible = Number = 5
        Options_BgProcs.Visible = Number = 6
        Options_FileAssocs.Visible = Number = 7
        Options_Startup.Visible = Number = 8
        Options_Shutdown.Visible = Number = 9
        Label49.Font = New Font("Segoe UI", 9, If(Number = 0, FontStyle.Bold, FontStyle.Regular))
        Label50.Font = New Font("Segoe UI", 9, If(Number = 1, FontStyle.Bold, FontStyle.Regular))
        Label51.Font = New Font("Segoe UI", 9, If(Number = 2, FontStyle.Bold, FontStyle.Regular))
        Label52.Font = New Font("Segoe UI", 9, If(Number = 3, FontStyle.Bold, FontStyle.Regular))
        Label53.Font = New Font("Segoe UI", 9, If(Number = 4, FontStyle.Bold, FontStyle.Regular))
        Label54.Font = New Font("Segoe UI", 9, If(Number = 5, FontStyle.Bold, FontStyle.Regular))
        Label55.Font = New Font("Segoe UI", 9, If(Number = 6, FontStyle.Bold, FontStyle.Regular))
        Label57.Font = New Font("Segoe UI", 9, If(Number = 7, FontStyle.Bold, FontStyle.Regular))
        Label58.Font = New Font("Segoe UI", 9, If(Number = 8, FontStyle.Bold, FontStyle.Regular))
        Label34.Font = New Font("Segoe UI", 9, If(Number = 9, FontStyle.Bold, FontStyle.Regular))
        ProgramSectionBtn.BackColor = If(Number = 0, BackColor, ImageTaskHeader1.BackColor)
        PersonalizationSectionBtn.BackColor = If(Number = 1, BackColor, ImageTaskHeader1.BackColor)
        LogSectionBtn.BackColor = If(Number = 2, BackColor, ImageTaskHeader1.BackColor)
        ImgOpsSectionBtn.BackColor = If(Number = 3, BackColor, ImageTaskHeader1.BackColor)
        ScDirSectionBtn.BackColor = If(Number = 4, BackColor, ImageTaskHeader1.BackColor)
        OutputSectionBtn.BackColor = If(Number = 5, BackColor, ImageTaskHeader1.BackColor)
        BgProcsSectionBtn.BackColor = If(Number = 6, BackColor, ImageTaskHeader1.BackColor)
        AssocsSectionBtn.BackColor = If(Number = 7, BackColor, ImageTaskHeader1.BackColor)
        StartupSectionBtn.BackColor = If(Number = 8, BackColor, ImageTaskHeader1.BackColor)
        ShutdownSectionBtn.BackColor = If(Number = 9, BackColor, ImageTaskHeader1.BackColor)
        SectionNum = Number
    End Sub

    Private Sub ProgramSectionBtn_Click(sender As Object, e As EventArgs) Handles ProgramSectionBtn.Click, Label49.Click, PictureBox10.Click
        ChangeSections(0)
    End Sub

    Private Sub PersonalizationSectionBtn_Click(sender As Object, e As EventArgs) Handles PersonalizationSectionBtn.Click, Label50.Click, PictureBox11.Click
        ChangeSections(1)
    End Sub

    Private Sub LogSectionBtn_Click(sender As Object, e As EventArgs) Handles LogSectionBtn.Click, Label51.Click, PictureBox12.Click
        ChangeSections(2)
    End Sub

    Private Sub ImgOpsSectionBtn_Click(sender As Object, e As EventArgs) Handles ImgOpsSectionBtn.Click, Label52.Click, PictureBox13.Click
        ChangeSections(3)
    End Sub

    Private Sub ScDirSectionBtn_Click(sender As Object, e As EventArgs) Handles ScDirSectionBtn.Click, Label53.Click, PictureBox14.Click
        ChangeSections(4)
    End Sub

    Private Sub OutputSectionBtn_Click(sender As Object, e As EventArgs) Handles OutputSectionBtn.Click, Label54.Click, PictureBox15.Click
        ChangeSections(5)
    End Sub

    Private Sub BgProcsSectionBtn_Click(sender As Object, e As EventArgs) Handles BgProcsSectionBtn.Click, Label55.Click, PictureBox16.Click
        ChangeSections(6)
    End Sub

    Private Sub AssocsSectionBtn_Click(sender As Object, e As EventArgs) Handles AssocsSectionBtn.Click, Label57.Click, PictureBox18.Click
        ChangeSections(7)
    End Sub

    Private Sub StartupSectionBtn_Click(sender As Object, e As EventArgs) Handles StartupSectionBtn.Click, Label58.Click, PictureBox19.Click
        ChangeSections(8)
    End Sub

    Private Sub ShutdownSectionBtn_Click(sender As Object, e As EventArgs) Handles ShutdownSectionBtn.Click, Label34.Click, PictureBox20.Click
        ChangeSections(9)
    End Sub

#End Region

    Private Sub CheckBox7_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox7.CheckedChanged
        DynaLog.LogMessage("Toggling progress panel state preview...")
        ProgressPanelPic.Image = If(CheckBox7.Checked, My.Resources.progresspanel_logview_shown, My.Resources.progresspanel_logview_hidden)
    End Sub

    Private Sub CheckBox20_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox20.CheckedChanged
        If CheckBox20.Checked Then
            DynaLog.DisableLogging()
            MainForm.EnableDynaLog = False
        Else
            MainForm.EnableDynaLog = True
            DynaLog.EnableLogging()
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        EditorOFD.ShowDialog(Me)
    End Sub

    Private Sub EditorOFD_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles EditorOFD.FileOk
        TextBox5.Text = EditorOFD.FileName
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "explorer.exe"), "/select," & Quote & Path.Combine(Application.StartupPath, "logs", "DT_DynaLog.log") & Quote)
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If File.Exists(Path.Combine(Application.StartupPath, "tools", "ThemeDesigner", "DT_ThemeDesigner.exe")) Then
            Process.Start(Path.Combine(Application.StartupPath, "tools", "ThemeDesigner", "DT_ThemeDesigner.exe"),
                          String.Format("/userdata={0} {1}", ControlChars.Quote & Path.Combine(Application.StartupPath, "userdata", "themes") & ControlChars.Quote, LocalizationService.GetLanguageCommandLineArgument()))
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim qhMessage As String = LocalizationService.ForSection("Options.QuickHelp").Format("DISM.Tools.Enable.Message", Environment.NewLine)
        ShowQuickHelp(qhMessage)
    End Sub

    Private Sub ComboBox8_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox8.SelectedIndexChanged
        Dim exampleDisplayName As String = "UbisoftEntertainment.RaymanJungleRun",
            exampleFriendlyDisplayName As String = "Rayman Jungle Run"

        Select Case ComboBox8.SelectedIndex
            Case 0
                Label75.Text = exampleDisplayName
            Case 1
                Label75.Text = String.Format("{0} ({1})", exampleDisplayName, exampleFriendlyDisplayName)
            Case 2
                Label75.Text = exampleFriendlyDisplayName
        End Select
    End Sub

    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        Dim qhMessage As String = LocalizationService.ForSection("Options.QuickHelp").Format("AppX.Package.Display.Message", Environment.NewLine, Quote)
        QuickHelpModule.ShowQuickHelp(qhMessage)
    End Sub

    Private Sub ComboBox7_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox7.SelectedIndexChanged
        If SearchEngineHelper.GetAllSearchEngines().ElementAt(ComboBox7.SelectedIndex).AIPermission > ComboBox9.SelectedIndex Then
            ' The user has selected a search engine with a higher AI tolerance level.
            If MessageBox.Show(LocalizationService.ForSection("Options").Format("Selected.Search.Message", Environment.NewLine, Quote, ComboBox7.SelectedItem, ComboBox9.SelectedItem),
                                         LocalizationService.ForSection("Options")("Aitolerance.Exceeded.Title"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.No Then
                ComboBox7.SelectedItem = SearchEngineHelper.GetAllSearchEngines().First(Function(engine) engine.AIPermission = ComboBox9.SelectedIndex).Name
            End If
        End If
    End Sub

    Private Sub LinkLabel5_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked
        Dim qhMessage As String = LocalizationService.ForSection("Options.QuickHelp").Format("Configure.Search.Message", Environment.NewLine, Quote)
        QuickHelpModule.ShowQuickHelp(qhMessage)
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Dim qhMessage As String = LocalizationService.ForSection("Options.QuickHelp").Format("Bg.Procs.Allow.Message", Environment.NewLine)
        QuickHelpModule.ShowQuickHelp(qhMessage)
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Try
            If WindowsServiceHelper.InstallService(New WindowsService("DT_AutoReload",
                                                                      LocalizationService.ForSection("Options.AutoReloadService")("DISM.Tools.Automatic.Label"), "", "",
                                                                      Path.Combine(Application.StartupPath, "AutoReload", "AutoReloadSvc.exe"),
                                                                      "", WindowsService.ServiceStartType.Automatic, False,
                                                                      WindowsService.ServiceType.WindowsApplication,
                                                                      WindowsService.ServiceErrorControl.Normal,
                                                                      {}.Cast(Of NTSecurityPrivilegeConstant).ToList(),
                                                                      {"EventLog"}, New WindowsService.ServiceFailureActions(), Integer.MinValue)) Then
                ' Set the description manually
                WindowsServiceHelper.SetOnlineServiceDescription("DT_AutoReload", LocalizationService.ForSection("Options.AutoReloadService")("AutoReload.Description"))

                GetAIRServiceInformation()
            Else
                Throw New Exception(LocalizationService.ForSection("Options.AutoReloadService")("ServiceInstalled.Label"))
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, ImageTaskHeader1.ItemText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If WindowsServiceHelper.EnableOnlineService("DT_AutoReload") Then
            GetAIRServiceInformation()
        Else
            MessageBox.Show(LocalizationService.ForSection("Options.Messages")("ServiceEnabled.Label"), ImageTaskHeader1.ItemText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        If WindowsServiceHelper.DisableOnlineService("DT_AutoReload") Then
            GetAIRServiceInformation()
        Else
            MessageBox.Show(LocalizationService.ForSection("Options.Messages")("ServiceDisabled.Label"), ImageTaskHeader1.ItemText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        If WindowsServiceHelper.DeleteService("DT_AutoReload") Then
            GetAIRServiceInformation()
        Else
            MessageBox.Show(LocalizationService.ForSection("Options.Messages")("ServiceRemoved.Label"), ImageTaskHeader1.ItemText, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "system32", "dism.exe"),
                      "/cleanup-mountpoints")
    End Sub

    Private Sub DTProjAssocCB_CheckedChanged(sender As Object, e As EventArgs) Handles DTProjAssocCB.CheckedChanged
        CheckBox11.Enabled = DTProjAssocCB.Checked
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        ' Compare by processor family
        Try
            Dim processorFamilyMOC As ManagementObjectCollection = WMIHelper.GetResultsFromManagementQuery("SELECT Family FROM Win32_Processor")
            If processorFamilyMOC IsNot Nothing Then
                Dim processorFamily As Integer = WMIHelper.GetObjectValue(processorFamilyMOC(0), "Family")
                Dim processorDetails As ProcessorFamilyCategory = SpecialProcessorFamilies.FirstOrDefault(Function(procFamily) procFamily.Family = processorFamily)

                If processorDetails IsNot Nothing Then
                    Select Case processorDetails.Beefiness
                        Case ProcessorFamilyBeefiness.Potato : NumericUpDown2.Value = 2
                        Case ProcessorFamilyBeefiness.Average : NumericUpDown2.Value = 5
                        Case ProcessorFamilyBeefiness.Beefy : NumericUpDown2.Value = 10
                    End Select

                    MessageBox.Show(String.Format("Based on your processor's specifications, the program has been configured to support up to {1} concurrent ISO creation tasks.{0}{0}" &
                                                  "Apart from your processor, consider other specifications in your system that may become bottlenecks, such as disk I/O, or memory bandwidth.", Environment.NewLine, NumericUpDown2.Value), ImageTaskHeader1.ItemText, MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class

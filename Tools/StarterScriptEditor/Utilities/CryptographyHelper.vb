Imports System.Security.Cryptography
Imports System.IO
Imports System.Text
#If VBC_VER >= 10.0 Then
Imports System.Linq
#End If

Public Class CryptographyHelper

    ' AES was originally known as Rijndael. .NET 2, while it does support AES (AES-256),
    ' only supports it by using RijndaelManaged, as opposed to Aes in newer .NET Framework
    ' versions.
    Private aes As New RijndaelManaged()

    Public Sub New()
        ' let's configure our encryption algorithm to use AES-256
        aes.KeySize = 256
        aes.BlockSize = 128
        aes.Mode = CipherMode.CBC
        aes.Padding = PaddingMode.PKCS7
    End Sub

    Public Sub EncryptStringToFile(ByVal PlainTextMessage As String, ByVal OutputFile As String, ByVal Key As Byte(), ByVal Salt As Byte())
        aes.Key = Key
        aes.GenerateIV()

        Using fs As New FileStream(OutputFile, FileMode.Create, FileAccess.Write)
            fs.Write(Salt, 0, Salt.Length)
            fs.Write(aes.IV, 0, aes.IV.Length)
            Using cs As New CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write)
                Dim PlainBytes As Byte() = Encoding.UTF8.GetBytes(PlainTextMessage)
                cs.Write(PlainBytes, 0, PlainBytes.Length)
                cs.FlushFinalBlock()
            End Using
        End Using
    End Sub

    Public Sub EncryptStringToFile(ByVal PlainTextMessage As String, ByVal OutputFile As String, ByVal Password As String)
#If VBC_VER >= 10.0 Then
        Dim Salt(15) As Byte
        Using rng As RandomNumberGenerator = RandomNumberGenerator.Create()
            rng.GetBytes(Salt)
        End Using

        Using pbkdf2 As New Rfc2898DeriveBytes(Password, Salt, 500000, HashAlgorithmName.SHA256)
            Dim EncryptionKey As Byte() = pbkdf2.GetBytes(32)
            Dim HmacKey As Byte() = pbkdf2.GetBytes(32)
            Dim IV(15) As Byte
            Dim CipherText As Byte()

            Using aes As Aes = Aes.Create()
                aes.KeySize = 256
                aes.BlockSize = 128
                aes.Mode = CipherMode.CBC
                aes.Padding = PaddingMode.PKCS7
                aes.Key = EncryptionKey

                aes.GenerateIV()
                IV = aes.IV

                Using ms As New MemoryStream()
                    Using cs As New CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write)
                        Dim PlainBytes As Byte() = Encoding.UTF8.GetBytes(PlainTextMessage)
                        cs.Write(PlainBytes, 0, PlainBytes.Length)
                        cs.FlushFinalBlock()
                    End Using
                    CipherText = ms.ToArray()
                End Using
            End Using

            Dim Tag As Byte()
            Using hmac As New HMACSHA256(HmacKey)
                Dim AuthData As Byte() = Salt.Concat(IV).Concat(CipherText).ToArray()
                Tag = hmac.ComputeHash(AuthData)
            End Using

            Using fs As New FileStream(OutputFile, FileMode.Create, FileAccess.Write)
                fs.Write(Salt, 0, Salt.Length)
                fs.Write(IV, 0, IV.Length)
                fs.Write(CipherText, 0, CipherText.Length)
                fs.Write(Tag, 0, Tag.Length)
            End Using
        End Using
#Else
        Throw New NotSupportedException("This function is not supported on .NET 2.")
#End If
    End Sub

    Private Function FixedTimeEquals(ByVal a As Byte(), ByVal b As Byte()) As Boolean
        If a Is Nothing OrElse b Is Nothing Then Return False
        If a.Length <> b.Length Then Return False

        Dim diff As Integer = 0

        For i As Integer = 0 To a.Length - 1
            diff = diff Or (a(i) Xor b(i))
        Next

        Return diff = 0
    End Function

    Public Function DecryptStringFromFile(ByVal InputFile As String, ByVal Password As String) As String
        If Not File.Exists(InputFile) Then Return ""
#If VBC_VER >= 10.0 Then
        Using fs As New FileStream(InputFile, FileMode.Open, FileAccess.Read)
            Dim Salt(15) As Byte
            Dim IV(15) As Byte
            Dim Tag(31) As Byte

            fs.Read(Salt, 0, Salt.Length)
            fs.Read(IV, 0, IV.Length)

            Dim CipherTextLength As Integer = CInt(fs.Length - Salt.Length - IV.Length - Tag.Length)
            Dim CipherText(CipherTextLength - 1) As Byte

            fs.Read(CipherText, 0, CipherText.Length)
            fs.Read(Tag, 0, Tag.Length)

            Using pbkdf2 As New Rfc2898DeriveBytes(Password, Salt, 500000, HashAlgorithmName.SHA256)
                Dim EncryptionKey As Byte() = pbkdf2.GetBytes(32)
                Dim HmacKey As Byte() = pbkdf2.GetBytes(32)

                Using hmac As New HMACSHA256(HmacKey)
                    Dim AuthData As Byte() = Salt.Concat(IV).Concat(CipherText).ToArray()
                    Dim ComputedTag As Byte() = hmac.ComputeHash(AuthData)

                    If Not FixedTimeEquals(Tag, ComputedTag) Then
                        Throw New CryptographicException("Authentication failed.")
                    End If
                End Using

                Using aes As Aes = Aes.Create()
                    aes.KeySize = 256
                    aes.BlockSize = 128
                    aes.Mode = CipherMode.CBC
                    aes.Padding = PaddingMode.PKCS7
                    aes.Key = EncryptionKey
                    aes.IV = IV

                    Using ms As New MemoryStream(CipherText)
                        Using cs As New CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read)
                            Using sr As New StreamReader(cs, Encoding.UTF8)
                                Return sr.ReadToEnd()
                            End Using
                        End Using
                    End Using
                End Using
            End Using
        End Using
#Else
        Using fs As New FileStream(InputFile, FileMode.Open, FileAccess.Read)
            Dim IV(15) As Byte, _
                Salt(15) As Byte
            fs.Read(Salt, 0, Salt.Length)
            fs.Read(IV, 0, IV.Length)

            Dim pbkdf2 As New Rfc2898DeriveBytes(Password, Salt, 500000)
            Dim Key As Byte() = pbkdf2.GetBytes(32)

            aes.Key = Key
            aes.IV = IV

            Using cs As New CryptoStream(fs, aes.CreateDecryptor(), CryptoStreamMode.Read)
                Using sr As New StreamReader(cs, Encoding.UTF8)
                    Return sr.ReadToEnd()
                End Using
            End Using
        End Using
#End If
    End Function

End Class

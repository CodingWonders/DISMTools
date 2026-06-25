Imports IniParser.Model.Configuration

Namespace Elements.IniParserConfigurators

    Public Class FfuIniParserConfiguration
        Inherits IniParserConfiguration

        Public Sub New()
            ' We have to work around certain quirks with the FFU manifest. For instance, there is one [Partition] section
            ' for EACH partition in a FFU file (VHD), so 4 partitions constitute 4 [Partition] sections. Same goes for keys,
            ' which are duplicated for EACH of the [Partition] sections. Microsoft also thought of leaving an empty space on the
            ' second-to-last line that can, and will, throw off the parser. Also, "ManiOS"? Typoing in production, eh?

            AllowDuplicateSections = True
            AllowDuplicateKeys = True
            SkipInvalidLines = True
        End Sub

    End Class

End Namespace

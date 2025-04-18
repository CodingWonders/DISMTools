Public Interface IImageTaskDialog

    ''' <summary>
    ''' Initializes an image task dialog
    ''' </summary>
    ''' <returns>Whether or not the image task dialog can be initialized</returns>
    ''' <remarks>This is only applied to edition-related tasks in 0.6.2. Version 0.7 will have all startup code moved to implementations of this function</remarks>
    Function Initialize() As Boolean

End Interface

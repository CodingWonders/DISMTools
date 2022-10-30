## Panel category help
---

The external dialogs accessed in DISMTools are stored in categorized folders depending on its functionality:

- The `DoWork` folder contains dialogs or panels that perform a number of actions:
    - `ProgressPanel`
    - `PleaseWaitDialog`
- The `Exe_Ops` folder contains settings the program will save or load:
    - `Options`
- The `Get_Ops` folder contains settings the program will obtain from the image or from the project:
    - `GetImgInfoDlg`
- The `Img_Ops` folder contains actions the program can perform to an image:
    - `ImgAppend`
    - `ImgMount`
    - `ImgUMount`
- The `Internal` folder contains actions the program will do internally (like loading or saving projects):
    - `ProjectValueLoadForm`

    > If run on a VS debugging session, the form will be shown when saving or loading a project
- The `MSEdge` folder contains actions related to Microsoft Edge servicing the program can perform to a mounted image:
    - `AddEdgeBrowser`
    - `AddEdgeFull`
    - `AddEdgeWebView`
- The `Proj_Ops` folder contains forms that view or set project settings:
    - `NewProj`
    - `ProjProperties`
- The `Questions` folder contains questions that will be asked to the user:
    - `OrphanedMountedImgDialog`
    - `SaveProjectQuestionDialog`
- The `Set_Ops` folder contains settings the program will be set to the image or to the project:

The developer discourages you to delete ANY of these files, as they are essential to the program's functionality. However, you can modify them to add or fix buggy code
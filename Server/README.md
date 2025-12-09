# Clean PlayerIO Server

This is a clean starter project for PlayerIO server-side code.

## How to use

1.  **Launch Server**: Just double-click `RunServer.bat`. It will:
    *   Build your code in `GameCode/`.
    *   Start the Player.IO Development Server.
2.  **Edit Code**:
    *   Project is in `GameCode/CleanPlayerIOServer.csproj`.
    *   Main logic: `GameCode/GameCode.cs`.
    *   Room logic: `GameCode/Rooms/`.
    *   Models: `GameCode/Models/`.
3.  **Manual Build** (Optional):
    *   Open `GameCode/CleanPlayerIOServer.csproj` in your IDE.
    *   Build.
    *   The DLL is output to `bin/Debug` (relative to the root).

## References
*   `Lib/Player.IO GameLibrary.dll`: Referenced by the project.
*   `Player.IO GameLibrary.dll` (Root): Used by the Development Server.

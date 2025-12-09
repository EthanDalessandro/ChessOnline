@echo off
echo ==========================================
echo       REBUILDING PLAYERIO SERVER
echo ==========================================
echo.

:: Go to project directory
cd GameCode

:: Build with dotnet
echo Running dotnet build...
dotnet build CleanPlayerIOServer.csproj
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ERROR] Build FAILED!
    echo Please check the error messages above.
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo [SUCCESS] Build Complete!

:: Copy to parent bin folder to ensure it updates where the server might be looking
echo.
echo Copying DLL to ..\bin\Debug\ ...
:: (dotnet build already puts it there based on csproj, but let's be sure)

echo Copying DLL to ..\bin\ ...
copy "..\bin\Debug\CleanPlayerIOServer.dll" "..\bin\CleanPlayerIOServer.dll" /Y

echo.
echo ==========================================
echo    DONE! 
echo    1. Close any running PlayerIO Server window.
echo    2. Start the PlayerIO Server again.
echo    3. Check that the DLL date is NOW.
echo ==========================================
pause

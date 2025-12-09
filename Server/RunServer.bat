@echo off
echo Building CleanPlayerIOServer...

REM Define path to MSBuild. Try to find commonly used paths.
set "MSBUILD=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"

if not exist "%MSBUILD%" (
    echo Error: Could not find MSBuild at %MSBUILD%
    echo Please ensure .NET Framework 4.0 is installed.
    pause
    exit /b 1
)

"%MSBUILD%" "GameCode\CleanPlayerIOServer.csproj" /p:Configuration=Debug /verbosity:minimal

if %ERRORLEVEL% NEQ 0 (
    echo Build failed!
    pause
    exit /b 1
)

echo.
echo Starting Player.IO Development Server...
start "" "Player.IO Development Server.exe"

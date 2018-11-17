@echo off

::The following command executes Unity in batch mode, executes the MyEditorScript.PerformBuild method, and then quits upon completion.

set PRE_BUILD=xcopy /y ..\Resources\*.graphml  ..\Resources\*.txt

%PRE_BUILD%

set PROJECT=-projectPath 
set PROJECT_PATH="%USERPROFILE%\Documents\workspace\YouMeetInATavern"

set WIN_PATH="%USERPROFILE%\Documents\Builds\YouMeetInATavern\youmeetinatavern.exe"
set OSX_PATH="%USERPROFILE%\Documents\project\Unity\UGDE\build\osx\island.app"

@REM With Unity 4 we now have Linux
set LINUX_PATH="%USERPROFILE%\Documents\project\Unity\UGDE\build\linux\island.app"

set LINUX64_PATH="%USERPROFILE%\Documents\project\Unity\UGDE\build\linux64\island.app"

@REM Common options
set BATCH=-batchmode
set QUIT=-quit

@REM Builds:
set WIN=-buildWindowsPlayer %WIN_PATH%
set OSX=-buildOSXPlayer %OSX_PATH%
set LINUX=-buildLinux32Player %LINUX_PATH%
set LINUX64=-buildLinux64Player %LINUX64_PATH%

@REM Win32 build
"%PROGRAMFILES%\Unity\Editor\Unity.exe" %BATCH% %QUIT% %PRE_BUILD% %PROJECT_PATH% %WIN%
::"%ProgramFiles(x86)%\Unity\Editor\Unity.exe" %BATCH% %QUIT% %PRE_BUILD% %PROJECT_PATH% %WIN%

@REM OSX build
:: echo Running OSX Build for: %PROJECT_PATH%
:: echo "%PROGRAMFILES%\Unity\Editor\Unity.exe" %BATCH% %QUIT% %PRE_BUILD% %PROJECT_PATH% %OSX%
:: "%ProgramFiles(x86)%\Unity\Editor\Unity.exe" %BATCH% %QUIT% %PRE_BUILD% %PROJECT_PATH% %OSX%

@REM Linux build
:: echo Running Linux Build for: %PROJECT_PATH%
:: echo "%PROGRAMFILES%\Unity\Editor\Unity.exe" %BATCH% %QUIT% %PRE_BUILD% %PROJECT_PATH% %LINUX%
:: "%ProgramFiles(x86)%\Unity\Editor\Unity.exe" %BATCH% %QUIT% %PRE_BUILD% %PROJECT_PATH% %LINUX%

@REM Linux 64-bit build
:: echo Running Linux Build for: %PROJECT_PATH%
:: echo "%PROGRAMFILES%\Unity\Editor\Unity.exe" %BATCH% %QUIT% %PRE_BUILD% %PROJECT_PATH% %LINUX64%
:: "%ProgramFiles(x86)%\Unity\Editor\Unity.exe" %BATCH% %QUIT% %PRE_BUILD% %PROJECT_PATH% %LINUX64%


PAUSE
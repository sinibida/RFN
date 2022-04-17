@echo off
echo F|xcopy /S /Q /Y /F "App\bin\Release\App.exe" "C:\Program Files (x86)\RFN\RFN.exe"
echo F|xcopy /S /Q /Y /F "App\bin\Release\*.dll" "C:\Program Files (x86)\RFN\*.dll"
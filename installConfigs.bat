@echo off
echo F|xcopy /S /Q /Y /F "App\bin\Release\commands.json" "C:\Program Files (x86)\RFN\commands.json"
echo F|xcopy /S /Q /Y /F "App\bin\Release\config.json" "C:\Program Files (x86)\RFN\config.json"
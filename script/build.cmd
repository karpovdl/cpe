:: Script for building an application for Windows

echo off

call killprocess.cmd

cd ..

:: Kill running processes
:: SETLOCAL EnableExtensions

:: set EXE=cpe.exe
:: FOR /F %%x IN ('tasklist /NH /FI "IMAGENAME eq %EXE%"') DO IF %%x == %EXE% taskkill /F /IM cpe.exe /T

:: Del old data
IF EXIST "publish" RMDIR "publish" /Q /S
IF EXIST "cpe/bin" RMDIR "cpe/bin" /Q /S

:: Publishing [Release] exe file
dotnet publish -r win-x64 -c Release -o publish/cpe --self-contained

:: Publishing [Release] a single exe file
dotnet publish -r win-x64 -c Release -o publish/cpe.single /p:PublishSingleFile=true

:: Publishong [Release] a single exe min file
dotnet publish -r win-x64 -c Release -o publish/cpe.single.min /p:PublishSingleFile=true /p:PublishTrimmed=true

:: Del files
cd publish
DEL cpe.pdb /Q /S
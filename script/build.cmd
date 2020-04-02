:: Script for building an application for Windows

cd ..

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
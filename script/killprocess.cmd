:: Kill running processes

echo off 

SETLOCAL EnableExtensions

set EXE=cpe.exe
FOR /F %%x IN ('tasklist /NH /FI "IMAGENAME eq %EXE%"') DO IF %%x == %EXE% taskkill /F /IM cpe.exe /T
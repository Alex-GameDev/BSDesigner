@echo off
setlocal

REM Define las carpetas de origen y destino
set "source=..\BSDesigner\bin"
set "destination=..\Unity\Assets\libs"

REM Verifica que la carpeta de origen exista
if not exist "%source%" (
    echo La carpeta de origen no existe: %source%
    exit /b 1
)

REM Verifica que la carpeta de destino exista, si no, la crea
if not exist "%destination%" (
    echo La carpeta de destino no existe. Creando: %destination%
    mkdir "%destination%"
)

REM Copia los archivos de la carpeta de origen a la de destino, sobreescribiendo los existentes
xcopy "%source%\*" "%destination%\" /s /e /y

REM Comprueba si la copia fue exitosa
if %errorlevel% neq 0 (
    echo Error al copiar los archivos.
    exit /b 1
)

echo Archivos copiados exitosamente de %source% a %destination%
endlocal
exit /b 0
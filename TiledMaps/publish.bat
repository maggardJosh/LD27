@ECHO OFF
IF NOT "%1"=="" GOTO setFilename
SET /P filename="Enter map name(don't include extension): "
GOTO finish

:setFilename
SET filename=%1 

:finish
copy .\%filename%.tmx ..\Assets\Resources\Maps\%filename%.txt && echo "Copied %filename%.tmx to ..\Assets\Resources\Maps\%filename%.txt" || echo "Nothing copied"

pause
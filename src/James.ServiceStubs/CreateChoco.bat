@ECHO OFF

REM Delete Any Artifacts
REM ====================
IF EXIST build (
	cd build
	del /f /q servicestubs.latest.nupkg
	cd ..
) ELSE (
	mkdir build
)

SET spec="servicestubs.nuspec"
SET package="servicestubs.latest.nupkg"

REM Create the Package
REM ==================

ECHO "Packing Chocolatey package based on the following specification:  %spec%."
cd James.ServiceStubs.CommandLine
choco pack %spec%
ren *.nupkg %package%
move %package% ..\build
cd ..

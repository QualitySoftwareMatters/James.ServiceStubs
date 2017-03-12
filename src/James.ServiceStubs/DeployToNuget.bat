@ECHO OFF

REM Update Nuget
REM ============
nuget.exe update -self

REM Delete Any Artifacts
REM ====================
IF EXIST build (
	cd build
	del /f /q James.ServiceStubs.*.nupkg
	cd ..
) ELSE (
	mkdir build
)

REM Requests the API Key
REM ====================
SET /p NuGetApiKey= Please enter the project's NuGet API Key: 
nuget.exe setApiKey %NuGetApiKey%

SET package="James.ServiceStubs\James.ServiceStubs.csproj"

REM Create the Package
REM ==================
ECHO "Packing/Pushing project found here:  %package%."
nuget.exe pack -Build -OutputDirectory build %package% -Prop Configuration=Release

REM Push to Nuget 
REM =============
cd build
nuget.exe push James.ServiceStubs.*.nupkg -Source https://www.nuget.org/api/v2/package
cd ..
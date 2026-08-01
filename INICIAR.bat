@echo off
cd /d "%~dp0"
dotnet run --project ".\src\Nexus.Desktop\Nexus.Desktop.csproj"
if errorlevel 1 pause

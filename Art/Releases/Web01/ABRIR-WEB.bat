@echo off
setlocal
cd /d "%~dp0"
if exist "%~dp0..\..\..\tools\envs\unity\Scripts\python.exe" (
  "%~dp0..\..\..\tools\envs\unity\Scripts\python.exe" "%~dp0serve.py"
  exit /b
)
where py >nul 2>nul
if not errorlevel 1 (
  py -3 "%~dp0serve.py"
  exit /b
)
echo Para testar localmente, instale Python 3 ou publique esta pasta em um servidor web.
echo Nao abra index.html diretamente pelo Explorador de Arquivos.
pause

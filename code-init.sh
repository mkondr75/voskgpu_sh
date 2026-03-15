#!/bin/bash
# Постоянный лаунчер изолированного окружения

CODE_EXE="/c/vscodeport/Code.exe"
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

# Подготовка Windows-путей (критично для Code.exe)
WIN_DIR=$(cygpath -w "$SCRIPT_DIR")
WIN_DATA=$(cygpath -w "$SCRIPT_DIR/.vscode/user-data")
WIN_EXT=$(cygpath -w "$SCRIPT_DIR/.vscode/extensions")

# Создаем папки, если их нет (mkdir -p не вредит существующим)
mkdir -p "$SCRIPT_DIR/.vscode/user-data"
mkdir -p "$SCRIPT_DIR/.vscode/extensions"

export VSCODE_PORTABLE="$WIN_DATA"
export MSYSTEM=UCRT64
export CHERE_INVOKING=1

echo "--- VS Code Launcher ---"
echo "Project Path: $WIN_DIR"

# Запуск: логи в файл (на случай если захочешь почитать), процесс в фон
"$CODE_EXE" --user-data-dir "$WIN_DATA" --extensions-dir "$WIN_EXT" --new-window "$WIN_DIR" > .vscode/last-run-log.txt 2>&1 &

disown
echo "VS Code запущен. Консоль свободна для команд."

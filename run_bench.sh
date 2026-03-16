#!/bin/bash
# Правило 9: Нативное наследие. Логика вызова сохранена.
# Мы используем бинарники из deploy, а данные из test_data.

EXE="./SHARP/SAMPLES/deploy/SAMPLES.exe"
MODEL="c:/MAX/download/speechtotext/models/ru/vosk-model-small-ru-0.22"
DATA_DIR="./test_data"
LOG_FILE="local_bench_$(date +%Y%m%d_%H%M).log"

echo "=== VOSK GPU PROJECT: LOCAL CPU BENCHMARK ===" | tee $LOG_FILE
echo "Model: $MODEL" | tee -a $LOG_FILE
echo "--------------------------------------------" | tee -a $LOG_FILE

# Цикл по всем wav файлам
for wav in $DATA_DIR/*.wav; do
    [ -e "$wav" ] || continue
    FILENAME=$(basename "$wav")
    echo "FILE: $FILENAME" | tee -a $LOG_FILE
    
    # Запуск. Передаем модель и путь к аудио.
    # Фильтруем вывод, оставляя только самое важное.
    $EXE "$MODEL" "$wav" | grep -E "\[METRIC\]|\[FINAL RESULT\]|\"text\"" | tee -a $LOG_FILE
    
    echo "--------------------------------------------" | tee -a $LOG_FILE
done

echo "Done. Results saved to $LOG_FILE"

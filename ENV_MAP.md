# UCRT-Forge: Vosk-GPU Environment Map

## 1. Local (Windows 11 / MSYS2)
- **Project Root:** `C:/msys64/home/maxko/dev/sharp/voskgpu_sh`
- **Model Path:** MODEL="c:/MAX/download/speechtotext/models/ru/vosk-model-small-ru-0.22"
- **Audio Path:** `C:/vosk_test/tst_data/test.wav`
- **Audio Path:** c:\msys64\home\maxko\dev\sharp\voskgpu_sh\test_data\test.wav 
- **Run Command:** `./SAMPLES.exe "C:/path/to/model" "C:/path/to/audio"`

## 2. Remote (GCP VM - instance-20260311-151421)
- **Deploy Path:** `C:\vosk_test\v03`
- **Model Path:** `C:\vosk_test\models\vosk-model-small-ru-0.22`
- **Audio Path:** `C:\vosk_test\tst_data\test.wav`

## 3. Media Tools (Portable)
- **FFmpeg:** `c:/dev/ffmpeg/bin/ffmpeg.exe` (Added to PATH)
- **yt-dlp:** `c:/dev/downloader/yt-dlp.exe`
- **Benchmarks:** `c:/dev/downloader/ru_bench_30s.zip` (6 samples, 16kHz, Mono)

временное:
maxko@MAX_NOTE UCRT64 C:/msys64/home/maxko/dev/sharp/voskgpu_sh/SHARP/SAMPLES/deploy
$ # Определяем пути для удобства
EXE="./SAMPLES.exe"
MODEL="c:/MAX/download/speechtotext/models/ru/vosk-model-small-ru-0.22"
AUDIO_DIR="c:/dev/downloader"

# 1. Тест "Easy" 15 секунд
echo "--- Testing Easy Sample ---"
$EXE "$MODEL" "$AUDIO_DIR/easy_15s.wav"

# 2. Тест "Hard" 15 секунд
echo "--- Testing Hard Sample ---"
$EXE "$MODEL" "$AUDIO_DIR/hard_15s.wav"

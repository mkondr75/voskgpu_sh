# UCRT-Forge: Vosk-GPU Environment Map

## 1. Local (Windows 11 / MSYS2)
- **Project Root:** `C:/msys64/home/maxko/dev/sharp/voskgpu_sh`
- **Model Path:** MODEL="c:/MAX/download/speechtotext/models/ru/vosk-model-small-ru-0.22"
- **Audio Path:** c:\msys64\home\maxko\dev\sharp\voskgpu_sh\test_data\*.wav 
- **Run Command:** `./SAMPLES.exe "C:/path/to/model" "C:/path/to/audio"`
c:\msys64\home\maxko\dev\sharp\voskgpu_sh\SHARP\SAMPLES\deploy\SAMPLES.exe "c:\MAX\download\speechtotext\models\ru\vosk-model-small-ru-0.22" "c:\msys64\home\maxko\dev\sharp\voskgpu_sh\test_data"

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

# 1. Тест "Easy" 15 секунд есть 30 60 имена с другим цифрами
echo "--- Testing Easy Sample ---"
$EXE "$MODEL" "$AUDIO_DIR/easy_15s.wav"

# 2. Тест "Hard" 15 секунд есть 30 60 имена с другим цифрами
echo "--- Testing Hard Sample ---"
$EXE "$MODEL" "$AUDIO_DIR/hard_15s.wav"

cmd:
c:\msys64\home\maxko\dev\sharp\voskgpu_sh\SHARP\SAMPLES\deploy>c:\msys64\home\maxko\dev\sharp\voskgpu_sh\SHARP\SAMPLES\deploy\SAMPLES.exe "c:\MAX\download\speechtotext\models\ru\vosk-model-small-ru-0.22" "c:\msys64\home\maxko\dev\sharp\voskgpu_sh\test_data"
=== UCRT-Forge: Vosk GPU MULTI-STREAM Benchmark ===
[INFO] Loading Shared Model: c:\MAX\download\speechtotext\models\ru\vosk-model-small-ru-0.22
LOG (VoskAPI:ReadDataFiles():src\model.cc:223) Decoding params beam=10 max-active=3000 lattice-beam=2
LOG (VoskAPI:ReadDataFiles():src\model.cc:226) Silence phones 1:2:3:4:5:6:7:8:9:10
LOG (VoskAPI:RemoveOrphanNodes():nnet-nnet.cc:948) Removed 0 orphan nodes.
LOG (VoskAPI:RemoveOrphanComponents():nnet-nnet.cc:847) Removing 0 orphan components.
LOG (VoskAPI:CompileLooped():nnet-compile-looped.cc:345) Spent 0.020334 seconds in looped compilation.
LOG (VoskAPI:ReadDataFiles():src\model.cc:259) Loading i-vector extractor from c:\MAX\download\speechtotext\models\ru\vosk-model-small-ru-0.22/ivector/final.ie
LOG (VoskAPI:ComputeDerivedVars():ivector-extractor.cc:183) Computing derived variables for iVector extractor
LOG (VoskAPI:ComputeDerivedVars():ivector-extractor.cc:204) Done.
LOG (VoskAPI:ReadDataFiles():src\model.cc:295) Loading HCL and G from c:\MAX\download\speechtotext\models\ru\vosk-model-small-ru-0.22/graph/HCLr.fst c:\MAX\download\speechtotext\models\ru\vosk-model-small-ru-0.22/graph/Gr.fst
LOG (VoskAPI:ReadDataFiles():src\model.cc:321) Loading winfo c:\MAX\download\speechtotext\models\ru\vosk-model-small-ru-0.22/graph/phones/word_boundary.int
[INFO] Starting Parallel Processing for 7 files...
[THREAD 1] Finished test.wav in 2913 ms
[THREAD 2] Finished hard_15s.wav in 8352 ms
[THREAD 3] Finished easy_15s.wav in 8670 ms
[THREAD 4] Finished hard_30s.wav in 10699 ms
[THREAD 5] Finished easy_30s.wav in 11109 ms
[THREAD 6] Finished easy_60s.wav in 12650 ms
[THREAD 7] Finished hard_60s.wav in 12756 ms
The current time is 15:48:55 Hrs.

=== BENCHMARK RESULTS ===
Files processed:    7
Sum of task times:  67149 ms
Total wall clock:   12852 ms
Boost factor:       5,22x
==========================

c:\msys64\home\maxko\dev\sharp\voskgpu_sh\SHARP\SAMPLES\deploy>
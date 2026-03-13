/*
maxko@MAX_NOTE UCRT64 ~/dev/sharp/voskgpu_sh
$ g++ -shared -fPIC CPP/vosk_api.cpp -o CPP/voskgpu.dll

maxko@MAX_NOTE UCRT64 ~/dev/sharp/voskgpu_sh
$ cp CPP/voskgpu.dll SHARP/SAMPLES/bin/Debug/net10.0/

maxko@MAX_NOTE UCRT64 ~/dev/sharp/voskgpu_sh
$ cd SHARP/SAMPLES/bin/Debug/net10.0/

maxko@MAX_NOTE UCRT64 ~/dev/sharp/voskgpu_sh/SHARP/SAMPLES/bin/Debug/net10.0
$ ./SAMPLES.exe
=== UCRT-Forge: Interop Test ===
[C#] Calling native library...
[NATIVE] Checking CUDA availability...
[C#] Native response: 0
[C#] Result: GPU not detected (as expected in stub).

maxko@MAX_NOTE UCRT64 ~/dev/sharp/voskgpu_sh/SHARP/SAMPLES/bin/Debug/net10.0
$ ls -l vosk_gpu_debug.log
-rw-r--r-- 1 maxko maxko 31 мар 10 20:00 vosk_gpu_debug.log

maxko@MAX_NOTE UCRT64 ~/dev/sharp/voskgpu_sh/SHARP/SAMPLES/bin/Debug/net10.0
$ cat vosk_gpu_debug.log
Checking CUDA availability...

maxko@MAX_NOTE UCRT64 ~/dev/sharp/voskgpu_sh/SHARP/SAMPLES/bin/Debug/net10.0
*/

#include "vosk_api.h"
#include <iostream>
#include <fstream>

void log_to_file(const char* message) {
    // Используем абсолютный путь или просто имя. 
    // Внимание: std::ios::app - дозапись
    std::ofstream log_file("vosk_gpu_debug.log", std::ios::app);
    if (log_file.is_open()) {
        log_file << message << std::endl;
        log_file.flush(); // ГАРАНТИРУЕМ запись на диск
        log_file.close();
    } else {
        // Если файл не открылся - выведем в консоль причину
        std::cerr << "[NATIVE ERROR] Could not open log file for writing!" << std::endl;
    }
}

VOSK_API void vosk_gpu_init_logger(const char* log_path) {
    // Временно игнорируем log_path и пишем в текущую папку для теста
    log_to_file("=== INITIALIZING UCRT-FORGE LOGGER ===");
    std::cout << "[NATIVE] Logger attempt finished." << std::endl;
}

VOSK_API int vosk_gpu_check_availability() {
    std::cout << "[NATIVE] Checking CUDA availability..." << std::endl;
    log_to_file("Checking CUDA availability...");
    return 0; 
}
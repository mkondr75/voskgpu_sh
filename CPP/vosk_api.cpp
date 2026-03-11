#include "vosk_api.h"
#include <iostream>
#include <fstream>
#include <cuda_runtime.h> // Тот самый файл из твоего SDK!

void log_to_file(const char* message) {
    std::ofstream log_file("vosk_gpu_debug.log", std::ios::app);
    if (log_file.is_open()) {
        log_file << message << std::endl;
        log_file.flush();
        log_file.close();
    }
}

VOSK_API int vosk_gpu_check_availability() {
    int deviceCount = 0;
    
    // Пытаемся вызвать реальную функцию из CUDA SDK
    cudaError_t error = cudaGetDeviceCount(&deviceCount);
    
    if (error != cudaSuccess) {
        std::string err_msg = "[NATIVE] CUDA Error: " + std::string(cudaGetErrorString(error));
        std::cout << err_msg << std::endl;
        log_to_file(err_msg.c_str());
        return -1; // Ошибка драйвера или отсутствие CUDA
    }

    std::cout << "[NATIVE] CUDA Devices found: " << deviceCount << std::endl;
    log_to_file(("Devices found: " + std::to_string(deviceCount)).c_str());
    
    return deviceCount;
}

// Остальные функции (init_logger и т.д.) оставь как были

VOSK_API void vosk_gpu_init_logger(const char* log_path) {
    // Временно игнорируем log_path и пишем в текущую папку для теста
    log_to_file("=== INITIALIZING UCRT-FORGE LOGGER ===");
    std::cout << "[NATIVE] Logger attempt finished." << std::endl;
}

//VOSK_API int vosk_gpu_check_availability() {
//    std::cout << "[NATIVE] Checking CUDA availability..." << std::endl;
//    log_to_file("Checking CUDA availability...");
//    return 0; 
//}
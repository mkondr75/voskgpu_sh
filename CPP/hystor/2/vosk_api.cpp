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
    
    // 1. Сначала ГАРНТИРОВАННО сбрасываем старые ошибки, если они были в стеке
    cudaGetLastError(); 
    
    // 2. Вызываем проверку
    cudaError_t error = cudaGetDeviceCount(&deviceCount);
    
    if (error != cudaSuccess) {
        // Если ошибка реально есть, выведем её код цифрой для дебага
        std::string err_msg = "[NATIVE] CUDA Status: " + std::to_string((int)error) + 
                              " (" + std::string(cudaGetErrorString(error)) + ")";
        std::cout << err_msg << std::endl;
        log_to_file(err_msg.c_str());
        
        // Маленький хак: если девайсы всё же найдены (больше 0), 
        // игнорируем "предупреждение" и пробуем работать.
        if (deviceCount > 0) {
             std::cout << "[NATIVE] Warning ignored, devices available." << std::endl;
             return deviceCount;
        }
        return -1; 
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
VOSK_API IntPtr vosk_model_new(const char* model_path) {
    std::cout << "[NATIVE] Received model path: " << model_path << std::endl;
    // Пока возвращаем фиктивный указатель
    return (IntPtr)1; 
}

VOSK_API void vosk_model_free(IntPtr model) {
    std::cout << "[NATIVE] Model freed." << std::endl;
}

//VOSK_API int vosk_gpu_check_availability() {
//    std::cout << "[NATIVE] Checking CUDA availability..." << std::endl;
//    log_to_file("Checking CUDA availability...");
//    return 0; 
//}
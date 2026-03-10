#include "vosk_api.h"
#include <iostream>
#include <fstream>
#include <string>

// Реализация логгера для "соседа"
VOSK_API void vosk_gpu_init_logger(const char* log_path) {
    std::ofstream log_file(log_path, std::ios::app);
    if (log_file.is_open()) {
        log_file << "[INFO] Logger initialized at 2026-03-10" << std::endl;
        log_file.close();
    }
    std::cout << "[NATIVE] Logger path set to: " << log_path << std::endl;
}

VOSK_API int vosk_gpu_check_availability() {
    std::cout << "[NATIVE] Checking CUDA availability..." << std::endl;
    // Пока возвращаем 0 (CPU-only), имитируем отсутствие GPU
    return 0; 
}

VOSK_API void* vosk_model_new(const char* model_path) {
    std::cout << "[NATIVE] Creating model from: " << model_path << std::endl;
    return (void*)0x12345678; // Фейковый указатель
}

VOSK_API void vosk_model_free(void* model) {
    std::cout << "[NATIVE] Freeing model at: " << model << std::endl;
}
// ============================================================================
// Project: UCRT-Forge (Vosk GPU Edition)
// File: vosk_api.h
// Version: 0.2.0-alpha
// Description: Native API for CUDA-accelerated Vosk
// ============================================================================

#ifndef VOSK_GPU_API_H
#define VOSK_GPU_API_H

#ifdef _WIN32
    #define VOSK_API __declspec(dllexport)
#else
    #define VOSK_API __attribute__((visibility("default")))
#endif

extern "C" {

/**
 * Проверяет доступность CUDA в системе.
 * Возвращает: 1 - доступно, 0 - только CPU, -1 - ошибка драйвера.
 */
VOSK_API int vosk_gpu_check_availability();

/**
 * Инициализирует расширенный логгинг в файл.
 * Помогает отладить падения на чужих GPU (у соседа).
 */
VOSK_API void vosk_gpu_init_logger(const char* log_path);

/**
 * Стандартные функции Vosk (заглушки для нашей прослойки)
 */
VOSK_API void* vosk_model_new(const char* model_path);
VOSK_API void vosk_model_free(void* model);

} // extern "C"

#endif // VOSK_GPU_API_H
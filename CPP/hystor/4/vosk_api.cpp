#include "vosk_api.h"
#include <iostream>
#include <fstream>
#include <string>

// Объявляем внешние функции из libvosk.dll (они подцепятся через libvosk.dll.a)
extern "C" {
    void* vosk_model_new_base(const char* model_path) __asm__("vosk_model_new");
    void vosk_model_free_base(void* model) __asm__("vosk_model_free");
    void* vosk_recognizer_new_base(void* model, float sample_rate) __asm__("vosk_recognizer_new");
    int vosk_recognizer_accept_waveform_base(void* recognizer, const char* data, int length) __asm__("vosk_recognizer_accept_waveform");
    const char* vosk_recognizer_result_base(void* recognizer) __asm__("vosk_recognizer_result");
    void vosk_recognizer_free_base(void* recognizer) __asm__("vosk_recognizer_free");
}

static std::string global_log_path = "vosk_gpu_default.log";

VOSK_API int vosk_gpu_check_availability() {
#ifdef USE_CUDA
    // В будущем тут будет реальный cudaGetDeviceCount
    return 1; 
#else
    return 0;
#endif
}

VOSK_API void vosk_gpu_init_logger(const char* log_path) {
    if (log_path) global_log_path = log_path;
    std::ofstream log(global_log_path, std::ios::app);
    log << "[INFO] Logger initialized inside voskgpu.dll" << std::endl;
}

VOSK_API void* vosk_model_new(const char* model_path) {
    return vosk_model_new_base(model_path);
}

VOSK_API void vosk_model_free(void* model) {
    vosk_model_free_base(model);
}

VOSK_API void* vosk_recognizer_new(void* model, float sample_rate) {
    return vosk_recognizer_new_base(model, sample_rate);
}

VOSK_API int vosk_recognizer_accept_waveform(void* recognizer, const char* data, int length) {
    return vosk_recognizer_accept_waveform_base(recognizer, data, length);
}

VOSK_API const char* vosk_recognizer_result(void* recognizer) {
    return vosk_recognizer_result_base(recognizer);
}

VOSK_API void vosk_recognizer_free(void* recognizer) {
    vosk_recognizer_free_base(recognizer);
}

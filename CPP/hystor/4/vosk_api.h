// ============================================================================
// Project: UCRT-Forge (Vosk GPU Edition)
// File: vosk_api.h
// Version: 0.2.1-alpha (Extended for Recognition)
// ============================================================================

#ifndef VOSK_GPU_API_H
#define VOSK_GPU_API_H

#ifdef _WIN32
    #define VOSK_API __declspec(dllexport)
#else
    #define VOSK_API __attribute__((visibility("default")))
#endif

extern "C" {

// --- GPU & Service ---
VOSK_API int vosk_gpu_check_availability();
VOSK_API void vosk_gpu_init_logger(const char* log_path);

// --- Model Management ---
VOSK_API void* vosk_model_new(const char* model_path);
VOSK_API void vosk_model_free(void* model);

// --- Recognizer Management (Pass-through to libvosk) ---
VOSK_API void* vosk_recognizer_new(void* model, float sample_rate);
VOSK_API int vosk_recognizer_accept_waveform(void* recognizer, const char* data, int length);
VOSK_API const char* vosk_recognizer_result(void* recognizer);
VOSK_API void vosk_recognizer_free(void* recognizer);

} // extern "C"
#endif 

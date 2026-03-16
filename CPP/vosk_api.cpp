#include "vosk_api.h"
#include <windows.h>
#include <cuda_runtime.h>
#include <iostream>
#include <fstream>
#include <string>
#include <ctime>

typedef void* (*PFN_VOSK_MODEL_NEW)(const char*);
typedef void (*PFN_VOSK_MODEL_FREE)(void*);
typedef void* (*PFN_VOSK_RECOGNIZER_NEW)(void*, float);
typedef int (*PFN_VOSK_RECOGNIZER_ACCEPT_WAVEFORM)(void*, const char*, int);
typedef const char* (*PFN_VOSK_RECOGNIZER_RESULT)(void*);
typedef void (*PFN_VOSK_RECOGNIZER_FREE)(void*);

static HMODULE hBaseVosk = NULL;
static std::string global_log_path = "vosk_gpu_debug.log";

void* GetVoskFunc(const char* name) {
    if (!hBaseVosk) {
        hBaseVosk = GetModuleHandleA("libvosk.dll");
        if (!hBaseVosk) hBaseVosk = LoadLibraryA("libvosk.dll");
    }
    return (void*)GetProcAddress(hBaseVosk, name);
}

VOSK_API int vosk_gpu_check_availability() {
    int runtimeVersion = 0;
    int driverVersion = 0;
    cudaRuntimeGetVersion(&runtimeVersion);
    cudaDriverGetVersion(&driverVersion);
    std::time_t now = std::time(0); // Get current time
    struct tm tstruct;
    char buf[80];
    tstruct = *std::localtime(&now); // Convert to local time structure
    // Use strftime to format the time
    strftime(buf, sizeof(buf), "%Y-%m-%d.%X", &tstruct); // %X is the locale's time representation

    std::ofstream log(global_log_path, std::ios::app);
    log << "[NATIVE] CUDA Runtime Version: " << runtimeVersion << buf << std::endl;
    log << "[NATIVE] CUDA Driver Version: " << driverVersion << std::endl;

    int deviceCount = 0;
    cudaError_t error = cudaGetDeviceCount(&deviceCount);
    
    if (error != cudaSuccess) {
        log << "[NATIVE] CUDA Error: " << cudaGetErrorString(error) << std::endl;
        return -1;
    }
    return deviceCount;
}

VOSK_API void vosk_gpu_init_logger(const char* log_path) {
    if (log_path) global_log_path = log_path;
    std::ofstream log(global_log_path, std::ios::app);
    log << "=== INITIALIZING UCRT-FORGE LOGGER ===" << std::endl;
}

VOSK_API void* vosk_model_new(const char* model_path) {
    auto f = (PFN_VOSK_MODEL_NEW)GetVoskFunc("vosk_model_new");
    return f ? f(model_path) : nullptr;
}

VOSK_API void vosk_model_free(void* model) {
    auto f = (PFN_VOSK_MODEL_FREE)GetVoskFunc("vosk_model_free");
    if (f) f(model);
}

VOSK_API void* vosk_recognizer_new(void* model, float sample_rate) {
    auto f = (PFN_VOSK_RECOGNIZER_NEW)GetVoskFunc("vosk_recognizer_new");
    return f ? f(model, sample_rate) : nullptr;
}

VOSK_API int vosk_recognizer_accept_waveform(void* recognizer, const char* data, int length) {
    auto f = (PFN_VOSK_RECOGNIZER_ACCEPT_WAVEFORM)GetVoskFunc("vosk_recognizer_accept_waveform");
    return f ? f(recognizer, data, length) : -1;
}

VOSK_API const char* vosk_recognizer_result(void* recognizer) {
    auto f = (PFN_VOSK_RECOGNIZER_RESULT)GetVoskFunc("vosk_recognizer_result");
    return f ? f(recognizer) : nullptr;
}

VOSK_API void vosk_recognizer_free(void* recognizer) {
    auto f = (PFN_VOSK_RECOGNIZER_FREE)GetVoskFunc("vosk_recognizer_free");
    if (f) f(recognizer);
}

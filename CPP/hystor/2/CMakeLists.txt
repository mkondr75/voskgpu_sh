cmake_minimum_required(VERSION 3.18)
project(VoskGpuProject LANGUAGES CXX)

# 1. Определяем базовый путь к вашему кастомному SDK
set(MY_CUDA_SDK "C:/dev/cuda_sdk")

# 2. Собираем все пути к инклудам
# ДОБАВЛЕНО: путь к заголовкам nvcc, где лежит crt/host_config.h
set(CUDA_INCLUDE_DIRS
    "${MY_CUDA_SDK}/cuda_cudart/cudart/include"
    "${MY_CUDA_SDK}/cuda_cccl/include"
    "${MY_CUDA_SDK}/cuda_nvcc/nvcc/include" # <-- Вот здесь живет crt/
)

# 3. Собираем библиотеку
add_library(voskgpu SHARED vosk_api.cpp)

# 4. Подключаем инклуды как SYSTEM
target_include_directories(voskgpu SYSTEM PRIVATE ${CUDA_INCLUDE_DIRS})

# 5. Настройка линкера
target_link_directories(voskgpu PRIVATE "${MY_CUDA_SDK}/lib/x64")

# ИСПРАВЛЕНО: MY_CUDA_PATH заменено на MY_CUDA_SDK
#target_link_libraries(voskgpu PRIVATE "${MY_CUDA_SDK}/lib/x64/cudart_static.lib")
target_link_libraries(voskgpu PRIVATE "${MY_CUDA_SDK}/lib/x64/cudart.lib")

# 6. Настройка имен для Windows
set_target_properties(voskgpu PROPERTIES PREFIX "" IMPORT_PREFIX "")

# Добавим макрос, чтобы код знал, что мы пробуем CUDA
target_compile_definitions(voskgpu PRIVATE USE_CUDA)
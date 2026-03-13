cmake_minimum_required(VERSION 3.18)
project(VoskGpuProject LANGUAGES CXX)

set(MY_CUDA_SDK "C:/dev/cuda_sdk")
set(VOSK_LIB_DIR "${CMAKE_CURRENT_SOURCE_DIR}/lib")

# Инклуды
set(CUDA_INCLUDE_DIRS
    "${MY_CUDA_SDK}/cuda_cudart/cudart/include"
    "${MY_CUDA_SDK}/cuda_cccl/include"
    "${MY_CUDA_SDK}/cuda_nvcc/nvcc/include"
)

add_library(voskgpu SHARED vosk_api.cpp)

# Указываем путь к нашему заголовочному файлу vosk_api.h
target_include_directories(voskgpu PRIVATE "${CMAKE_CURRENT_SOURCE_DIR}")
target_include_directories(voskgpu SYSTEM PRIVATE ${CUDA_INCLUDE_DIRS})

# Директории поиска библиотек
target_link_directories(voskgpu PRIVATE "${MY_CUDA_SDK}/lib/x64" "${VOSK_LIB_DIR}")

# Линкуемся: для MinGW/MSYS2 указываем либо имя без расширения, либо полный путь
# cudart.lib обычно подхватывается из CUDA SDK, для vosk используем .dll.a
target_link_libraries(voskgpu PRIVATE 
    "${MY_CUDA_SDK}/lib/x64/cudart.lib" 
    "${VOSK_LIB_DIR}/libvosk.dll.a"
)

set_target_properties(voskgpu PROPERTIES PREFIX "" IMPORT_PREFIX "")
target_compile_definitions(voskgpu PRIVATE USE_CUDA)

# АВТОМАТИЗАЦИЯ: Копируем всё в build, чтобы чемодан собирался сам
add_custom_command(TARGET voskgpu POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy_if_different
        "${VOSK_LIB_DIR}/libvosk.dll"
        $<TARGET_FILE_DIR:voskgpu>
    COMMENT "Deploying libvosk.dll to build folder..."
)
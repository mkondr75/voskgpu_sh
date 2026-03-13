cmake_minimum_required(VERSION 3.18)
project(voskgpu_native LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)

# 1. Указываем путь к твоему портативному SDK
set(MY_CUDA_PATH "C:/dev/cuda_sdk")

# 2. Добавляем пути для поиска заголовочных файлов и библиотек
include_directories(${MY_CUDA_PATH}/include)
link_directories(${MY_CUDA_PATH}/lib/x64)

# 3. Собираем библиотеку
add_library(voskgpu SHARED vosk_api.cpp)

# 4. Линкуем статическую CUDA (чтобы DLL была автономной)
# Мы вручную указываем cudart_static.lib из твоей папки
target_link_libraries(voskgpu PRIVATE "${MY_CUDA_PATH}/lib/x64/cudart_static.lib")

# 5. Настройка имен для Windows
set_target_properties(voskgpu PROPERTIES PREFIX "" IMPORT_PREFIX "")

# Добавим макрос, чтобы код знал, что мы пробуем CUDA
target_compile_definitions(voskgpu PRIVATE USE_CUDA)
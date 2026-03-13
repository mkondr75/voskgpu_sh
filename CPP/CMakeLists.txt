cmake_minimum_required(VERSION 3.18)
project(VoskGpuProject LANGUAGES CXX)

set(MY_CUDA_SDK "C:/dev/cuda_sdk")
set(VOSK_LIB_DIR "${CMAKE_CURRENT_SOURCE_DIR}/lib")
set(TEST_RUN_DIR "${CMAKE_CURRENT_SOURCE_DIR}/../test_run")

set(CUDA_INCLUDE_DIRS
    "${MY_CUDA_SDK}/cuda_cudart/cudart/include"
    "${MY_CUDA_SDK}/cuda_cccl/include"
    "${MY_CUDA_SDK}/cuda_nvcc/nvcc/include"
)

add_library(voskgpu SHARED vosk_api.cpp)
target_include_directories(voskgpu PRIVATE "${CMAKE_CURRENT_SOURCE_DIR}")
target_include_directories(voskgpu SYSTEM PRIVATE ${CUDA_INCLUDE_DIRS})
target_link_directories(voskgpu PRIVATE "${MY_CUDA_SDK}/lib/x64" "${VOSK_LIB_DIR}")

target_link_libraries(voskgpu PRIVATE 
    "${MY_CUDA_SDK}/lib/x64/cudart.lib" 
)

set_target_properties(voskgpu PROPERTIES PREFIX "" IMPORT_PREFIX "")
target_compile_definitions(voskgpu PRIVATE USE_CUDA)

# Автоматический деплой в папку теста
add_custom_command(TARGET voskgpu POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E make_directory "${TEST_RUN_DIR}"
    COMMAND ${CMAKE_COMMAND} -E copy_if_different "${VOSK_LIB_DIR}/libvosk.dll" "${TEST_RUN_DIR}/"
    COMMAND ${CMAKE_COMMAND} -E copy_if_different "$<TARGET_FILE:voskgpu>" "${TEST_RUN_DIR}/"
    COMMENT "Deploying binaries to test_run folder..."
)

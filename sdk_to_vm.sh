#!/bin/bash
# Скрипт прицельной доливки CUDA библиотек на ВМ (v0.4)
# 15.03.2026

SDK_BIN="/c/dev/cuda_sdk/bin"
REMOTE_PATH="C:\\vosk_test\\v03"
INSTANCE="instance-20260311-151421"
ZONE="us-central1-b"

# Список файлов "Критического Минимума"
FILES=(
    "cudart64_12.dll"
    "cublas64_12.dll"
    "cublasLt64_12.dll"
    "cusolver64_12.dll"
    "cusparse64_12.dll"
    "cudnn64_9.dll"
    "cudnn_ops64_9.dll"
    "cudnn_cnn64_9.dll"
    "cudnn_engines_runtime_compiled64_9.dll"
)
#cublas64_12.dll    cudnn_engines_precompiled64_9.dll       cudnn_ops64_9.dll    nvblas64_12.dll
#cublasLt64_12.dll  cudnn_engines_runtime_compiled64_9.dll  cudnn64_9.dll        nvvm64_40_0.dll
#cudart64_12.dll    cudnn_graph64_9.dll                     cusolver64_12.dll
#cudnn_adv64_9.dll  cudnn_heuristic64_9.dll                 cusolverMg64_12.dll
#cudnn_cnn64_9.dll  cusparse64_12.dll


echo "=== STARTING INCREMENTAL SDK DEPLOY ==="

for FILE in "${FILES[@]}"; do
    if [ -f "$SDK_BIN/$FILE" ]; then
        echo "[SENDING] $FILE..."
        gcloud compute scp "$SDK_BIN/$FILE" ${INSTANCE}:${REMOTE_PATH} --zone=${ZONE}
    else
        echo "[ERROR] File NOT FOUND in SDK: $FILE"
    fi
done

echo "=== DEPLOY COMPLETE ==="

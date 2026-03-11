// ============================================================================
// Project: UCRT-Forge (Vosk GPU Edition)
// File: NativeMethods.cs
// Version: 0.2.0-alpha
// Description: P/Invoke definitions for voskgpu.dll
// ============================================================================

using System;
using System.Runtime.InteropServices;

namespace UCRTForge.Vosk.Core
{
    //internal static class NativeMethods
    public static class NativeMethods
    {
        private const string LibName = "voskgpu.dll";

        // Проверка: есть ли вообще CUDA в системе?
        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int vosk_gpu_check_availability();

        // Инициализация логгера для "соседских" тестов
        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void vosk_gpu_init_logger(string logPath);

        // Создание модели (стандартный Vosk API, но в нашей DLL)
        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr vosk_model_new(string modelPath);

        [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void vosk_model_free(IntPtr model);
    }
}

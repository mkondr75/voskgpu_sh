using System;
using System.IO;
using System.Runtime.InteropServices;

Console.WriteLine("=== UCRT-Forge: Vosk GPU Full Cycle Test ===");

// Дефолтные пути на стороне ВМ
string defaultWav = @"C:\vosk_test\tst_data\test.wav";
string defaultModel = @"C:\vosk_test\models\vosk-model-small-ru-0.22";

string wavPath = args.Length > 0 ? args[0] : defaultWav;
string modelPath = args.Length > 1 ? args[1] : defaultModel;

if (args.Length == 0)
{
    Console.WriteLine($"[INFO] No arguments provided. Using defaults:");
    Console.WriteLine($"  WAV: {wavPath}");
    Console.WriteLine($"  Model: {modelPath}");
}

try 
{
    // 1. Проверка GPU
    Console.WriteLine("[C#] Initializing GPU Check...");
    int gpuStatus = NativeMethods.vosk_gpu_check_availability();
    Console.WriteLine($"[C#] GPU Status: {gpuStatus} (1=OK, 0=CPU, -1=Error)");

    NativeMethods.vosk_gpu_init_logger("vosk_gpu_debug.log");

    // 2. Загрузка модели
    Console.WriteLine($"[C#] Loading model...");
    IntPtr model = NativeMethods.vosk_model_new(modelPath);
    
    if (model == IntPtr.Zero)
    {
        Console.WriteLine($"[ERROR] Failed to load model from {modelPath}!");
        return;
    }
    Console.WriteLine("[C#] Model loaded.");

    // 3. Инициализация распознавателя (16кГц - стандарт для малых моделей)
    IntPtr rec = NativeMethods.vosk_recognizer_new(model, 16000.0f);
    
    // 4. Чтение и распознавание
    if (File.Exists(wavPath))
    {
        byte[] audioData = File.ReadAllBytes(wavPath);
        Console.WriteLine($"[C#] Processing {audioData.Length} bytes of PCM data...");
        
        // В упрощенном тесте скармливаем всё разом
        NativeMethods.vosk_recognizer_accept_waveform(rec, audioData, audioData.Length);
        
        IntPtr resultPtr = NativeMethods.vosk_recognizer_result(rec);
        string jsonResult = Marshal.PtrToStringAnsi(resultPtr);
        
        Console.WriteLine("\n[FINAL RESULT] -----------------------");
        Console.WriteLine(jsonResult);
        Console.WriteLine("--------------------------------------\n");
    }
    else
    {
        Console.WriteLine($"[ERROR] Audio file not found: {wavPath}");
    }

    // 5. Очистка
    NativeMethods.vosk_recognizer_free(rec);
    NativeMethods.vosk_model_free(model);
    Console.WriteLine("[C#] Cleanup complete. Resources released.");
}
catch (DllNotFoundException ex) 
{
    Console.WriteLine($"[FATAL] Missing DLL (voskgpu, libvosk, or CUDA): {ex.Message}");
}
catch (Exception ex) 
{
    Console.WriteLine($"[FATAL] Error: {ex.Message}");
}

internal static class NativeMethods
{
    private const string DllName = "voskgpu";

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vosk_gpu_check_availability();

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void vosk_gpu_init_logger(string logPath);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr vosk_model_new(string modelPath);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void vosk_model_free(IntPtr model);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr vosk_recognizer_new(IntPtr model, float sampleRate);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int vosk_recognizer_accept_waveform(IntPtr recognizer, byte[] data, int length);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr vosk_recognizer_result(IntPtr recognizer);

    [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void vosk_recognizer_free(IntPtr recognizer);
}

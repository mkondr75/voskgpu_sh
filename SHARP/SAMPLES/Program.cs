/*
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
*/

/*
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

// Фикс Rule 8: Оставляем нативный импорт, добавляем профилирование
Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("=== UCRT-Forge: Vosk GPU Benchmark Mode ===");

string defaultWav = @"C:\vosk_test\tst_data\test.wav";
string defaultModel = @"C:\vosk_test\models\vosk-model-small-ru-0.22";

// string wavPath = args.Length > 0 ? args[0] : defaultWav;
// string modelPath = args.Length > 1 ? args[1] : defaultModel;
// Правило 9: Прозрачный ввод. Порядок: [Model] [WAV]
string modelPath = args.Length > 0 ? args[0] : defaultModel;
string wavPath = args.Length > 1 ? args[1] : defaultWav;

Console.WriteLine($"[INFO] Target Model: {modelPath}");
Console.WriteLine($"[INFO] Target Audio: {wavPath}");

Stopwatch sw = new Stopwatch();

try 
{
    // 1. Проверка GPU
    sw.Start();
    int gpuStatus = NativeMethods.vosk_gpu_check_availability();
    sw.Stop();
    Console.WriteLine($"[METRIC] GPU Check: {gpuStatus} (Time: {sw.ElapsedMilliseconds} ms)");

    NativeMethods.vosk_gpu_init_logger("vosk_gpu_debug.log");

    // 2. Загрузка модели (Холодный старт)
    Console.WriteLine($"[C#] Loading model: {modelPath}");
    sw.Restart();
    IntPtr model = NativeMethods.vosk_model_new(modelPath);
    sw.Stop();
    
    if (model == IntPtr.Zero) {
        Console.WriteLine("[ERROR] Model load failed!");
        return;
    }
    long modelLoadTime = sw.ElapsedMilliseconds;
    Console.WriteLine($"[METRIC] Model Load Time: {modelLoadTime} ms");

    // 3. Инициализация распознавателя
    IntPtr rec = NativeMethods.vosk_recognizer_new(model, 16000.0f);

    // 4. Обработка аудио
    if (File.Exists(wavPath))
    {
        byte[] audioData = File.ReadAllBytes(wavPath);
        Console.WriteLine($"[C#] Processing {audioData.Length} bytes...");
        
        sw.Restart();
        NativeMethods.vosk_recognizer_accept_waveform(rec, audioData, audioData.Length);
        IntPtr resultPtr = NativeMethods.vosk_recognizer_result(rec);
        sw.Stop();

        // Rule 8: Используем PtrToStringUTF8 для корректной кириллицы
        string jsonResult = Marshal.PtrToStringAnsi(resultPtr); 
        // Примечание: Если в JSON будет "кракозябра", заменим на UTF8-хендлер
        
        Console.WriteLine("\n[FINAL RESULT] -----------------------");
        Console.WriteLine(jsonResult);
        Console.WriteLine("--------------------------------------");
        Console.WriteLine($"[METRIC] Recognition Time: {sw.ElapsedMilliseconds} ms");
    }
    else
    {
        Console.WriteLine($"[ERROR] File not found: {wavPath}");
    }

    // 5. Cleanup
    NativeMethods.vosk_recognizer_free(rec);
    NativeMethods.vosk_model_free(model);
    Console.WriteLine("[C#] Done.");
}
catch (Exception ex) 
{
    Console.WriteLine($"[FATAL] {ex.Message}");
}
*/

/*
using System;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("=== UCRT-Forge: Vosk GPU MULTI-STREAM Benchmark ===");

// Порядок аргументов: [ModelPath] [AudioDir/File]
string modelPath = args.Length > 0 ? args[0] : @"C:\vosk_test\models\vosk-model-small-ru-0.22";
string audioInput = args.Length > 1 ? args[1] : @"C:\vosk_test\tst_data";

if (!Directory.Exists(modelPath)) {
    Console.WriteLine($"[ERROR] Model path not found: {modelPath}");
    return;
}

// Собираем список файлов
List<string> wavFiles = new List<string>();
if (Directory.Exists(audioInput)) {
    wavFiles.AddRange(Directory.GetFiles(audioInput, "*.wav"));
} else if (File.Exists(audioInput)) {
    wavFiles.Add(audioInput);
}

if (wavFiles.Count == 0) {
    Console.WriteLine("[ERROR] No wav files to process.");
    return;
}

Stopwatch globalSw = new Stopwatch();

try {
    // 1. Инициализация модели (Shared)
    Console.WriteLine($"[INFO] Loading Shared Model: {modelPath}");
    IntPtr model = NativeMethods.vosk_model_new(modelPath);
    if (model == IntPtr.Zero) return;

    Console.WriteLine($"[INFO] Starting Parallel Processing for {wavFiles.Count} files...");
    
    globalSw.Start();

    // 2. Параллельная обработка
    // Ограничим степень параллелизма, чтобы не "положить" GPU сразу, если файлов будет 100
    var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
    
    ConcurrentBag<long> individualTimes = new ConcurrentBag<long>();

    Parallel.ForEach(wavFiles, options, (wavPath) => {
        Stopwatch threadSw = Stopwatch.StartNew();
        
        // В каждом потоке свой Recognizer
        IntPtr rec = NativeMethods.vosk_recognizer_new(model, 16000.0f);
        
        if (File.Exists(wavPath)) {
            byte[] audioData = File.ReadAllBytes(wavPath);
            NativeMethods.vosk_recognizer_accept_waveform(rec, audioData, audioData.Length);
            IntPtr resultPtr = NativeMethods.vosk_recognizer_result(rec);
            
            // Нам важна скорость, JSON просто "трогаем", чтобы убедиться в завершении
            string result = Marshal.PtrToStringAnsi(resultPtr); 
            threadSw.Stop();
            
            individualTimes.Add(threadSw.ElapsedMilliseconds);
            Console.WriteLine($"[THREAD {Task.CurrentId}] Finished {Path.GetFileName(wavPath)} in {threadSw.ElapsedMilliseconds} ms");
        }
        
        NativeMethods.vosk_recognizer_free(rec);
    });

    globalSw.Stop();

    // 3. Итоги
    long sumTimes = individualTimes.Sum();
    long parallelTime = globalSw.ElapsedMilliseconds;
    double speedup = (double)sumTimes / parallelTime;
        // Get the current local date and time
    DateTime currentDateTime = DateTime.Now;

        // Print the result to the console
    Console.WriteLine(string.Format("The current time is {0:HH:mm:ss} Hrs.", currentDateTime));
    Console.WriteLine("\n=== BENCHMARK RESULTS ===");
    Console.WriteLine($"Files processed:    {wavFiles.Count}");
    Console.WriteLine($"Sum of task times:  {sumTimes} ms");
    Console.WriteLine($"Total wall clock:   {parallelTime} ms");
    Console.WriteLine($"Boost factor:       {speedup:F2}x");
    Console.WriteLine("==========================");

    NativeMethods.vosk_model_free(model);
}
catch (Exception ex) {
    Console.WriteLine($"[FATAL] {ex.Message}");
}
*/

using System;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("=== UCRT-Forge: Vosk GPU RAM-STRESS Benchmark ===");

// Аргументы: [0] ModelPath, [1] AudioInput, [2] Multiplier (0-XXX)
string modelPath = args.Length > 0 ? args[0] : @"C:\vosk_test\models\vosk-model-small-ru-0.22";
string audioInput = args.Length > 1 ? args[1] : @"C:\msys64\home\maxko\dev\sharp\voskgpu_sh\test_data";
int multiplier = (args.Length > 2 && int.TryParse(args[2], out int m)) ? m : 1;

if (!Directory.Exists(modelPath)) {
    Console.WriteLine($"[ERROR] Model path not found: {modelPath}");
    return;
}

// 1. Инициализация логгера (Абсолютный путь)
string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vosk_gpu_debug.log");
NativeMethods.vosk_gpu_init_logger(logPath);

// 2. Предварительная загрузка в RAM
var sourceFiles = Directory.Exists(audioInput) 
    ? Directory.GetFiles(audioInput, "*.wav") 
    : (File.Exists(audioInput) ? new[] { audioInput } : Array.Empty<string>());

if (!sourceFiles.Any()) {
    Console.WriteLine("[ERROR] No audio files found.");
    return;
}

Console.WriteLine($"[RAM] Preloading {sourceFiles.Length} files...");
var ramBuffers = sourceFiles.Select(f => new {
    Name = Path.GetFileName(f),
    Data = File.ReadAllBytes(f)
}).ToList();

// 3. Плодим задачи согласно множителю
var workQueue = new List<dynamic>();
for (int i = 0; i < multiplier; i++) {
    foreach (var item in ramBuffers) {
        workQueue.Add(new { 
            Id = i, 
            Name = item.Name, 
            Data = item.Data 
        });
    }
}

Console.WriteLine($"[INFO] Total tasks to process: {workQueue.Count} (Multiplier: {multiplier}x)");

try {
    IntPtr model = NativeMethods.vosk_model_new(modelPath);
    if (model == IntPtr.Zero) return;

    string csvPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "benchmark_results.csv");
    
    using (var csv = new StreamWriter(csvPath, false, Encoding.UTF8)) {
        csv.WriteLine("RunId;FileName;ThreadId;DurationMs");
        
        Stopwatch globalSw = Stopwatch.StartNew();
        
        // Ограничиваем параллелизм количеством логических ядер для чистоты CPU-теста
        var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

        Parallel.ForEach(workQueue, options, task => {
            Stopwatch threadSw = Stopwatch.StartNew();
            
            IntPtr rec = NativeMethods.vosk_recognizer_new(model, 16000.0f);
            NativeMethods.vosk_recognizer_accept_waveform(rec, task.Data, task.Data.Length);
            NativeMethods.vosk_recognizer_result(rec); // Нам важен факт обработки
            NativeMethods.vosk_recognizer_free(rec);
            
            threadSw.Stop();

            lock (csv) {
                csv.WriteLine($"{task.Id};{task.Name};{Task.CurrentId};{threadSw.ElapsedMilliseconds}");
            }
        });

        globalSw.Stop();
        
        Console.WriteLine("\n=== STRESS TEST RESULTS ===");
        Console.WriteLine($"Total tasks:      {workQueue.Count}");
        Console.WriteLine($"Total wall clock: {globalSw.ElapsedMilliseconds} ms");
        Console.WriteLine($"Report:           {csvPath}");
    }

    NativeMethods.vosk_model_free(model);
}
catch (Exception ex) {
    Console.WriteLine($"[FATAL] {ex.Message}");
}
// NativeMethods остаются без изменений (см. предыдущий Program.cs)
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
using System;
using UCRTForge.Vosk.Core;

Console.WriteLine("=== UCRT-Forge: Interop Test ===");

try 
{
    Console.WriteLine("[C#] Calling native library...");
    int status = NativeMethods.vosk_gpu_check_availability();
    
    Console.WriteLine($"[C#] Native response: {status}");
    
    if (status == 0) {
        Console.WriteLine("[C#] Result: GPU not detected (as expected in stub).");
    }
}
catch (DllNotFoundException) 
{
    Console.WriteLine("[ERROR] voskgpu.dll not found in search path!");
}
catch (Exception ex) 
{
    Console.WriteLine($"[ERROR] {ex.Message}");
}
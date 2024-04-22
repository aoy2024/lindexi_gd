﻿// See https://aka.ms/new-console-template for more information
#define DebugAllocated

using System.Diagnostics;

using EDIDParser;

var file = "edid";

unsafe
{
#if DebugAllocated
    var currentAllocatedBytes = GC.GetAllocatedBytesForCurrentThread();
    long detalAllocatedBytes = 0;
    var lastAllocatedBytes = currentAllocatedBytes;
#endif

    Span<byte> edidSpan;
    // https://glenwing.github.io/docs/VESA-EEDID-A1.pdf
    // This document describes the basic 128-byte data structure "EDID 1.3", as well as the overall layout of the
    // data blocks that make up Enhanced EDID. 
    const int minLength = 128;
    using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read, minLength))
    {
        LogMemoryAllocated();

        if (fileStream.Length <= minLength * 2)
        {
            edidSpan = stackalloc byte[(int) fileStream.Length];
        }
        else
        {
            edidSpan = new byte[(int) fileStream.Length];
        }

        var readLength = fileStream.Read(edidSpan);
        Debug.Assert(fileStream.Length == readLength);
    }

    LogMemoryAllocated();


    // Header
    var edidHeader = edidSpan[..8];
    if (edidHeader is not [0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00])
    {
        // 这不是一份有效的 edid 文件
        throw new ArgumentException("这不是一份有效的 edid 文件，校验 Header 失败");
    }

    // checksum
    // 3.11 Extension Flag and Checksum
    // This byte should be programmed such that a one-byte checksum of the entire 128-byte EDID equals 00h.
    byte checksumValue = 0;
    for (int i = 0; i < minLength; i++)
    {
        checksumValue += edidSpan[i];
    }

    if (checksumValue != 0)
    {
        throw new ArgumentException("这不是一份有效的 edid 文件，校验 checksum 失败");
    }

    LogMemoryAllocated();

    int[] n = [1, 2, 3];

    var value = n.AsSpan();
    if (value is [2, 2, 3])
    {
        // 底层
        /*
       IL_0021: ldloca.s     'value'
       IL_0023: call         instance int32 valuetype [System.Runtime]System.Span`1<int32>::get_Length()
       IL_0028: ldc.i4.3
       IL_0029: bne.un.s     IL_0051
       IL_002b: ldloca.s     'value'
       IL_002d: ldc.i4.0
       IL_002e: call         instance !0/*int32* /& valuetype [System.Runtime]System.Span`1<int32>::get_Item(int32)
       IL_0033: ldind.i4
       IL_0034: ldc.i4.2
       IL_0035: bne.un.s     IL_0051
       IL_0037: ldloca.s     'value'
       IL_0039: ldc.i4.1
       IL_003a: call         instance !0/*int32* /& valuetype [System.Runtime]System.Span`1<int32>::get_Item(int32)
       IL_003f: ldind.i4
       IL_0040: ldc.i4.2
       IL_0041: bne.un.s     IL_0051
       IL_0043: ldloca.s     'value'
       IL_0045: ldc.i4.2
       IL_0046: call         instance !0/*int32* /& valuetype [System.Runtime]System.Span`1<int32>::get_Item(int32)
       IL_004b: ldind.i4
       IL_004c: ldc.i4.3
       IL_004d: ceq
       IL_004f: br.s         IL_0052
       IL_0051: ldc.i4.0
       IL_0052: stloc.s      V_5
     */
        // 重新转换为低级 C# 代码
        /*
         if (value.Length != 3 || value[0] != 2 || value[1] != 2 || value[2] != 3)
     */
    }

    void LogMemoryAllocated()
    {
#if DebugAllocated
        currentAllocatedBytes = GC.GetAllocatedBytesForCurrentThread();
        detalAllocatedBytes = currentAllocatedBytes - lastAllocatedBytes;
        Console.WriteLine($"内存申请量 {detalAllocatedBytes}");
        lastAllocatedBytes = GC.GetAllocatedBytesForCurrentThread();
#endif // DebugAllocated
    }
}
// 内容很小，全部读取出来也不怕
var data = File.ReadAllBytes(file);
var edid = new EDID(data);

Console.WriteLine("Hello, World!");
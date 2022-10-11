﻿using System.Diagnostics;
using System.Numerics;

using Vortice.Mathematics;
using Vortice.WIC;

using D2D = Vortice.Direct2D1;
using PixelFormat = Vortice.DCommon.PixelFormat;

namespace WakolerwhaKanicabirem;

class Program
{
    // 设置可以支持 Win7 和以上版本。如果用到 WinRT 可以设置为支持 win10 和以上。这个特性只是给 VS 看的，没有实际影响运行的逻辑
    static void Main(string[] args)
    {
        // 对接 D2D 需要创建工厂
        using D2D.ID2D1Factory1 d2DFactory = D2D.D2D1.D2D1CreateFactory<D2D.ID2D1Factory1>();

        var renderTargetProperties = new D2D.RenderTargetProperties(PixelFormat.Premultiplied);

        using var wicImagingFactory = new IWICImagingFactory();
        var width = 1000;
        var height = 1000;
        using var wicBitmap =
            wicImagingFactory.CreateBitmap(width, height, Win32.Graphics.Imaging.Apis.GUID_WICPixelFormat32bppPBGRA);

        D2D.ID2D1RenderTarget d2D1RenderTarget =
            d2DFactory.CreateWicBitmapRenderTarget(wicBitmap, renderTargetProperties);

        using var renderTarget = d2D1RenderTarget;
        var stopwatch = Stopwatch.StartNew();
        // 开始绘制逻辑
        renderTarget.BeginDraw();

        // 随意创建颜色
        var color = new Color4((byte) Random.Shared.Next(255), (byte) Random.Shared.Next(255),
            (byte) Random.Shared.Next(255));
        renderTarget.Clear(color);
        color = new Color4(GetRandom(), GetRandom(), GetRandom());
        using var brush = renderTarget.CreateSolidColorBrush(color);

        for (int i = 0; i < 10; i++)
        {
            var radiusX = 5;
            var radiusY = 5;
            renderTarget.DrawEllipse(new D2D.Ellipse(new Vector2(Random.Shared.Next(width - radiusX), Random.Shared.Next(height - radiusY)), radiusX, radiusY), brush, 2);
        }

        stopwatch.Stop();
        Console.WriteLine($"Draw: {stopwatch.ElapsedMilliseconds}");
        stopwatch.Restart();

        renderTarget.EndDraw();

        stopwatch.Stop();
        Console.WriteLine($"EndDraw: {stopwatch.ElapsedMilliseconds}");
        stopwatch.Restart();

        byte GetRandom() => (byte) Random.Shared.Next(255);

        var file = @"D2D.png";
        using (var fileStream = File.OpenWrite(file))
        {
            using var wicBitmapEncoder =
                wicImagingFactory.CreateEncoder(Win32.Graphics.Imaging.Apis.GUID_ContainerFormatPng);

            wicBitmapEncoder.Initialize(fileStream);
            using var wicFrameEncode = wicBitmapEncoder.CreateNewFrame(out var _);
            wicFrameEncode.Initialize();
            wicFrameEncode.WriteSource(wicBitmap);
            wicFrameEncode.Commit();
            wicBitmapEncoder.Commit();
        }
    }
}
﻿using System.Collections.Generic;
using LightTextEditorPlus.Core.Primitive;

namespace LightTextEditorPlus.Core.Document;

/// <summary>
/// 文本 Run 的渲染数据
/// </summary>
/// todo 此类型考虑给业务渲染层
///  RunRenderInfo
class RunVisualData
{
    public RunVisualData(IImmutableRun run, Size size, IList<Size>? charSizeList, int charIndexInLine)
    {
        Run = run;
        Size = size;
        CharSizeList = charSizeList;
        CharIndexInLine = charIndexInLine;
    }

    /// <summary>
    /// 当前的 Run 用来调试使用
    /// </summary>
    public IImmutableRun Run { get; }

    /// <summary>
    /// 左上角的点，相对于文本框
    /// </summary>
    /// 可用来辅助布局上下标
    public Point LeftTop { set; get; }

    /// <summary>
    /// 尺寸
    /// </summary>
    public Size Size { get; }

    /// <summary>
    /// 相对于行的字符序号
    /// </summary>
    public int CharIndexInLine { get; }

    public int CharCount => Run.Count;

    public IList<Size>? CharSizeList { set; get; }
}
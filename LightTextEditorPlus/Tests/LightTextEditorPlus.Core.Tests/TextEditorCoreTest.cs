﻿using MSTest.Extensions.Contracts;

namespace LightTextEditorPlus.Core.Tests;

[TestClass]
public class TextEditorCoreTest
{
    [ContractTestCase]
    public void TestCreate()
    {
        "测试文本的创建".Test(() =>
        {
            var textEditorCore = new TextEditorCore(new TestPlatformProvider());

            // 没有异常，那就是符合预期
            Assert.IsNotNull(textEditorCore);
        });
    }

    [ContractTestCase]
    public void BuildTextLogger()
    {
        "文本的日志属性不为空，即使平台返回空".Test(() =>
        {
            // Arrange
            var testPlatformProvider = new TestPlatformProvider();

            // Action
            var textEditorCore = new TextEditorCore(testPlatformProvider);

            // Assert
            Assert.IsNotNull(textEditorCore.Logger);
        });
    }

    [ContractTestCase]
    public void EventArrange()
    {
        "文本编辑的事件触发是 DocumentChanging DocumentChanged LayoutCompleted 顺序".Test(() =>
        {
            // Arrange
            var textEditorCore = TestHelper.GetTextEditorCore();
            var raiseCount = 0;

            textEditorCore.DocumentChanging += (sender, args) =>
            {
                // Assert
                Assert.AreEqual(0, raiseCount);
                raiseCount++;
            };

            textEditorCore.DocumentChanged += (sender, args) =>
            {
                // Assert
                Assert.AreEqual(1, raiseCount);
                raiseCount = 2;
            };

            textEditorCore.LayoutCompleted += (sender, args) =>
            {
                // Assert
                Assert.AreEqual(2, raiseCount);
                raiseCount = 3;
            };

            // Action
            textEditorCore.AppendText(TestHelper.PlainNumberText);

            // Assert
            Assert.AreEqual(3, raiseCount);
        });
    }

    [ContractTestCase]
    public void GetDocumentBounds()
    {
        "给文本编辑器追加一段纯文本，在布局渲染完成之后，可以获取到文档的尺寸".Test(() =>
        {
            // Arrange
            var textEditorCore = TestHelper.GetTextEditorCore();

            textEditorCore.LayoutCompleted += (sender, args) =>
            {
                // Assert
                // 可以获取到文档的尺寸
                var documentBounds = textEditorCore.GetDocumentBounds();
                Assert.AreEqual(true, documentBounds.Width > 0);
                Assert.AreEqual(true, documentBounds.Height > 0);
            };

            // Action
            textEditorCore.AppendText(TestHelper.PlainNumberText);
        });
    }

    [ContractTestCase]
    public void AppendText()
    {
        "给文本编辑器追加一段纯文本，先触发 DocumentChanging 再触发 DocumentChanged 事件".Test(() =>
        {
            // Arrange
            var textEditorCore = TestHelper.GetTextEditorCore();
            var raiseCount = 0;

            textEditorCore.DocumentChanging += (sender, args) =>
            {
                // Assert
                Assert.AreEqual(0, raiseCount);
                raiseCount++;
            };

            textEditorCore.DocumentChanged += (sender, args) =>
            {
                // Assert
                Assert.AreEqual(1, raiseCount);
                raiseCount = 2;
            };

            // Action
            textEditorCore.AppendText(TestHelper.PlainNumberText);

            // Assert
            Assert.AreEqual(2, raiseCount);
        });

        // todo 考虑首次追加的就是 \r\n 换行
        // todo 考虑传入 123\r\n123 文本
        // todo 考虑传入 123\r\n123\r\n 文本
    }
}
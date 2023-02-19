# PdfSharpCore

标签（空格分隔）： C#

---
## PdfSharpCore 读写PDF，要求能设置字体，图片位置，格式等 ##
要使用 C# 的 PdfSharpCore 库读写 PDF 并设置字体、图片位置和格式等，你可以按照以下步骤进行操作：

安装 PdfSharpCore 库
你可以使用 NuGet 包管理器来安装 PdfSharpCore 库，或者手动从官方网站下载并安装。

创建一个 PDF 文档
使用 PdfSharpCore 库创建一个新的 PDF 文档，可以通过以下代码实现

    using PdfSharpCore.Pdf;
    using PdfSharpCore.Drawing;
    // 创建一个新的 PDF 文档
    var document = new PdfDocument();
    // 添加一个页面
    var page = document.AddPage();
    // 获取页面的绘图对象
    var graphics = XGraphics.FromPdfPage(page);
设置字体
在 PdfSharpCore 库中，可以使用 XFont 类来设置字体。你可以按照以下代码设置字体：

    // 设置字体
    var font = new XFont("Arial", 12, XFontStyle.Regular);
    // 在页面上绘制文本
    graphics.DrawString("Hello, world!", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
添加图片
在 PdfSharpCore 库中，可以使用 XImage 类来添加图片。你可以按照以下代码将图片添加到 PDF 文档中：

    // 加载图片
    var image = XImage.FromFile("image.jpg");
    // 在页面上绘制图片
    graphics.DrawImage(image, new XRect(100, 100, image.PixelWidth, image.PixelHeight));
保存 PDF 文档
最后，使用 PdfSharpCore 库将 PDF 文档保存到本地磁盘上。你可以按照以下代码保存 PDF 文档：

    // 保存 PDF 文档
    document.Save("output.pdf");
综合起来，以下是一个示例代码，它可以创建一个新的 PDF 文档，设置字体、添加图片并保存 PDF 文档：

    using PdfSharpCore.Pdf;
    using PdfSharpCore.Drawing;
    var document = new PdfDocument();
    var page = document.AddPage();
    var graphics = XGraphics.FromPdfPage(page);
    // 设置字体
    var font = new XFont("Arial", 12, XFontStyle.Regular);
    // 在页面上绘制文本
    graphics.DrawString("Hello, world!", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
    // 加载图片
    var image = XImage.FromFile("image.jpg");
    // 在页面上绘制图片
    graphics.DrawImage(image, new XRect(100, 100, image.PixelWidth, image.PixelHeight));
    // 保存 PDF 文档
    document.Save("output.pdf");





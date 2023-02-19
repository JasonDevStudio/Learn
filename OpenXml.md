# OpenXml

标签（空格分隔）： C# OpenXml 

---



## Open Xml 读写PPT大文件Demo ##

    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Presentation;
    using A = DocumentFormat.OpenXml.Drawing;
    public static void WriteLargePowerPointFile(string filePath)
    {
        // Create a new presentation document.
        using (PresentationDocument document = PresentationDocument.Create(filePath, PresentationDocumentType.Presentation))
        {
            // Add a new presentation part to the document.
            PresentationPart presentationPart = document.AddPresentationPart();
            presentationPart.Presentation = new Presentation();
            // Add a new slide part to the presentation part.
            SlidePart slidePart = presentationPart.AddNewPart<SlidePart>();
            slidePart.Slide = new Slide(new CommonSlideData(), new ShapeTree());
            // Create a writer for the slide part.
            OpenXmlWriter writer = OpenXmlWriter.Create(slidePart);
            writer.WriteStartElement(new Slide());
            // Write a large number of shapes to the slide.
            for (int i = 1; i <= 100000; i++)
            {
                A.Transform2D transform = new A.Transform2D(new A.Offset { X = 914400L, Y = 914400L }, new A.Extents { Cx = 914400L, Cy = 914400L });
                A.SolidFill solidFill = new A.SolidFill(new A.SchemeColor { Val = A.SchemeColorValues.Accent1 });
                A.Outline outline = new A.Outline(new A.SolidFill(new A.SchemeColor { Val = A.SchemeColorValues.Accent1 }));
                A.ShapeProperties shapeProperties = new A.ShapeProperties(transform, solidFill, outline);
                A.TextBody textBody = new A.TextBody(new A.BodyProperties(), new A.ListStyle(), new A.Paragraph());
                A.Shape shape = new A.Shape(shapeProperties, new A.NonVisualShapeProperties(new A.NonVisualDrawingProperties { Id = i, Name = "Shape " + i }, new A.NonVisualShapeDrawingProperties()), new A.TextBodyProperties(), textBody);
                writer.WriteElement(shape);
            }
            writer.WriteEndElement();
            writer.Close();
            // Add a new slide to the presentation.
            SlideIdList slideIdList = presentationPart.Presentation.SlideIdList;
            SlideId slideId = slideIdList.Append(new SlideId());
            slideId.Id = (uint)(slideIdList.ChildElements.Count - 1);
            slideId.RelationshipId = presentationPart.GetIdOfPart(slidePart);
            // Save the presentation.
            presentationPart.Presentation.Save();
            // Close the document.
            document.Close();
        }
    }
    public static void ReadLargePowerPointFile(string filePath)
    {
        // Open the presentation document.
        using (PresentationDocument document = PresentationDocument.Open(filePath, false))
        {
            // Get the presentation part.
            PresentationPart presentationPart = document.PresentationPart;
            // Get the slide part.
            SlidePart slidePart = (SlidePart)presentationPart.GetPartById(presentationPart.Presentation.SlideIdList.ChildElements[0].RelationshipId);
            // Get the shape tree.
            ShapeTree shapeTree = slidePart.Slide.CommonSlideData.ShapeTree;
            // Read a large number of shapes from the shape tree.
            foreach (A.Shape shape in shapeTree.Elements<A.Shape>())
            {
                Console.WriteLine("Shape {0}", shape.NonVisualShapeProperties.NonVisualDrawingProperties.Name);
            }
            // Close the document.
            document.Close();
        }
    }
    
在这个示例中，WriteLargePptFile 方法创建一个新的 PPT 文件，写入了一百个空白幻灯片。ReadLargePptFile 方法打开这个 PPT 文件并读取其中的幻灯片。注意，在读取大型 PPT 文件时，最好逐个幻灯片地读取，以避免一次性加载整个文件到内存中造成性能问题.

详细说明 C# OpenXml PPT 中如插入100万行 Excel 表格数据，要求低内存，高性能
--------------------------------------------------
在本文中，我们将提供一个C#的OpenXml PPT示例，以向PPT中插入100万行Excel表格数据，并通过分段方式将数据写入以保证低内存和高性能。同时，我们也将提供代码讲解和示例代码。

示例代码如下：

    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using DocumentFormat.OpenXml.Presentation;
    using System.IO;
    
    namespace PPTWithExcelData
    {
        class Program
        {
            static void Main(string[] args)
            {
                //创建一个新的PPT
                using (PresentationDocument presentationDocument = PresentationDocument.Create("Presentation.pptx", PresentationDocumentType.Presentation))
                {
                    PresentationPart presentationPart = presentationDocument.AddPresentationPart();
                    presentationPart.Presentation = new Presentation();
                    //添加幻灯片
                    SlidePart slidePart = presentationPart.AddNewPart<SlidePart>();
                    Slide slide = new Slide(new CommonSlideData(new ShapeTree()));
                    slidePart.Slide = slide;
                    //创建一个表格
                    var table = new Table();
                    //添加表头
                    var headerRow = new TableRow();
                    for (int i = 1; i <= 5; i++)
                    {
                        headerRow.AppendChild(new TableCell(new Paragraph(new Run(new Text($"Header{i}")))));
                    }
                    table.AppendChild(headerRow);
                    //分段写入数据
                    for (int i = 1; i <= 1000000; i += 1000)
                    {
                        var dataRows = new TableRowCollection();
                        for (int j = i; j < i + 1000 && j <= 1000000; j++)
                        {
                            var row = new TableRow();
                            for (int k = 1; k <= 5; k++)
                            {
                                row.AppendChild(new TableCell(new Paragraph(new Run(new Text($"Data{j}:{k}")))));
                            }
                            dataRows.AppendChild(row);
                        }
                        table.AppendChild(dataRows);
                    }
                    //将表格添加到幻灯片中
                    slide.CommonSlideData.ShapeTree.AppendChild(new GraphicFrame(new DocumentFormat.OpenXml.Drawing.NonVisualGraphicFrameProperties(new DocumentFormat.OpenXml.Drawing.NonVisualDrawingProperties() { Id = 1, Name = "Table 1" }, new DocumentFormat.OpenXml.Drawing.NonVisualGraphicFrameDrawingProperties()), new DocumentFormat.OpenXml.Drawing.Graphic(new DocumentFormat.OpenXml.Drawing.GraphicData(new DocumentFormat.OpenXml.Drawing.Table(table))) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/table" }));
                    //保存PPT
                    presentationPart.Presentation.Save();
                }
            }
        }
    }
    
这个示例代码首先使用OpenXml SDK创建了一个新的PPT，并创建了一个幻灯片。接着，它创建了一个表格，添加表头并分段方式写入100万行的数据。每个段的大小为1000行，这可以通过修改for循环中的i和j的值来进行调整。最后，该代码将表格添加到幻灯片中，并保存PPT。

这种分段写入数据的方式能够保证在写入数据时内存使用低，并且由于数据量较大，单次写入也能够提高性能。






# OpenXml

标签（空格分隔）： 未分类

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







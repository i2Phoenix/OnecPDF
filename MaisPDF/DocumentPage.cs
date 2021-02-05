using MigraDoc.DocumentObjectModel;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Runtime.InteropServices;

namespace MaisPDF
{
    [Guid("B13C27CB-F053-42E5-A2C3-B40834EA8FDE")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class DocumentPage : IDocumentPage
    {
        private Document Owner { get; set; }
        private PdfPage Page { get; set; }
        private XGraphics GfxContext { get; set; }

        private XColor DebugImageColor = XColor.FromCmyk(0.40, 0.28, 0.88, 0, 0);
        private XColor DebugTextColor = XColor.FromCmyk(0.40, 0.82, 0, 1, 0);

        public DocumentPage(Document document, PdfPage page)
        {
            Owner = document;
            Page = page;
        }

        public void НачатьОтрисовку()
        {
            GfxContext = XGraphics.FromPdfPage(Page);
            GfxContext.SmoothingMode = XSmoothingMode.HighQuality;
        }

        public void ЗавершитьОтрисовку()
        {
            GfxContext.Dispose();
            GfxContext = null;
        }

        public bool НаписатьТекст(string text, double left, double top, double width, double height, string fontName, double fontSize, int fontStyle, int textAlign)
        {
            try
            {
                var graphicsPath = new XGraphicsPath();
                graphicsPath.FillMode = XFillMode.Winding;

                var font = new XFont(fontName, fontSize, (XFontStyle)Enum.Parse(typeof(XFontStyle), fontStyle.ToString()));

                var stringFormat = XStringFormats.Default;

                switch (textAlign)
                {
                    case 0:
                        stringFormat = XStringFormats.BaseLineLeft;
                        break;
                    case 1:
                        stringFormat = XStringFormats.TopLeft;
                        break;
                    case 2:
                        stringFormat = XStringFormats.CenterLeft;
                        break;
                    case 3:
                        stringFormat = XStringFormats.BottomLeft;
                        break;
                    case 4:
                        stringFormat = XStringFormats.BaseLineCenter;
                        break;
                    case 5:
                        stringFormat = XStringFormats.TopCenter;
                        break;
                    case 6:
                        stringFormat = XStringFormats.Center;
                        break;
                    case 7:
                        stringFormat = XStringFormats.BottomCenter;
                        break;
                    case 8:
                        stringFormat = XStringFormats.BaseLineRight;
                        break;
                    case 9:
                        stringFormat = XStringFormats.TopRight;
                        break;
                    case 10:
                        stringFormat = XStringFormats.CenterRight;
                        break;
                    case 11:
                        stringFormat = XStringFormats.BottomRight;
                        break;
                    default:
                        stringFormat = XStringFormats.Default;
                        break;
                }

                var color = XColor.FromCmyk(255, 255, 255, 0);
                var rect = new XRect(new XPoint(new Unit(left, UnitType.Millimeter), new Unit(top, UnitType.Millimeter)), new XSize(new Unit(width, UnitType.Millimeter), new Unit(height, UnitType.Millimeter)));

                if (Owner.Отладка)
                {
                    GfxContext.DrawRectangle(new XSolidBrush(DebugTextColor), rect);
                }

                graphicsPath.AddString(text, font.FontFamily, font.Style, font.Size, rect, stringFormat);

                GfxContext.DrawPath(null, new XSolidBrush(color), graphicsPath);

                return true;
            }
            catch (Exception ex)
            {
                Owner.Ошибка = ex.Message;
                return false;
            }
        }

    }
}

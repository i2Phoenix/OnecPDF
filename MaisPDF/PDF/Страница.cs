using MigraDoc.DocumentObjectModel;
using PdfSharp.Drawing;
using PdfSharp.Drawing.BarCodes;
using PdfSharp.Pdf;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MaisPDF
{
    [Guid("B13C27CB-F053-42E5-A2C3-B40834EA8FDE")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Страница : IСтраница
    {
        private XColor DebugImageColor = XColor.FromCmyk(0.40, 0.28, 0.88, 0, 0);
        private XColor DebugTextColor = XColor.FromCmyk(0.40, 0.82, 0, 1, 0);

        private Документ Owner { get; set; }
        private PdfPage Page { get; set; }

        private XGraphics GfxContext { get; set; }
        private XFont CurrentFont { get; set; }
        private XColor CurrentColor { get; set; }
        private XImage CurrentImage { get; set; }

        public Страница(Документ document, PdfPage page)
        {
            Owner = document;
            Page = page;

            CurrentFont = new XFont("OpenSans", 12.5, XFontStyle.Regular);
            CurrentColor = XColor.FromCmyk(255, 255, 255, 0);
            CurrentImage = null;
            GfxContext = null;
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

        public double ШиринаСтраницы()
        {
            try
            {
                if (Page != null)
                    return Page.Width;

                return 0;
            }
            catch (Exception ex)
            {
                Owner.ErrorMessage = ex.Message;
                return 0;
            }
        }

        public double ВысотаСтраницы()
        {
            try
            {
                if (Page != null)
                    return Page.Height;

                return 0;
            }
            catch (Exception ex)
            {
                Owner.ErrorMessage = ex.Message;
                return 0;
            }
        }

        public void УстановитьШрифт(string fontName, double fontSize, int fontStyle)
        {
            CurrentFont = new XFont(fontName, fontSize, (XFontStyle)Enum.Parse(typeof(XFontStyle), fontStyle.ToString()));
        }

        public void УстановитьЦветCMYK(double cyan, double magenta, double yellow, double black)
        {
            CurrentColor = XColor.FromCmyk(cyan, magenta, yellow, black);
        }

        public void УстановитьЦветRGB(int red, int green, int blue)
        {
            CurrentColor = XColor.FromArgb(red, green, blue);
        }

        public bool Текст(string text, double left, double top, double width, double height, int textAlign = 0)
        {
            try
            {
                var graphicsPath = new XGraphicsPath();
                graphicsPath.FillMode = XFillMode.Winding;

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

                var rect = new XRect(new XPoint(new Unit(left, UnitType.Millimeter), new Unit(top, UnitType.Millimeter)), new XSize(new Unit(width, UnitType.Millimeter), new Unit(height, UnitType.Millimeter)));

                if (Owner.DebugDraw)
                {
                    GfxContext.DrawRectangle(new XSolidBrush(DebugTextColor), rect);
                }

                graphicsPath.AddString(text, CurrentFont.FontFamily, CurrentFont.Style, CurrentFont.Size, rect, stringFormat);

                GfxContext.DrawPath(null, new XSolidBrush(CurrentColor), graphicsPath);

                return true;
            }
            catch (Exception ex)
            {
                Owner.ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool Штрихкод(string barcodeValue, double left, double top, double width, double height)
        {
            try
            {
                MBarcodeEAN13 barCode = new MBarcodeEAN13(barcodeValue);
                barCode.TextLocation = TextLocation.BelowEmbedded;
                barCode.Text = barcodeValue;
                barCode.Size = new XSize(XUnit.FromMillimeter(width), XUnit.FromMillimeter(height));
                barCode.StartChar = Convert.ToChar("*");
                barCode.EndChar = Convert.ToChar("*");
                barCode.Direction = CodeDirection.LeftToRight;

                if (Owner.DebugDraw)
                {
                    GfxContext.DrawRectangle(new XSolidBrush(DebugTextColor), XUnit.FromMillimeter(left), XUnit.FromMillimeter(top), XUnit.FromMillimeter(width), XUnit.FromMillimeter(height));
                }

                GfxContext.DrawBarCode(barCode, new XSolidBrush(CurrentColor), CurrentFont, new XPoint(XUnit.FromMillimeter(left), XUnit.FromMillimeter(top)));

                return true;
            }
            catch (Exception ex)
            {
                Owner.ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool QRКод(string barcodeValue, double left, double top, double scale, int version)
        {
            try
            {
                var barCode = new MBarcodeQR(barcodeValue, scale, version);

                GfxContext.DrawBarCode(barCode, new XSolidBrush(CurrentColor), CurrentFont, new XPoint(XUnit.FromMillimeter(left), XUnit.FromMillimeter(top)));

                return true;
            }
            catch (Exception ex)
            {
                Owner.ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool Линия(double x1, double y1, double x2, double y2, double width)
        {
            try
            {
                GfxContext.DrawLine(new XPen(CurrentColor, new Unit(width, UnitType.Millimeter)), new XPoint(new Unit(x1, UnitType.Millimeter), new Unit(y1, UnitType.Millimeter)), new XPoint(new Unit(x2, UnitType.Millimeter), new Unit(y2, UnitType.Millimeter)));

                return true;
            }
            catch (Exception ex)
            {
                Owner.ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool ЗаполненныйПрямоугольник(double left, double top, double width, double height)
        {
            try
            {
                GfxContext.DrawRectangle(new XSolidBrush(CurrentColor), new XRect(new XPoint(new Unit(left, UnitType.Millimeter), new Unit(top, UnitType.Millimeter)), new XSize(new Unit(width, UnitType.Millimeter), new Unit(height, UnitType.Millimeter))));

                return true;
            }
            catch (Exception ex)
            {
                Owner.ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool Прямоугольник(double left, double top, double width, double height, double penWidth)
        {
            try
            {
                GfxContext.DrawRectangle(new XPen(CurrentColor, new Unit(penWidth, UnitType.Millimeter)), new XRect(new XPoint(new Unit(left, UnitType.Millimeter), new Unit(top, UnitType.Millimeter)), new XSize(new Unit(width, UnitType.Millimeter), new Unit(height, UnitType.Millimeter))));

                return true;
            }
            catch (Exception ex)
            {
                Owner.ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool ЗагрузитьВекторноеИзображение(byte[] imageContent)
        {
            try
            {
                using (MemoryStream loadStream = new MemoryStream(imageContent, 0, imageContent.Length, false, true))
                {
                    CurrentImage = XImage.FromStream(loadStream);
                }

                return true;
            }
            catch (Exception ex)
            {
                Owner.ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool ЗагрузитьРастовоеИзображение(byte[] imageContent)
        {
            try
            {
                var tempFile = $"{Path.GetTempPath()}{Guid.NewGuid()}";
                File.WriteAllBytes(tempFile, imageContent);
                CurrentImage = XImage.FromFile(tempFile);
                File.Delete(tempFile);

                return true;
            }
            catch (Exception ex)
            {
                Owner.ErrorMessage = ex.Message;
                return false;
            }
        }

        public double ШиринаИзображения()
        {
            try
            {
                if (CurrentImage != null)
                    return CurrentImage.PixelWidth;

                return 0;
            }
            catch (Exception ex)
            {
                Owner.ErrorMessage = ex.Message;
                return 0;
            }
        }

        public double ВысотаИзображения()
        {
            try
            {
                if (CurrentImage != null)
                    return CurrentImage.PixelHeight;

                return 0;
            }
            catch (Exception ex)
            {
                Owner.ErrorMessage = ex.Message;
                return 0;
            }
        }

        public bool Изображение(double left, double top)
        {
            try
            {
                if (CurrentImage != null)
                {
                    XRect imageRect;

                    double imageWidth = CurrentImage.PixelWidth * 72 / CurrentImage.HorizontalResolution;
                    double imageHeight = CurrentImage.PixelHeight * 72 / CurrentImage.HorizontalResolution;
                    imageRect = new XRect(new Unit(left, UnitType.Millimeter), new Unit(top, UnitType.Millimeter), imageWidth, imageHeight);

                    GfxContext.DrawImage(CurrentImage, imageRect);

                    if (Owner.DebugDraw)
                    {
                        GfxContext.DrawRectangle(new XSolidBrush(DebugImageColor), imageRect);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Owner.ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool ИзображениеВписать(double left, double top, double width, double height)
        {
            try
            {
                if (CurrentImage != null)
                {
                    XRect imageRect;

                    imageRect = new XRect(new Unit(left, UnitType.Millimeter), new Unit(top, UnitType.Millimeter), new Unit((double)width, UnitType.Millimeter), new Unit((double)height, UnitType.Millimeter));

                    GfxContext.DrawImage(CurrentImage, imageRect);

                    if (Owner.DebugDraw)
                    {
                        GfxContext.DrawRectangle(new XSolidBrush(DebugImageColor), imageRect);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Owner.ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool ИзображениеЦентр(double left, double top, double width, double height)
        {
            try
            {
                if (CurrentImage != null)
                {
                    Unit imageWidth = CurrentImage.PixelWidth * 72 / CurrentImage.HorizontalResolution;
                    Unit imageHeight = CurrentImage.PixelHeight * 72 / CurrentImage.HorizontalResolution;

                    var placeholderRect = new XRect
                    {
                        Height = new Unit(height, UnitType.Millimeter),
                        Width = new Unit(width, UnitType.Millimeter),
                        X = new Unit(left, UnitType.Millimeter),
                        Y = new Unit(top, UnitType.Millimeter)
                    };

                    var aspectRatio = 1d;
                    if (imageWidth > imageHeight)
                    {
                        aspectRatio = placeholderRect.Width / imageWidth;
                    }
                    else
                    {
                        aspectRatio = placeholderRect.Height / imageHeight;
                    }

                    var imageRect = new XRect
                    {
                        Width = imageWidth * aspectRatio,
                        Height = imageHeight * aspectRatio,
                        X = placeholderRect.Left + ((placeholderRect.Width / 2) - (imageWidth * aspectRatio / 2)),
                        Y = placeholderRect.Top + ((placeholderRect.Height / 2) - (imageHeight * aspectRatio / 2))
                    };

                    GfxContext.DrawImage(CurrentImage, imageRect);

                    if (Owner.DebugDraw)
                    {
                        GfxContext.DrawRectangle(new XSolidBrush(DebugImageColor), imageRect);
                        GfxContext.DrawRectangle(new XSolidBrush(DebugImageColor), placeholderRect);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Owner.ErrorMessage = ex.Message;
                return false;
            }
        }
    }
}

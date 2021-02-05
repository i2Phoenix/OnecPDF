using PdfSharp.Drawing;
using System;
using System.Runtime.InteropServices;

namespace MaisPDF
{
    [Guid("4EC03727-0D73-44F2-A812-5F4579333025")]
    [ComVisible(true)]
    public interface IDocumentPage
    {
        void НачатьОтрисовку();

        void ЗавершитьОтрисовку();

        bool НаписатьТекст(string text, double left, double top, double width, double height, string fontName, double fontSize, int fontStyle, int textAlign);
    }

    [Guid("866DEE21-909F-4ACE-91EB-50622C518D79")]
    [ComVisible(true)]
    public enum TextAlign
    {
        BaseLineLeft = 0,
        TopLeft = 1,
        CenterLeft = 2,
        BottomLeft = 3,
        BaseLineCenter = 4,
        TopCenter = 5,
        Center = 6,
        BottomCenter = 7,
        BaseLineRight = 8,
        TopRight = 9,
        CenterRight = 10,
        BottomRight = 11
    }
}

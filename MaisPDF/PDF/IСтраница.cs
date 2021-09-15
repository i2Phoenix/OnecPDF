using System;
using System.Runtime.InteropServices;

namespace MaisPDF
{
    [Guid("4EC03727-0D73-44F2-A812-5F4579333025")]
    [ComVisible(true)]
    public interface IСтраница
    {
        void НачатьОтрисовку();

        void ЗавершитьОтрисовку();

        double ШиринаСтраницы();

        double ВысотаСтраницы();
        
        void УстановитьШрифт(string fontName, double fontSize, int fontStyle);

        void УстановитьЦветCMYK(double cyan, double magenta, double yellow, double black);

        void УстановитьЦветRGB(int red, int green, int blue);

        bool Текст(string text, double left, double top, double width, double height, int textAlign);

        bool Штрихкод(string barcodeValue, double left, double top, double width, double height);

        bool QRКод(string barcodeValue, double left, double top, double scale, int version);

        bool ЗаполненныйПрямоугольник(double left, double top, double width, double height);

        bool Прямоугольник(double left, double top, double width, double height, double penWidth);

        bool Линия(double x1, double y1, double x2, double y2, double width);

        bool ЗагрузитьВекторноеИзображение(byte[] imageContent);

        bool ЗагрузитьРастовоеИзображение(byte[] imageContent);

        double ШиринаИзображения();

        double ВысотаИзображения();

        bool Изображение(double left, double top);

        bool ИзображениеВписать(double left, double top, double width, double height);

        bool ИзображениеЦентр(double left, double top, double width, double height);
    }
}

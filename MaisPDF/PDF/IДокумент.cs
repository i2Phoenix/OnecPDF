using System;
using System.Runtime.InteropServices;

namespace MaisPDF.PDF
{
    [Guid("2A44957D-A704-4370-9C4B-5D6EE147ABB6")]
    [ComVisible(true)]
    internal interface IДокумент
    {
        void ВключитьОтладку();

        void ВыключитьОтладку();

        string ПолучитьОшибку();

        int КоличествоСтраниц();

        void ЗащититьДокумент(string Password);

        bool Создать();

        bool СоздатьИзШаблона(byte[] templateContent);

        Страница НоваяСтраница(double pageHeight, double pageWidth);

        Страница ПолучитьСтраницу(int pageNumber);

        bool СохранитьФайл(string Path);

        byte[] СохранитьБайтМассив();
    }
}

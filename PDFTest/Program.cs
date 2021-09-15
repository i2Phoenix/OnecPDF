using MaisPDF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var document = new Документ();

            document.Создать();

            var page = document.НоваяСтраница(150, 150);

            page.НачатьОтрисовку();
            
            page.УстановитьЦветCMYK(0, 0, 0, 1);
            page.УстановитьШрифт("OpenSans", 6, 0);

            for (int i1 = 0; i1 < 1; i1++)
            {
                for (int i2 = 0; i2 < 5; i2++)
                {
                    var d1 = i2 > 2 ? 1.5 * (i2 - 1): 0;

                    page.QRКод("1234567890ABCD", (10 * (i2 + 1)) + d1, 10 + (10 * i1), 1, i2 + 1);
                }




                //0
                //0
                //1.5*0
                //page.QRКод("1234567890ABCD", 10, 10 + (10 * i), 1, 1); 

                //1
                //0
                //1.5*0
                //page.QRКод("1234567890ABCDFGHJKLMNPQRS", 10 + (10 * 1), 10 + (10 * i), 1, 2);

                //2
                //1.5
                //1.5*1
                //page.QRКод("1234567890ABCDFGHJKLMNPQRSTVXZ", 11.5 + (10 * 2), 10 + (10 * i), 1, 3); //1.5

                //3
                //4.5
                //1.5*3
                //page.QRКод("1234567890ABCDFGHJKLMNPQRSTVXZ", 14.5 + (10 * 3), 10 + (10 * i), 1, 4); //3

                //4
                //9
                //1.5*6
                //page.QRКод("1234567890ABCDFGHJKLMNPQRSTVXZ", 19 + (10 * 4), 10 + (10 * i), 1, 5); //4.5

                //5
                //15
                //1.5*9
                //page.QRКод("1234567890ABCDFGHJKLMNPQRSTVXZ", 25 + (10 * 5), 10 + (10 * i), 1, 6); //6





                //page.QRКод("1234567890ABCDFGHJKLMNPQRSTVXZ", 32.5 + (10 * 6), 10 + (10 * i), 1, 7); //7.5

                //page.QRКод("1234567890ABCDFGHJKLMNPQRSTVXZ", 41.5 + (10 * 7), 10 + (10 * i), 1, 8); //9 //1.5











                //page.QRКод("1234567890ABCD", 10, 10 + (10 * i), 1, 1);

                //page.QRКод("1234567890ABCDFGHJKLMNPQRS", 10 + (12 * 1), 10 + (10 * i), 0.85, 2);

                //page.QRКод("1234567890ABCDFGHJKLMNPQRSTVXZ", 10 + (12 * 2), 10 + (10 * i), 0.75, 3);

                //page.QRКод("1234567890ABCDFGHJKLMNPQRSTVXZ", 10 + (12 * 3), 10 + (10 * i), 0.65, 4);

                //page.QRКод("1234567890ABCDFGHJKLMNPQRSTVXZ", 10 + (12 * 4), 10 + (10 * i), 0.6, 5);

                //page.QRКод("1234567890ABCDFGHJKLMNPQRSTVXZ", 10 + (12 * 5), 10 + (10 * i), 0.55, 6);

                //page.QRКод("1234567890ABCDFGHJKLMNPQRSTVXZ", 10 + (12 * 6), 10 + (10 * i), 0.5, 7);

                //page.QRКод("1234567890ABCDFGHJKLMNPQRSTVXZ", 10 + (12 * 7), 10 + (10 * i), 0.45, 8);

                //page.QRКод("1234567890ABCDFGHJKLMNPQRSTVXZ", 10 + (12 * 8), 10 + (10 * i), 0.42, 9);

                //page.QRКод("1234567890ABCDFGHJKLMNPQRSTVXZ", 10 + (12 * 9), 10 + (10 * i), 0.39, 10);
            }

            page.ЗавершитьОтрисовку();

            var tempFile = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";

            document.СохранитьФайл(tempFile);
            Process.Start(tempFile);

            Console.ReadKey();
        }
    }
}

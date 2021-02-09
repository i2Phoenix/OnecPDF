using MaisPDF.PDF;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Security;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MaisPDF
{
    [Guid("E60248B4-F573-4EAE-93E9-5E77341D05BA")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Документ : IДокумент
    {
        private string _errorMessage;
        private PdfDocument _document;

        private bool _isDebugDraw;
        private bool _iSecured;
        private string _password;

        public Документ()
        {
            _isDebugDraw = false;
            _iSecured = false;
            _password = string.Empty;
        }

        internal bool DebugDraw => _isDebugDraw;

        internal string ErrorMessage  { get => _errorMessage; set => _errorMessage = value; }

        public void ВключитьОтладку()
        {
            _isDebugDraw = true;
        }

        public void ВыключитьОтладку()
        {
            _isDebugDraw = false;
        }

        public string ПолучитьОшибку()
        {
            return _errorMessage;
        }

        public int КоличествоСтраниц()
        {
            try
            {
                if (_document != null)
                    return _document.PageCount;

                return 0;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                return 0;
            }
        }

        public void ЗащититьДокумент(string Password)
        {
            _iSecured = true;
            _password = Password;
        }

        private void InitDocument()
        {
            _document.Info.Subject = string.Empty;
            _document.Info.Keywords = string.Empty;

            _document.Info.Author = "Maytoni GmbH";
            _document.Info.Creator = "Adobe Illustrator";

            _document.Options.ColorMode = PdfColorMode.Cmyk;
            _document.Version = 15;

            if (_iSecured)
            {
                PdfSecuritySettings securitySettings = _document.SecuritySettings;
                securitySettings.OwnerPassword = _password;
                securitySettings.PermitAccessibilityExtractContent = false;
                securitySettings.PermitAnnotations = false;
                securitySettings.PermitAssembleDocument = false;
                securitySettings.PermitExtractContent = false;
                securitySettings.PermitFormsFill = false;
                securitySettings.PermitModifyDocument = false;
                securitySettings.PermitFullQualityPrint = true;
                securitySettings.PermitPrint = true;
            }
        }

        public bool Создать()
        {
            try
            {
                _document = new PdfDocument();

                InitDocument();

                return true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                return false;
            }
        }

        public bool СоздатьИзШаблона(byte[] templateContent)
        {
            try
            {
                using (MemoryStream loadStream = new MemoryStream(templateContent, 0, templateContent.Length, false, true))
                {
                    _document =  PdfReader.Open(loadStream);
                }

                InitDocument();

                return true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                return false;
            }
        }

        public Страница НоваяСтраница(double pageWidth, double pageHeight)
        {
            try
            {
                PdfPage Page = _document.AddPage();
                Page.Width = new XUnit(pageWidth, XGraphicsUnit.Millimeter);
                Page.Height = new XUnit(pageHeight, XGraphicsUnit.Millimeter);

                return new Страница(this, Page);
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                return null;
            }
        }

        public Страница ПолучитьСтраницу(int pageNumber)
        {
            try
            {
                PdfPage Page = _document.Pages[pageNumber];
                return new Страница(this, Page);
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                return null;
            }
        }

        public bool ДобавитьСтраницуИзШаблона(byte[] templateContent)
        {
            throw new NotImplementedException();
        }

        public bool СохранитьФайл(string Path)
        {
            try
            {
                _document.Save(Path);
                return true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                return false;
            }
        }

        public byte[] СохранитьБайтМассив()
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    _document.Save(memoryStream, false);

                    byte[] buffer = memoryStream.ToArray();

                    return buffer;
                }
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                return null;
            }
        }
    }
}

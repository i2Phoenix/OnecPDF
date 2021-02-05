using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Security;
using System;
using System.Runtime.InteropServices;

namespace MaisPDF
{
    [Guid("2A44957D-A704-4370-9C4B-5D6EE147ABB6")]
    [ComVisible(true)]
    internal interface IDocument
    {
        #region Свойства

        bool Отладка { get; set; }

        string Ошибка { get; set; }

        #endregion

        #region Методы

        void ЗащититьДокумент(string Password);

        bool СоздатьПустойДокумент();

        bool СоздатьДокументИзШаблона(byte[] templateContent);

        DocumentPage СоздатьСтраницу(double pageHeight, double pageWidth);

        bool СохранитьДокумент(string Path);

        #endregion
    }

    [Guid("E60248B4-F573-4EAE-93E9-5E77341D05BA")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Document : IDocument
    {
        private string _errorMessage;
        private PdfDocument _document;

        private bool _isDebugDraw;
        private bool _iSecured;
        private string _password;

        public Document()
        {
            _isDebugDraw = false;
            _iSecured = false;
            _password = string.Empty;
        }

        #region Свойства

        public bool Отладка
        {
            get => _isDebugDraw;
            set => _isDebugDraw = value;
        }

        public string Ошибка
        {
            get => _errorMessage;
            set => _errorMessage = value;
        }

        #endregion

        #region Методы

        public void ЗащититьДокумент(string Password)
        {
            _iSecured = true;
            _password = Password;
        }

        public bool СоздатьПустойДокумент()
        {
            try
            {
                _document = new PdfDocument();

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

                return true;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                return false;
            }
        }

        public bool СоздатьДокументИзШаблона(byte[] templateContent)
        {
            throw new NotImplementedException();
        }

        public DocumentPage СоздатьСтраницу(double pageWidth, double pageHeight)
        {
            try
            {
                PdfPage Page = _document.AddPage();
                Page.Width = new XUnit(pageWidth, XGraphicsUnit.Millimeter);
                Page.Height = new XUnit(pageHeight, XGraphicsUnit.Millimeter);

                return new DocumentPage(this, Page);
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
        public bool СохранитьДокумент(string Path)
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

        #endregion

    }
}

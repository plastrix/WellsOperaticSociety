using System;
using System.IO;
namespace WellsOperaticSociety.PDFService
{
    public class PdfWriter
    {
        public MemoryStream GenertatePdfFromHtml(string html)
        {
            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            var pdfBytes = htmlToPdf.GeneratePdf(html);
            return new MemoryStream(pdfBytes);
        }
    }
}

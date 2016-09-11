using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WellsOperaticSociety.PDFService
{
    public class PdfWriter
    {
        public MemoryStream GenertatePdfFromHtml(string html)
        {
            MemoryStream ms;
            using (var documentMemoryStream = new MemoryStream())
            {
                using (var doc = new Document())
                {
                    using (var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, documentMemoryStream))
                    {
                        doc.Open();

                        using (var sr = new StringReader(html))
                        {
                            iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);
                        }

                        doc.Close();
                    }
                }
                ms = new MemoryStream(documentMemoryStream.ToArray());
            }
            return ms;
        }
    }
}

using iText.Html2pdf;
using iText.Kernel.Pdf;
using System.Text;
using iText.Kernel.Geom;

namespace back_courrier.Utils
{
    public class Helper
    {
        public static int CalculateTotalPage<T>(List<T> liste, int pageSize)
        {
            int totalRecords = liste.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            return totalPages;
        }

        /*public static IQueryable<T> Paginate<T>(IQueryable<T> query, int pageNumber, int pageSize)
        {
            int skipAmount = (pageNumber - 1) * pageSize;
            var paginatedQuery = query.Skip(skipAmount).Take(pageSize);
            return paginatedQuery;
        }*/

        public static byte[][] ExportPdfHtml(string GridHtml)
        {
            using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(GridHtml)))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(outputStream);
                    PdfDocument pdfDocument = new PdfDocument(writer);
                    /*pdfDocument.SetDefaultPageSize(PageSize.A4);*/
                    pdfDocument.SetDefaultPageSize(PageSize.A4.Rotate());
                    HtmlConverter.ConvertToPdf(stream, pdfDocument);
                    pdfDocument.Close();
                    return new byte[][] { outputStream.ToArray() };
                }
            }
        }
    }
}

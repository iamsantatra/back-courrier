using System.Collections.Generic;

namespace back_courrier.Helper
{
    public class Helper
    {
        public static int CalculateTotalPage<T>(List<T> liste, int pageSize)
        {
            int totalRecords = liste.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            return totalPages;
        }
    }
}

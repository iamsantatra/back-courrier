namespace back_courrier.Models
{
    public class ListeCourrier
    {
        public List<VueListeCourrier> ListeCourrier { get; set; }
        public int CurrentPageIndex { get; set; }
        public int PageCount { get; set; }

    }
}

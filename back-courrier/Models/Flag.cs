using System.ComponentModel.DataAnnotations;

namespace back_courrier.Models
{
    public class Flag : BaseModel
    {
        [Required]
        public string Designation { get; set; }
    }
}

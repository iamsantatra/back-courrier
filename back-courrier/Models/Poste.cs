using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_courrier.Models
{
    public class Poste : BaseModel
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Designation { get; set; }
    }
}

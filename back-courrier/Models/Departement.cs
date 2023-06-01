using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back_courrier.Models
{
    public class Departement : BaseModel
    {
        [Required]
        public string Designation { get; set; }
    }
}

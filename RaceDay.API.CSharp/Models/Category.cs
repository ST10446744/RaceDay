using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.CSharp.Models
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

    [Required]
        public int EventID { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public int? AgeMinimum { get; set; }

        public int? AgeMaximum { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CategoryFee { get; set; }

        [ForeignKey(nameof(EventID))]
        public Event? Event { get; set; }
    }


}

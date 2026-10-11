using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.CSharp.Models
{
    [Table("Payments")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentID { get; set; }


    [Required]
        public int EnrolmentID { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public DateTime? PaymentDate { get; set; }

        [MaxLength(30)]
        public string? PaymentMethod { get; set; }

        [MaxLength(100)]
        public string? TransactionReference { get; set; }

        [Required]
        [MaxLength(20)]
        public string PaymentStatus { get; set; } = "Pending";

        [ForeignKey(nameof(EnrolmentID))]
        public Enrolment? Enrolment { get; set; }
    }

}

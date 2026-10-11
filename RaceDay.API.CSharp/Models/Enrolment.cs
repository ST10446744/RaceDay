using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.CSharp.Models
{
    [Table("Enrolments")]
    public class Enrolment
    {
        [Key]
        public int EnrolmentID { get; set; }

        [Required]
        public int ParticipantID { get; set; }

        [Required]
        public int CategoryID { get; set; }

        public DateTime EnrolmentDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(100)]
        public string EmergencyContactName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string EmergencyContactPhone { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string PaymentStatus { get; set; } = "Pending";

        [Required]
        [MaxLength(20)]
        public string EnrolmentStatus { get; set; } = "Active";

        [ForeignKey(nameof(ParticipantID))]
        public User? Participant { get; set; }

        [ForeignKey(nameof(CategoryID))]
        public Category? Category { get; set; }
    }
}

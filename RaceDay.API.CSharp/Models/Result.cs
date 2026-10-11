using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.CSharp.Models
{
    [Table("Results")]
    public class Result
    {
        [Key]
        public int ResultID { get; set; }

        [Required]
        public int EnrolmentID { get; set; }

        public int? FinishPosition { get; set; }

        public TimeSpan? FinishTime { get; set; }

        [Required]
        [MaxLength(20)]
        public string ResultStatus { get; set; } = "Finished";

        public DateTime RecordedDate { get; set; } = DateTime.Now;

        [ForeignKey(nameof(EnrolmentID))]
        public Enrolment? Enrolment { get; set; }
    }
}
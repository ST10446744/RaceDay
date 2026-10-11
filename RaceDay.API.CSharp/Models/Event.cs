using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.CSharp.Models
{
    [Table("Events")]
    public class Event
    {
        [Key]
        public int EventID { get; set; }


    [Required]
        public int OrganiserID { get; set; }

        [Required]
        [MaxLength(150)]
        public string EventName { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(30)]
        public string EventType { get; set; } = "Running";

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        [MaxLength(200)]
        public string Venue { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Province { get; set; } = string.Empty;

        [Column(TypeName = "decimal(6,2)")]
        public decimal DistanceKM { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal EntryFee { get; set; }

        public int? MaximumParticipants { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Upcoming";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey(nameof(OrganiserID))]
        public User? Organiser { get; set; }

        public ICollection<Category> Categories { get; set; }
            = new List<Category>();
    }


}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.CSharp.Models
{
    [Table("WeatherInformation")]
    public class WeatherInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WeatherID { get; set; }


    [Required]
        public int EventID { get; set; }

        [Column(TypeName = "date")]
        public DateTime ForecastDate { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? TemperatureC { get; set; }

        [MaxLength(100)]
        public string? Condition { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal? WindSpeedKmh { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? RainProbability { get; set; }

        public DateTime RetrievedDate { get; set; } = DateTime.Now;

        [ForeignKey(nameof(EventID))]
        public Event? Event { get; set; }
    }


}


using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.CSharp.Models
{
    [Table("EventDocuments")]
    public class EventDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentID { get; set; }

    [Required]
        public int EventID { get; set; }

        [Required]
        [MaxLength(200)]
        public string DocumentName { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string BlobFileName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? DocumentType { get; set; }

        public DateTime UploadedDate { get; set; } = DateTime.Now;

        [ForeignKey(nameof(EventID))]
        public Event? Event { get; set; }
    }

}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musicalog.Common.Data.Models
{
    [Table("artists")]
    public class Artist
    { 
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("full_name")]
        public string Name { get; set; }
    }
}

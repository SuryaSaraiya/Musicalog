using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musicalog.Common.Data.Models
{
    [Table("album_types")]
    public class AlbumType
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("Type")]
        public string Type { get; set; }
    }

}

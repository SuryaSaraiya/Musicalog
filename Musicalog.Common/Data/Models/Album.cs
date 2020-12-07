using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musicalog.Common.Data.Models
{
    [Table("albums")]
    public class Album
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("album_name")]
        public string AlbumName { get; set; }

        [Required]
        [Column("sku")]
        public string SKU { get; set; }

        [Required]
        [Column("type_id")]
        public int TypeId { get; set; }
    }
}

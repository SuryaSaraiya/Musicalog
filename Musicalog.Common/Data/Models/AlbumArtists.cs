using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musicalog.Common.Data.Models
{
    [Table("album_artists")]
    public class AlbumArtists
    { 
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("album_id")]
        public int AlbumId { get; set; }

        [Required]
        [Column("artist_id")]
        public int ArtistId { get; set; }
    }
}

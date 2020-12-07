using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Musicalog.Common.Data.Models
{
    [Table("inventory")]
    public class Inventory
    { 
        [Key]
        [Column("id")]
        public int Id { get; set; }


        [Required]
        [Column("sku")]
        public string SKU { get; set; }

        [Required]
        [Column("purchased")]
        public int StockPurchased { get; set; }

        [Required]
        [Column("sold")]
        public int StockSoldSoFar { get; set; }

        [Required]
        [Column("stock")]
        public int Stock { get; set; }
    }
}

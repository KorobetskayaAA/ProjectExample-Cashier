using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashierDB.Model.DTO
{
    [Table("Product")]
    public class ProductDbDto
    {
        [Key]
        [Column(TypeName = "char(13)")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Barcode { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

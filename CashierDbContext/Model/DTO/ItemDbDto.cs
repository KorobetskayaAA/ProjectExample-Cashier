using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CashierDB.Model.DTO
{
    [Table("Item")]
    public class ItemDbDto
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [Column(TypeName = "char(13)")]
        public string Barcode { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double Amount { get; set; } = 1;

        [Required]
        [Column("Bill")]
        public uint BillNumber { get; set; }
        [ForeignKey("BillNumber")]
        public BillDbDto Bill { get; set; }
        
        [ForeignKey("Barcode")]
        public ProductDbDto Product { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CashierDB.Model.DTO
{
    [Table("Bill")]
    public class BillDbDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Number { get; set; }
        public DateTime Created { get; set; }

        public ICollection<ItemDbDto> Items { get; set; }

        [Column("Status")]
        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        public BillStatusDbDto Status { get; set; }
    }
}

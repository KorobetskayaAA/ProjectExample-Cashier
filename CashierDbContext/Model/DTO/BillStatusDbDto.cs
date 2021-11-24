using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CashierDB.Model.DTO
{
    [Table("BillStatus")]
    public class BillStatusDbDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
    }
}

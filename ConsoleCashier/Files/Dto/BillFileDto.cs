using CashierModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ConsoleCashier
{
    [XmlType(TypeName = "Bill")]
    public class BillFileDto
    {
        [XmlAttribute("number")]
        public uint Number { get; set; }
        [XmlAttribute("created")]
        public DateTime Created { get; set; }
        public int Status { get; set; }
        public ItemFileDto[] Items { get; set; }


        public static BillFileDto Map(Bill bill)
        {
            return new BillFileDto()
            {
                Number = bill.Number,
                Created = bill.Created,
                Status = (int)bill.Status,
                Items = bill.Items.Select(item => ItemFileDto.Map(item)).ToArray()
            };
        }

        public static Bill Map(BillFileDto bill)
        {
            return new Bill(
                bill.Number,
                bill.Created,
                (BillStatus)bill.Status,
                bill.Items.Select(item => ItemFileDto.Map(item))
            );
        }
    }
}

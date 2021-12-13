using CashierDB.Model.DTO;

namespace CashierWebApi.BL.Model
{
    public class BillStatusApiDto
    {
        public int StatusId { get; set; }
        public string Name { get; set; }

        public BillStatusApiDto() { }
        public BillStatusApiDto(BillStatusDbDto status)
        {
            StatusId = status.Id;
            Name = status.Name;
        }
    }
}

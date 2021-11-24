using CashierDB.Model.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Linq;

namespace CashierDB.Model.Configure
{
    class BillStatusConfiguration : IEntityTypeConfiguration<BillStatusDbDto>
    {
        readonly Dictionary<int, string> existingStatuses;

        public BillStatusConfiguration(Dictionary<int, string> existingStatuses)
        {
            this.existingStatuses = existingStatuses ?? new Dictionary<int, string>();
        }

        public BillStatusConfiguration() : this(
            new Dictionary<int, string>()
            {
                { 0, "Открыт" },
                { 1, "Закрыт" },
                { 2, "Отмена" },
            }
        ) { }

        public void Configure(EntityTypeBuilder<BillStatusDbDto> builder)
        {
            builder.HasData(
                existingStatuses.Select(es =>
                    new BillStatusDbDto() { Id = es.Key, Name = es.Value }
                )
            );
        }
    }
}

using System;

namespace TDS.Domain.Models
{
    public class AccountDto
    {
        public Guid Id { get; set; }

        public string Address { get; set; }
        public DateTime? LastDiscoveredAt { get; set; }
        public string Cursor { get; internal set; }
    }
}

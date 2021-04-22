using System;

namespace TDS.Infrastructure.Models
{
    public class Account : EntitytBase<Guid>
    {
        public string Address { get; set; }
        public DateTime? LastDiscoveredAt { get; set; }
        public string Cursor { get; internal set; }
    }
}

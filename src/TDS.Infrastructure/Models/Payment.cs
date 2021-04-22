using System;

namespace TDS.Infrastructure.Models
{
    public class Payment : EntitytBase<Guid>
    {
        public string PaymentId { get; set; }
        public string PagingToken { get; set; }
        public bool TransactionSuccessful { get; set; }
        public string SourceAccount { get; set; }
        public string TransactionHash { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public double Amount { get; set; }
        public DateTime ActualCreatedAt { get; set; }
        public string AssetCode { get; set; }

    }
}

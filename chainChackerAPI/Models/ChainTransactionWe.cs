namespace chainChackerAPI.Models
{
    public enum TransactionType
    {
        Normal,
        Suspicious,
        Dangerous
    }
    public class ChainTransactionWe
    {
        public string Hash { get; set; }
        public string TrasactionType { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Value { get; set; }
        public TransactionType TranType { get; set; }

        public ChainTransactionWe() 
        {
            TranType = TransactionType.Normal;
        }
        public ChainTransactionWe(TransactionType type) 
        {
            TranType = type;
        }
    }
}

using System;
using Application;

namespace chainChackerAPI.Entities
{
    public enum TransactionStatus
    {
        None, Normal, Suspicious, Danger, Error
    }
	public class ChainTransactionWE
	{
        public string hash { get; set; }
        public string trasactionType { get; set; }
        public DateTime date { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string value { get; set; }
        public TransactionStatus status { get; set; }



        public ChainTransactionWE()
		{

		}
        public ChainTransactionWE(ChainTransaction entity, TransactionStatus status)
        {
            hash = entity.hash;
            trasactionType = entity.trasactionType;
            date = entity.date;
            from = entity.from;
            to = entity.to;
            value = entity.value;
            this.status = status;

        }
    }
}


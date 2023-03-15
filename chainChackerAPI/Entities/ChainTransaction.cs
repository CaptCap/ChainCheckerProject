using System;
namespace Application
{
    public class ChainTransaction
    {
        public string hash { get; set; }
        public string trasactionType { get; set; }
        public DateTime date { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string value { get; set; }



        public ChainTransaction()
        {
        }
    }
}


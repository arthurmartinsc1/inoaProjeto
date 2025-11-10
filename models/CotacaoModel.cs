namespace inoaProjectx.Models
{
    public class CotacaoModel
    {
        public string Symbol { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }

            public CotacaoModel(string symbol, double regularMarketPrice){
            Symbol = symbol;
            this.Price = regularMarketPrice;
            Date = DateTime.Now;

        }
        public override string ToString()
        {
            return $"Symbol: {Symbol}, Price: {Price}, Date: {Date}";
        }
    }
}
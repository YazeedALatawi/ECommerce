namespace ECommerce.ViewModel
{
    public class ProductCart
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double price { get; set; }
        public string image { get; set; }
        public int Quantity { get; set; }
        public double allprice { get; set; }
        public ProductViewModel? ProductViewModel { get; set; }

    }
}

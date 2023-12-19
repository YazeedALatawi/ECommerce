namespace ECommerce.ViewModel
{
    public class checkOutViewModelForFirst
    {
        public int ProductID { get; set; }
        public List<mainOption> allOptions { get; set; }
    }
    public class mainOption
    {
        public int MainOptionID { get; set; }
        public string MainOptionName { get; set; }
        public List<SubOptions> subOptions { get; set; }
    }
    public class SubOptions
    {
        public int SubOptionID { get; set; }
        public string SubOptionName { get; set; }
    }
}

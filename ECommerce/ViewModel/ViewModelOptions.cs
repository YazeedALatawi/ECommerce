namespace ECommerce.ViewModel
{
    public class ViewModelOptions
    {
    }

    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string SubOption { get; set; }
        public int count { get; set; }
        public List<MainOptionViewModel> MainOptions { get; set; }
        public List<MainOptionViewModel> ExistingOptions { get; set; }
        public List<ProductOptionViewModel> SelectedOptions { get; set; }

    }

    public class MainOptionViewModel
    {
        public int MainOptionId { get; set; } // معرّف الخيار الرئيسي
        public string MainOptionName { get; set; }
        public List<SubOptionViewModel> SubOptions { get; set; }
    }

    public class SubOptionViewModel
    {
        public int SubOptionId { get; set; } // معرّف الخيار الفرعي
        public string SubOptionName { get; set; }
        public int SubOptionCount { get; set; }
        public int MainOptionId { get; set; } // معرّف الخيار الرئيسي
    }

    public class ProductOptionViewModel
    {
        public int OptionId { get; set; }
        public int SubOptionId { get; set; }
    }

    public class CartOptionFinish
    {
        public List<MainOptionViewModel> ThaAllOption { get; set; }
        public List<ProductOptionViewModel> SelectedOptions { get; set; }
    }


}

using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class CartPrdouctOptions
    {
        public int Id { get; set; }
        public int MainOptionID { get; set; }
        public string MainOptionName { get; set; }
        public int SubOptionID { get; set; }
        public string SubOptionName { get; set; }
        public int ProductID { get; set; }
        public int cartID { get; set; }
        public int CartProductsID { get; set; }
        [ForeignKey("CartProductsID")]
        public CartProducts cartProducts { get; set; }
    }
}

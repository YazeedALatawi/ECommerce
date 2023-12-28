using ECommerce.Models.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Models.Service
{
    public class LayoutServices
    {
        private readonly IOperations<Category> _category;
        private readonly IOperations<Cart> _cart;
        private readonly IOperations<CartProducts> _cartProducts;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor accessor;
        public LayoutServices(IOperations<Category> category, IOperations<Cart> cart, IOperations<CartProducts> cartProducts, UserManager<User> userManager, IHttpContextAccessor accessor)
        {
           _category = category;
            _cart = cart;
            _cartProducts = cartProducts;
            _userManager = userManager;
            this.accessor = accessor;
        }

        public List<Category> getCategory()
        {
            return (List<Category>)_category.List();
        }

        public int getCountProductInCart()
        {
            var user = _userManager.GetUserId(accessor.HttpContext.User);
            if(!user.IsNullOrEmpty())
            {
                var theCart = _cart.findByIdUser(user);
                if(theCart != null)
                {
                    var allProduct = _cartProducts.findAllByCartId(theCart.Id);
                    return allProduct.Count;
                }
                else
                {
                    return 0;
                }


            }
            else
            {
                return 0;
            }

        }
    }
}

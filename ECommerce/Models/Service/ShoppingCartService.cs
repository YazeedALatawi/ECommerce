using ECommerce.ViewModel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ECommerce.Models.Service
{
    public class ShoppingCartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ShoppingCartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ShoppingCart GetShoppingCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartJson = session.GetString("ShoppingCart");
            if (cartJson != null)
            {
                return JsonConvert.DeserializeObject<ShoppingCart>(cartJson);
            }

            return new ShoppingCart();
        }

        public void AddToCart(ProductCart product)
        {
            var shoppingCart = GetShoppingCart();
            shoppingCart.Products.Add(product);

            var session = _httpContextAccessor.HttpContext.Session;
            var cartJson = JsonConvert.SerializeObject(shoppingCart);
            session.SetString("ShoppingCart", cartJson);
        }

        public void RemoveProduct(int productId)
        {
            var shoppingCart = GetShoppingCart();
            var productToRemove = shoppingCart.Products.FirstOrDefault(p => p.Id == productId);

            if (productToRemove != null)
            {
                shoppingCart.Products.Remove(productToRemove);
                SaveShoppingCart(shoppingCart);
            }
        }

        public void SaveShoppingCart(ShoppingCart shoppingCart)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cartJson = JsonConvert.SerializeObject(shoppingCart);
            session.SetString("ShoppingCart", cartJson);
        }

        public void UpdateQuantity(int productId, int newQuantity)
        {
            var shoppingCart = GetShoppingCart();
            var cartItem = shoppingCart.Products.FirstOrDefault(item => item.Id == productId);

            if (cartItem != null)
            {
                cartItem.Quantity = newQuantity;
                SaveShoppingCart(shoppingCart);
            }
        }

        public int GetCartItemCount()
        {
            var shoppingCartList = GetShoppingCart();
            int count = shoppingCartList.Products.Count;
            return count;
        }

        public void ClearSession()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.Clear();
        }
    }
}

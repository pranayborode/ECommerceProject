using ECommerceProject.Models;
using ECommerceProject.Repositories;

namespace ECommerceProject.Services
{
	public class CartService : ICartService
	{
		private readonly ICartRepository repo;

        public CartService(ICartRepository repo)
        {
			this.repo = repo;
        }

		public Cart GetCartByUserId(string userId)
		{
			return repo.GetCartByUserId(userId);
		}
		public int AddToCart(CartItem cartItem)
		{
            var userCart = repo.GetCartByUserId(cartItem.UserId);
            if (userCart == null)
            {
                // Create a new cart for the user
                var newCart = new Cart
                {
                    UserId = cartItem.UserId,
                    CreatedDate = DateTime.Now
                };
                repo.CreateCart(newCart);
                userCart = newCart;
            }
            cartItem.CartId = userCart.CartId;
            return repo.AddToCart(cartItem);
        }

		public int RemoveFromCart(int cartItemId)
		{
			return repo.RemoveFromCart(cartItemId);
		}

		public int ClearCart(string userId)
		{
			return repo.ClearCart(userId);
		}

		public int UpdateCartItem(CartItem cartItem)
		{
			return repo.UpdateCartItem(cartItem);
		}

		public void UpdateCartItemQuantity(int cartItemId, int quantity)
		{
			var cartItem = GetCartItemById(cartItemId);
			if (cartItem != null)
			{
				cartItem.Quantity = quantity;
				repo.UpdateCartItem(cartItem);
			}
		}

		public CartItem GetCartItemById(int cartItemId)
		{
			return repo.GetCartItemById(cartItemId);
		}
	}
}

using ECommerceProject.Models;

namespace ECommerceProject.Services
{
	public interface ICartService
	{
		Cart GetCartByUserId(string userId);
		int AddToCart(CartItem cartItem);
		int RemoveFromCart(int cartItemId);
		int ClearCart(string userId);
		int UpdateCartItem(CartItem cartItem);
		public void UpdateCartItemQuantity(int cartItemId, int quantity);
		CartItem GetCartItemById(int cartItemId);
	}
}

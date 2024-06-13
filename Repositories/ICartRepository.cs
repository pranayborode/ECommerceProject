using ECommerceProject.Models;

namespace ECommerceProject.Repositories
{
	public interface ICartRepository
	{
		Cart GetCartByUserId(string userId);
		int AddToCart(CartItem cartItem);
		int RemoveFromCart(int cartItemId);
		int ClearCart(string userId);
		int UpdateCartItem(CartItem cartItem);

        void CreateCart(Cart cart);
		CartItem GetCartItemById(int cartItemId);

	}
}

using ECommerceProject.Data;
using ECommerceProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceProject.Repositories
{
	public class CartRepository : ICartRepository
	{
		private readonly ApplicationDbContext _context;

		public CartRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public Cart GetCartByUserId(string userId)
		{
			return _context.Carts
				.Include(c => c.CartItems)
				.ThenInclude(ci => ci.Product)
				.FirstOrDefault(c => c.UserId == userId);
		}

		public int AddToCart(CartItem cartItem)
		{
			var cart = _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.CartId == cartItem.CartId);

			if (cart != null)
			{
				var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItem.ProductId && ci.Size == cartItem.Size);

				if (existingCartItem != null)
				{
					// Increase the quantity of the existing item
					existingCartItem.Quantity += cartItem.Quantity;
					_context.CartItems.Update(existingCartItem);
				}
				else
				{
					// Add new item to the cart
					_context.CartItems.Add(cartItem);
				}
				_context.SaveChanges();
				UpdateCartTotal(cart.CartId);
			}
			return _context.SaveChanges();
		}

		public int RemoveFromCart(int cartItemId)
		{
			var cartItem = _context.CartItems.Find(cartItemId);

			if(cartItem != null)
			{
				_context.CartItems.Remove(cartItem);
				UpdateCartTotal(cartItem.CartId);
				return _context.SaveChanges();
			}
			return 0;
		}

		public int ClearCart(string userId)
		{
			var cart = GetCartByUserId(userId);

            if (cart != null)
            {
				_context.CartItems.RemoveRange(cart.CartItems);
				UpdateCartTotal(cart.CartId);
				return _context.SaveChanges();
			}
			return 0;
		}

		public int UpdateCartItem(CartItem cartItem)
		{
			_context.CartItems.Update(cartItem);
			_context.SaveChanges();
			UpdateCartTotal(cartItem.CartId);
			return _context.SaveChanges();
		}

        public void CreateCart(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

		private void UpdateCartTotal(int cartId)
		{
			var cart = _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.CartId == cartId);
			if (cart != null)
			{
				cart.TotalAmount = cart.CartItems.Sum(item => item.Quantity * item.DiscountedPrice);
				_context.Carts.Update(cart);
				_context.SaveChanges();
			}
		}

		public CartItem GetCartItemById(int cartItemId)
		{
			return _context.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);
		}
	}
}

using ECommerceProject.Data;
using ECommerceProject.Models;

namespace ECommerceProject.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;

        public AddressRepository(ApplicationDbContext _context)
        {
           this._context = _context;
        }

        public int CreateAddress(Address address)
        {
           
            _context.Addresses.Add(address);
            int result = _context.SaveChanges();
            return result;
        }

        public int DeleteAddress(int addressId)
        {
            var address = _context.Addresses.Find(addressId);
            if(address != null)
            {
                _context.Addresses.Remove(address);
                int result = _context.SaveChanges();
                return result;
            }

            return 0;
        }

        public Address GetAddressById(int addressId)
        {
            var address= _context.Addresses.Find(addressId);

            if(address != null)
            {
                return address;
            }
            else
            {
                return null;
            }
        }

        public List<Address> GetAddressesByUserId(string userId)
        {
            return _context.Addresses.Where(a=>a.UserId == userId).ToList();
        }

        public int UpdateAddress(Address address)
        {
            _context.Addresses.Update(address);
            int result = _context.SaveChanges();
            return result;
        }
    }
}

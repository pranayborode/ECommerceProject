using ECommerceProject.Models;
using ECommerceProject.Repositories;

namespace ECommerceProject.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository repo;

        public AddressService(IAddressRepository repo)
        {
            this.repo = repo;   
        }
        public int CreateAddress(Address address)
        {
            return repo.CreateAddress(address);
        }

        public int DeleteAddress(int addressId)
        {
            return repo.DeleteAddress(addressId);
        }

        public Address GetAddressById(int addressId)
        {
          return repo.GetAddressById(addressId);
        }

        public List<Address> GetAddressesByUserId(string userId)
        {
           return repo.GetAddressesByUserId(userId);
        }

        public int UpdateAddress(Address address)
        {
            return repo.UpdateAddress(address);
        }
    }
}

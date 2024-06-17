using ECommerceProject.Models;

namespace ECommerceProject.Repositories
{
    public interface IAddressRepository
    {
        List<Address> GetAddressesByUserId(string userId);
        Address GetAddressById(int addressId);
        int  CreateAddress(Address address);
        int UpdateAddress(Address address);
        int DeleteAddress(int addressId);
    }
}

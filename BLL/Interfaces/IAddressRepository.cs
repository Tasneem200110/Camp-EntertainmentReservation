using DAL.Entities;

namespace BLL.Interfaces;

public interface IAddressRepository
{
    Task<Address> GetById(int id);
    Task<IEnumerable<Address>> GetAll();
    Task<Address> GetByAddressByGovernmentCityDistrict(Address address);
    bool Add(Address address);
    bool Save();
}
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAddressRepository
    {
        Task<Address> GetById(int id);
        Task<IEnumerable<Address>> GetAll();
        Task<Address> GetByAddressByGovernmentCityDistrict(Address address);
        bool Add(Address address);
        bool Save();
    }
}

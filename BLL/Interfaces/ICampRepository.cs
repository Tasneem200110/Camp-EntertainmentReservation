using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Enum;

namespace BLL.Interfaces
{
    public interface ICampRepository
    {
        Task<IEnumerable<Camp>> GetAll();
        Task<Camp> GetById(int id);
        Task<Camp> GetByIdNoTracking(int id);
        Task<IEnumerable<Camp>> GetCampByGovernment(string government);
        Task<IEnumerable<Camp>> GetCampByCity(string city);
        Task<IEnumerable<Camp>> GetCampByDistrict(string district);
        Task<IEnumerable<Camp>> GetCampByCategory(CampCategory category);

        bool Add(Camp camp);
        bool Delete(Camp camp);
        bool Update(Camp camp);
        bool Save();
    }
}

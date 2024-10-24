using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IImageRepository
    {
        Task <IEnumerable<Image>> GetAllByCampId(int campId);
        Task<Image> GetCampFirstImage(int campId);
        Task<Image> GetByUserId(int userId);
        Task<Image> GetById(int id);
        bool DeleteByCampId(int campId);
        bool DeleteByUserId(int userId);
        bool Add(Image image);
        bool AddRange(IEnumerable<Image> images);
        bool Delete(Image image);
        Task DeleteById(int id);
        bool Update(Image image);
        bool Save();
    }
}

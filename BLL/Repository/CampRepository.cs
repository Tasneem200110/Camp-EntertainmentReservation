using BLL.Interfaces;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repository
{
    public class CampRepository : ICampRepository
    {
        public MvcAppDbContext _context {  get; set; }
        public CampRepository(MvcAppDbContext context)
        {
            _context = context;
        }

        public bool Add(Camp camp)
        {
            _context.Add(camp);
            return Save();
        }

        public bool Delete(Camp camp)
        {
            _context.Remove(camp);
            return Save();
        }

        public async Task<IEnumerable<Camp>> GetAll()
        {
            return await _context.Camps.ToListAsync();  
        }

        public async Task<Camp> GetById(int id)
        {
            return await _context.Camps.Include(a => a.Address).FirstOrDefaultAsync(c => c.CampID == id);
        }

        public async Task<IEnumerable<Camp>> GetCampByDistrict(string district)
        {
            return await _context.Camps.Where(c => c.Address.District.Contains(district)).ToListAsync();
        }

        public async Task<IEnumerable<Camp>> GetCampByGovernment(string government)
        {
            return await _context.Camps.Where(c => c.Address.Government.Contains(government)).ToListAsync();
        }

        public async Task<IEnumerable<Camp>> GetClubByCity(string city)
        {
            return await _context.Camps.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Camp camp)
        {
            _context.Update(camp);
            return Save();
        }
    }
}

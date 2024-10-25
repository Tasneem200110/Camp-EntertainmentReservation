﻿using BLL.Interfaces;
using DAL.Context;
using DAL.Data.Enum;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BLL.Repository
{

    public class CampRepository : ICampRepository
    {
        public CampRepository(MvcAppDbContext context)
        {
            _context = context;
        }

        public MvcAppDbContext _context { get; set; }

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
            return await _context.Camps.Include(c => c.Images).Include(a => a.Address).ToListAsync();
        }

        public async Task<Camp> GetById(int id)
        {
            return await _context.Camps.Include(c => c.Images).Include(a => a.Address).Include(b => b.Bookings).FirstOrDefaultAsync(c => c.CampID == id);
        }

        public async Task<Camp> GetByIdNoTracking(int id)
        {
            return await _context.Camps.Include(c => c.Images).Include(a => a.Address).AsNoTracking().FirstOrDefaultAsync(c => c.CampID == id);
        }

        public async Task<IEnumerable<Camp>> GetCampByDistrict(string district)
        {
            return await _context.Camps.Where(c => c.Address.District.Contains(district)).ToListAsync();
        }

        public async Task<IEnumerable<Camp>> GetCampByGovernment(string government)
        {
            return await _context.Camps.Where(c => c.Address.Government.Contains(government)).ToListAsync();
        }

        public async Task<IEnumerable<Camp>> GetCampByCity(string city)
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

        public async Task<decimal> GetPriceByCampId(int campId)
        {
            var camp = await _context.Camps.FirstOrDefaultAsync(c => c.CampID == campId);
            return camp.PricePerNight;
        }

        public async Task<IEnumerable<Camp>> GetCampByCategory(CampCategory category)
        {
            return await _context.Camps.Where(c => c.CampCategory == category).ToListAsync();
        }

        public async Task<int> GetCampCount()
        {
            return  await _context.Camps.CountAsync();
        }

        public async Task<List<string>> GetCampNames()
        {
            return _context.Camps
                           .Select(c => c.CampName) 
                           .ToList();
        }

        // Returns a list of booking counts for each camp
        public async Task<List<int>> GetCampBookingsCount()
        {
            return _context.Camps
                           .Select(c => c.Bookings.Count) 
                           .ToList();
        }

    }
}
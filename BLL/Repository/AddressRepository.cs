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
    public class AddressRepository : IAddressRepository
    {
        public MvcAppDbContext _context { get; set; }
        public AddressRepository(MvcAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Address>> GetAll()
        {
            return await _context.Addresses.ToListAsync();
        }

        public async Task<Address> GetById(int id)
        {
            return await _context.Addresses.FirstOrDefaultAsync(c => c.AddressId == id);
        }

        public async Task<Address> GetByAddressByGovernmentCityDistrict(Address address)
        {
            return await _context.Addresses.FirstOrDefaultAsync(c => c.Government == address.Government && c.City == address.City && c.District == address.District);
        }

        public bool Add(Address address)
        {
            _context.Add(address);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public async Task<IEnumerable<string>> GetGovernments()
        {
            var governments = await _context.Addresses.Select(c => c.Government).Distinct().ToListAsync();
            governments.Insert(0, "All Governments");
            return governments;
        }

        public async Task<IEnumerable<string>> GetCities()
        {
            var cities =  await _context.Addresses.Select(c => c.City).Distinct().ToListAsync();
            cities.Insert(0, "All Cities");
            return cities;
        }

        public async Task<IEnumerable<string>> GetDistricts()
        {
            var districts = await _context.Addresses.Select(c => c.District).Distinct().ToListAsync();
            districts.Insert(0, "All Districts");
            return districts;
        }
    }
}

﻿using BLL.Interfaces;
using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repository;

public class AddressRepository : IAddressRepository
{
    public AddressRepository(MvcAppDbContext context)
    {
        _context = context;
    }

    public MvcAppDbContext _context { get; set; }

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
        return await _context.Addresses.FirstOrDefaultAsync(c =>
            c.Government == address.Government && c.City == address.City && c.District == address.District);
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
}
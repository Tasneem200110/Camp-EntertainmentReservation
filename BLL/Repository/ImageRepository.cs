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
    public class ImageRepository : IImageRepository
    {
        public MvcAppDbContext _context { get; set; }
        public ImageRepository(MvcAppDbContext context) 
        {
            _context = context;
        }
        public bool Add(Image image)
        {
            _context.Add(image);
            return Save();
        }
        public bool AddRange(IEnumerable<Image> images)
        {
            _context.Images.AddRange(images);
            return Save();  
        }
        public bool Delete(Image image)
        {
            _context.Remove(image);
            return Save();
        }

        //public bool DeleteById(int id)
        //{
            
        //}
        public async Task DeleteById(int id)
        {
            var image = await _context.Images.FindAsync(id);

            if (image != null)
            {
                _context.Images.Remove(image);

                Save();

            }
            //return S
        }

        public bool DeleteByCampId(int campId)
        {
            var images = _context.Images.Where(img => img.CampId == campId).ToList();
            if (images == null || !images.Any())
            {
                return false;
            }
            _context.Images.RemoveRange(images);
            return Save();
        }

        public bool DeleteByUserId(int userId)
        {
            throw new NotImplementedException();
        }
        public async Task<Image> GetById(int id)
        {
            return await _context.Images.Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Image>> GetAllByCampId(int campId)
        {
            return await _context.Images.Where(i => i.CampId == campId).ToListAsync();
        }

        public async Task<Image> GetByUserId(int userId)
        {
            return await _context.Images.FirstOrDefaultAsync(i => i.UserId == userId);
        }

        public async Task<Image> GetCampFirstImage(int campId)
        {
            return await _context.Images.FirstOrDefaultAsync(i => i.CampId == campId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Image image)
        {
            _context.Update(image);
            return Save();
        }
    }
}

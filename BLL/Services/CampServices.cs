using DAL.Data.Enum;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public static class CampServices
    {
        public static List<Camp> GetCampByDistrict(string district, IEnumerable<Camp> camps)
        {
            return camps.Where(c => c.Address.District.Contains(district)).ToList();
        }
        public static List<Camp> GetCampByCity(string city, IEnumerable<Camp> camps)
        {
            return camps.Where(c => c.Address.City.Contains(city)).ToList();
        }
        public static List<Camp> GetCampByGovernment(string government, IEnumerable<Camp> camps)
        {
            var filteredCamps = camps.Where(c =>
            {
                if (c.Address == null)
                {
                    // Log a warning or handle the situation appropriately
                    Console.WriteLine($"Camp ID {c.CampID} has a null address.");
                    return false; // Skip this camp
                }
                return c.Address.Government.Contains(government);
            }).ToList();

            return filteredCamps;
            //return camps.Where(c => c.Address.Government.Contains(government)).ToList();
        }
        public static List<Camp> GetCampByCategory(string category, IEnumerable<Camp> camps)
        {
            if (Enum.TryParse(category, out CampCategory parsedCategory))
            {
                return camps.Where(c => c.CampCategory == parsedCategory).ToList();
            }
            else
            {
                return new List<Camp>();
            }
        }
        //public static List<Camp> GetCampAllCategories(IEnumerable<Camp> camps)
        //{

        //}
    }
}

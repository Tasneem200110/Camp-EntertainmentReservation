using DAL.Context;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Data.Enum;
using System.Diagnostics;

namespace DAL.Data
{
    public class DataSeeder
    {
        public static void SeedCampData(MvcAppDbContext context)
        {
            if (!context.Addresses.Any())
            {
                var addresses = new List<Address>
                {
                    new Address { Government = "Cairo", City = "Nasr City", District = "7th District" },
                    new Address { Government = "Giza", City = "Dokki", District = "Tahrir Street" },
                    new Address { Government = "Cairo", City = "Maadi", District = "Corniche" },
                    new Address { Government = "Giza", City = "Sheikh Zayed", District = "Zayed Central" }
                };
                context.Addresses.AddRange(addresses);
                context.SaveChanges();
            }
            if (!context.Camps.Any())
            {
                var camps = new List<Camp>
                {
                    new Camp()
                    {
                        CampName = "Nile Adventure Park",
                        Description = "Experience thrilling activities along the Nile.",
                        CampCategory = CampCategory.Adventure,
                        Image = "https://res.cloudinary.com/abercrombie-kent-ltd/image/upload/f_auto,q_auto,h_656,w_1130,c_fill/dpr_1.0/v1686053148/websites-development/vlhcj4rk_mw190915nileadventurer_41_016.jpg",
                        PricePerNight = 200.00M,
                        AddressId = 1,
                        AvailabilityStartDate = DateTime.Now,
                        AvailabilityEndDate = DateTime.Now.AddMonths(6),
                    },
                    new Camp()
                    {
                        CampName = "Pyramids Cultural Camp",
                        Description = "Discover the history and culture around the Pyramids.",
                        CampCategory = CampCategory.Historical,
                        Image = "https://cdn.britannica.com/13/170813-131-B69A007D/Great-Pyramid-of-Cheops-Giza-Egypt.jpg",
                        PricePerNight = 250.00M,
                        AddressId = 2,
                        AvailabilityStartDate = DateTime.Now,
                        AvailabilityEndDate = DateTime.Now.AddMonths(4),
                    },
                    new Camp()
                    {
                        CampName = "Cairo Arts Retreat",
                        Description = "Immerse yourself in art and music in a serene environment.",
                        CampCategory = CampCategory.Music_Arts,
                        Image = "https://artlogic-res.cloudinary.com/w_1600,h_1600,c_limit,f_auto,fl_lossy,q_auto/artlogicstorage/azadartgallery/images/view/d0b16ca1948dbaffe94287d5bedadca4/azadartgallery-osama-farid-old-islamic-cairo-in-gold-2021.jpg",
                        PricePerNight = 150.00M,
                        AddressId = 1,
                        AvailabilityStartDate = DateTime.Now,
                        AvailabilityEndDate = DateTime.Now.AddMonths(5),
                    },
                    new Camp()
                    {
                        CampName = "Family Fun Resort",
                        Description = "A family-friendly resort with activities for all ages.",
                        CampCategory = CampCategory.FamilyFriendly,
                        Image = "https://www.propertyfinder.eg/blog/wp-content/uploads/2020/10/0013.png",
                        PricePerNight = 180.00M,
                        AddressId = 4,
                        AvailabilityStartDate = DateTime.Now,
                        AvailabilityEndDate = DateTime.Now.AddMonths(3),
                    }
                };

                context.Camps.AddRange(camps);
                context.SaveChanges();
            }
        }


        //public static void SeedData(IApplicationBuilder applicationBuilder)
        //    {
        //        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        //        {
        //            var context = serviceScope.ServiceProvider.GetService<MvcAppDbContext>();

        //            context.Database.EnsureCreated();

        //            if (!context.Clubs.Any())
        //            {
        //                context.Clubs.AddRange(new List<Club>()
        //                {
        //                    new Club()
        //                    {
        //                        Title = "Running Club 1",
        //                        Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
        //                        Description = "This is the description of the first club",
        //                        ClubCategory = ClubCategory.City,
        //                        Address = new Address()
        //                        {
        //                            Street = "123 Main St",
        //                            City = "Charlotte",
        //                            State = "NC"
        //                        }
        //                     },
        //                    new Club()
        //                    {
        //                        Title = "Running Club 2",
        //                        Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
        //                        Description = "This is the description of the first cinema",
        //                        ClubCategory = ClubCategory.Endurance,
        //                        Address = new Address()
        //                        {
        //                            Street = "123 Main St",
        //                            City = "Charlotte",
        //                            State = "NC"
        //                        }
        //                    },
        //                    new Club()
        //                    {
        //                        Title = "Running Club 3",
        //                        Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
        //                        Description = "This is the description of the 2nd club",
        //                        ClubCategory = ClubCategory.Trail,
        //                        Address = new Address()
        //                        {
        //                            Street = "123 Main St",
        //                            City = "Charlotte",
        //                            State = "NC"
        //                        }
        //                    },
        //                    new Club()
        //                    {
        //                        Title = "Running Club 3",
        //                        Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
        //                        Description = "This is the description of the 3rd club",
        //                        ClubCategory = ClubCategory.City,
        //                        Address = new Address()
        //                        {
        //                            Street = "123 Main St",
        //                            City = "Michigan",
        //                            State = "NC"
        //                        }
        //                    }
        //                });
        //                context.SaveChanges();
        //            }

        //        }
        //    }
    }
}



//var camps2 = new List<Camp>
//        {
//            new Camp
//            {
//                CampName = "Sunny Beach Resort",
//                Description = "A beautiful beach resort with stunning views.",
//                CampCategory = CampCategory.Resort,
//                Image = "https://media.istockphoto.com/id/926497236/photo/tropical-sea-in-summer.jpg?s=612x612&w=0&k=20&c=SgT--E-a5_UF4GPVnpC6r1IDtThuDiUHmtTbhHg5zJA=",
//                PricePerNight = 150.00M,
//                AddressId = 1,
//                AvailabilityStartDate = DateTime.Now,
//                AvailabilityEndDate = DateTime.Now.AddMonths(6),
//            },
//            new Camp
//            {
//                CampName = "Mountain Adventure Camp",
//                Description = "An exciting camp for hiking and mountain sports.",
//                CampCategory = CampCategory.Adventure,
//                Image = "https://www.journeysinternational.com/wp-content/uploads/2019/05/nepal-mountain-hikers-625x390.jpg",
//                PricePerNight = 100.00M,
//                AddressId = 2,
//                AvailabilityStartDate = DateTime.Now,
//                AvailabilityEndDate = DateTime.Now.AddMonths(3),
//            },
//            new Camp
//            {
//                CampName = "Historical Sites Tour",
//                Description = "Explore historical sites in a guided tour.",
//                CampCategory = CampCategory.Historical,
//                Image = "https://cdn.britannica.com/13/170813-131-B69A007D/Great-Pyramid-of-Cheops-Giza-Egypt.jpg",
//                PricePerNight = 120.00M,
//                AddressId = 3,
//                AvailabilityStartDate = DateTime.Now,
//                AvailabilityEndDate = DateTime.Now.AddMonths(2),
//            },
//            new Camp
//            {
//                CampName = "Family Fun Park",
//                Description = "A family-friendly park with numerous attractions.",
//                CampCategory = CampCategory.Entertainment,
//                Image = "https://www.propertyfinder.eg/blog/wp-content/uploads/2020/10/0013.png",
//                PricePerNight = 80.00M,
//                AddressId = 4,
//                AvailabilityStartDate = DateTime.Now,
//                AvailabilityEndDate = DateTime.Now.AddMonths(5),
//            }
//        };


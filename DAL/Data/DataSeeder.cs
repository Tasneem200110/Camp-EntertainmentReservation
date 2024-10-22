using DAL.Context;
using DAL.Data.Enum;
using DAL.Entities;

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
                new() { Government = "Cairo", City = "Nasr City", District = "7th District" },
                new() { Government = "Giza", City = "Dokki", District = "Tahrir Street" },
                new() { Government = "Cairo", City = "Maadi", District = "Corniche" },
                new() { Government = "Giza", City = "Sheikh Zayed", District = "Zayed Central" },
                new() { Government = "Red Sea", City = "Hurghada" }
            };
                context.Addresses.AddRange(addresses);
                context.SaveChanges();
            }

            if (!context.Camps.Any())
            {
                var camps = new List<Camp>
            {
                new()
                {
                    CampName = "Gifton Island",
                    Description =
                        "Giftun Island is one of the top destination island in Hurghada. Set your spirit free and enjoy amazing snorkeling experience to discover the wonderful underwater world around the island.",
                    CampCategory = CampCategory.Nature,
                    Image =
                        "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/0f/2e/40/58/giftun-islands-red-sea.jpg?w=1200&h=-1&s=1",
                    PricePerNight = 250.00M,
                    AddressId = 5,
                    AvailabilityStartDate = DateTime.Now,
                    AvailabilityEndDate = DateTime.Now.AddMonths(6)
                },
                new()
                {
                    CampName = "Nile Adventure Park",
                    Description = "Experience thrilling activities along the Nile.",
                    CampCategory = CampCategory.Adventure,
                    Image =
                        "https://res.cloudinary.com/abercrombie-kent-ltd/image/upload/f_auto,q_auto,h_656,w_1130,c_fill/dpr_1.0/v1686053148/websites-development/vlhcj4rk_mw190915nileadventurer_41_016.jpg",
                    PricePerNight = 200.00M,
                    AddressId = 1,
                    AvailabilityStartDate = DateTime.Now,
                    AvailabilityEndDate = DateTime.Now.AddMonths(6)
                },
                new()
                {
                    CampName = "Pyramids Cultural Camp",
                    Description = "Discover the history and culture around the Pyramids.",
                    CampCategory = CampCategory.Historical,
                    Image = "https://cdn.britannica.com/13/170813-131-B69A007D/Great-Pyramid-of-Cheops-Giza-Egypt.jpg",
                    PricePerNight = 250.00M,
                    AddressId = 2,
                    AvailabilityStartDate = DateTime.Now,
                    AvailabilityEndDate = DateTime.Now.AddMonths(4)
                },
                new()
                {
                    CampName = "Cairo Arts Retreat",
                    Description = "Immerse yourself in art and music in a serene environment.",
                    CampCategory = CampCategory.Music_Arts,
                    Image =
                        "https://artlogic-res.cloudinary.com/w_1600,h_1600,c_limit,f_auto,fl_lossy,q_auto/artlogicstorage/azadartgallery/images/view/d0b16ca1948dbaffe94287d5bedadca4/azadartgallery-osama-farid-old-islamic-cairo-in-gold-2021.jpg",
                    PricePerNight = 150.00M,
                    AddressId = 1,
                    AvailabilityStartDate = DateTime.Now,
                    AvailabilityEndDate = DateTime.Now.AddMonths(5)
                },
                new()
                {
                    CampName = "Family Fun Resort",
                    Description = "A family-friendly resort with activities for all ages.",
                    CampCategory = CampCategory.FamilyFriendly,
                    Image = "https://www.propertyfinder.eg/blog/wp-content/uploads/2020/10/0013.png",
                    PricePerNight = 180.00M,
                    AddressId = 4,
                    AvailabilityStartDate = DateTime.Now,
                    AvailabilityEndDate = DateTime.Now.AddMonths(3)
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
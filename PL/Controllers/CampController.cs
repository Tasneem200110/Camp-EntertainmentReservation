using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PL.ViewModels;
using BLL.Services;
using DAL.Data.Enum;
using BLL.Repository;

namespace PL.Controllers
{
    public class CampController : Controller
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ICampRepository _campRepository;
        private readonly IPhotoUploadService _photoService;
        private readonly IImageRepository _imageRepository;
        private readonly bool campImageDefaultFlag = false;

        public CampController(ICampRepository campRepository, IAddressRepository addressRepository,
            IPhotoUploadService photoService, IImageRepository imageRepository)
        {
            _campRepository = campRepository;
            _addressRepository = addressRepository;
            _photoService = photoService;
            _imageRepository = imageRepository;
        }


        public async Task<IActionResult> Index(string selectedCategory, string selectedGovernment, string selectedCity, string selectedDistrict)
        {
            // Get all camps
            IEnumerable<Camp> camps = await _campRepository.GetAll();

            // Apply filters if any are selected
            if (selectedCategory != null && selectedCategory != "All Categories")
            {
                camps = CampServices.GetCampByCategory(selectedCategory, camps);
            }

            if (!string.IsNullOrEmpty(selectedGovernment) && selectedGovernment != "All Governments")
            {
                camps = CampServices.GetCampByGovernment(selectedGovernment, camps);
            }

            if (!string.IsNullOrEmpty(selectedCity) && selectedCity != "All Cities")
            {
                camps = CampServices.GetCampByCity(selectedCity, camps);
            }

            var viewModel = new ListCampViewModel
            {
                Camps = camps,
                Categories = new[] { "All Categories" }.Concat(Enum.GetValues(typeof(CampCategory)).Cast<CampCategory>().Select(c => c.ToString())),
                Governments = await _addressRepository.GetGovernments(),
                Cities = await _addressRepository.GetCities(),
                Districts = await _addressRepository.GetDistricts(),
                SelectedCategory = selectedCategory,
                SelectedGovernment = selectedGovernment,
                SelectedCity = selectedCity
            };

            return View(viewModel);
        }


        public async Task<IActionResult> Detail(int id)
        {
            Camp camp = await _campRepository.GetById(id);
            return View(camp);
        }

        public async Task<IActionResult> Create()
        {
            var addresses = await _addressRepository.GetAll();
            AddressList(addresses);
            return View();
        }

        public async void AddressList(IEnumerable<Address> addresses)
        {
            if (addresses != null)
            {
                var groupedAddresses = addresses
                    .GroupBy(a => a.Government)
                    .Select(g => new SelectListGroup
                    {
                        Name = g.Key
                    }).ToList();

                var addressList = new List<SelectListItem>();

                foreach (var group in groupedAddresses)
                    // Add each city/district as a SelectListItem
                    foreach (var address in addresses.Where(a => a.Government == group.Name))
                        addressList.Add(new SelectListItem
                        {
                            Value = address.AddressId.ToString(),
                            Text = $"{address.City}, {address.District}",
                            Group = group
                        });
                ViewBag.AddressList = addressList;
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateCampViewModel campVM)
        {
            int errorCount = ModelState.Values.Sum(v => v.Errors.Count);
            string ImageUrl;
            List<string> imagesrc = new List<string>();
            bool modelNotValid = errorCount > 1;       //if the error is less than 2 then model is valid

            if (!modelNotValid)
            {
                
                //ImageUrl = await _photoService.AddPhoto(campVM.Image, campImageDefaultFlag);

                if (campVM.ImagesUrls != null)
                {
                    imagesrc = await _photoService.AddPhotos(campVM.ImagesUrls, campImageDefaultFlag);

                }

                var camp = new Camp
                {
                    CampName = campVM.CampName,
                    Description = campVM.Description,
                    CampCategory = campVM.CampCategory,
                    //Image = campVM.ImageUrl,
                    PricePerNight = campVM.PricePerNight,
                    AvailabilityStartDate = campVM.AvailabilityStartDate,
                    AvailabilityEndDate = campVM.AvailabilityEndDate
                };

                if (campVM.ExisitingAddressFlag)
                {
                    var existingAddress = await _addressRepository.GetById((int)campVM.AddressId);
                    camp.Address = existingAddress;
                }
                else
                {
                    var existingAddress = await _addressRepository.GetByAddressByGovernmentCityDistrict(campVM.Address);
                    if (existingAddress != null) //If the added address already exist
                    {
                        camp.Address = existingAddress;
                        camp.AddressId = existingAddress.AddressId;
                    }
                    else
                    {
                        _addressRepository.Add(campVM.Address);
                        camp.Address = campVM.Address;
                    }
                }

                _campRepository.Add(camp);

                foreach (var src in imagesrc)
                {
                    var image = new Image
                    {
                        Source = src,
                        CampId = camp.CampID
                    };

                    camp.Images.Add(image);

                    _imageRepository.Add(image);

                    //_imageRepository.Add(image1);
                    //await _photoService.DeletePhoto(campVM.FirstImageSrc);
                }

                //var image = new Image
                //{
                //    Source = ImageUrl,
                //    CampId = camp.CampID 
                //};

                //camp.Images.Add(image);

                //_imageRepository.Add(image);

                return RedirectToAction("Index");
            }

            var addresses = await _addressRepository.GetAll();
            AddressList(addresses);
            return View(campVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var addresses = await _addressRepository.GetAll();
            AddressList(addresses);
            var camp = await _campRepository.GetById(id);
            if (camp == null)
                return View("Error");
            var campVM = new EditCampViewModel
            {
                CampID = camp.CampID,
                CampName = camp.CampName,
                Description = camp.Description,
                CampCategory = camp.CampCategory,
                AddressId = camp.AddressId,
                Address = camp.Address,
                //FirstImageSrc = CampServices.GetFirstImageSrc(camp),
                PricePerNight = camp.PricePerNight,
                AvailabilityStartDate = camp.AvailabilityStartDate,
                AvailabilityEndDate = camp.AvailabilityEndDate,
                Images = camp.Images
            };
            return View(campVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit( EditCampViewModel campVM)
        {
            var id = campVM.CampID;
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit camp");
                return View("Edit", campVM);
            }

            var camp = await _campRepository.GetByIdNoTracking(id);
            if (camp != null)
            {
               
                if (campVM.ImagesUrls != null)
                {
                    var imagesrc = await _photoService.AddPhotos(campVM.ImagesUrls, campImageDefaultFlag);
                    foreach (var src in imagesrc)
                    {
                        var image = new Image
                        {
                            Source = src,
                            CampId = id
                        };

                        campVM.Images.Add(image);

                        _imageRepository.Add(image);
                        //await _photoService.DeletePhoto(campVM.FirstImageSrc);
                    }
                    
                }
                else
                    campVM.Images = camp.Images;

                if (campVM.ExisitingAddressFlag)
                {
                    var existingAddress = await _addressRepository.GetById((int)campVM.AddressId);
                    campVM.Address = existingAddress;
                }
                else
                {
                    var existingAddress = await _addressRepository.GetByAddressByGovernmentCityDistrict(campVM.Address);
                    if (existingAddress != null)
                    {
                        campVM.Address = existingAddress;
                        campVM.AddressId = existingAddress.AddressId;
                    }
                    else
                    {
                        _addressRepository.Add(campVM.Address);
                    }
                }

                var updatedcamp = new Camp
                {
                    CampID = id,
                    CampName = campVM.CampName,
                    Description = campVM.Description,
                    //Image = campVM.ImageUrl,
                    Address = campVM.Address,
                    AddressId = campVM.AddressId,
                    CampCategory = campVM.CampCategory,
                    PricePerNight = campVM.PricePerNight,
                    AvailabilityEndDate = campVM.AvailabilityEndDate,
                    AvailabilityStartDate = campVM.AvailabilityStartDate,
                    Images = campVM.Images,
                };

                _campRepository.Update(updatedcamp);
                return RedirectToAction("Index");
            }
            else
            {
                return View(campVM);
            }
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var campDetails = await _campRepository.GetById(id);
            if (campDetails == null)
                return View("Error");
            _campRepository.Delete(campDetails);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteImage(int imageId, EditCampViewModel campVM)
        {
            var image = await _imageRepository.GetById(imageId);

            if (image != null)
            {
                await _photoService.DeletePhoto(image.Source);
                _imageRepository.Delete(image); 
            }
            campVM.Images = (ICollection<Image>)await _imageRepository.GetAllByCampId(campVM.CampID);

            var addresses = await _addressRepository.GetAll();
            AddressList(addresses);
            return View("Edit", campVM);
        }

    }
}
using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Context;
using Microsoft.AspNetCore.Components.Web;
using PL.ViewModels;

namespace PL.Controllers
{
    public class CampController : Controller
    {
        private readonly ICampRepository _campRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IPhotoService _photoService;
        private readonly bool campImageDefaultFlag = false;

        public CampController(ICampRepository campRepository, IAddressRepository addressRepository, IPhotoService photoService)
        {
            _campRepository = campRepository;
            _addressRepository = addressRepository;
            _photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Camp> camps = await _campRepository.GetAll();
            return View(camps);
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
                {
                    // Add each city/district as a SelectListItem
                    foreach (var address in addresses.Where(a => a.Government == group.Name))
                    {
                        addressList.Add(new SelectListItem
                        {
                            Value = address.AddressId.ToString(),
                            Text = $"{address.City}, {address.District}",
                            Group = group
                        });
                    }
                }
                ViewBag.AddressList = addressList;
            }

        }



        [HttpPost]
        public async Task<IActionResult> Create(CreateCampViewModel campVM)
        {
            int errorCount = ModelState.Values.Sum(v => v.Errors.Count);
            bool modelNotValid = errorCount > 1;       //if the error is less than 2 then model is valid

            if (!modelNotValid)
            {
                campVM.ImageUrl = await _photoService.AddPhoto(campVM.Image, campImageDefaultFlag);

                var camp = new Camp
                {
                    CampName = campVM.CampName,
                    Description = campVM.Description,
                    CampCategory = campVM.CampCategory,
                    Image = campVM.ImageUrl,
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
                    if (existingAddress != null)        //If the added address already exist
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
                CampName = camp.CampName,
                Description = camp.Description,
                CampCategory = camp.CampCategory,
                AddressId = camp.AddressId,
                Address = camp.Address,                
                ImageUrl = camp.Image,
                PricePerNight = camp.PricePerNight,
                AvailabilityStartDate = camp.AvailabilityStartDate,
                AvailabilityEndDate = camp.AvailabilityEndDate
            };
            return View(campVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditCampViewModel campVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit camp");
                return View("Edit", campVM);
            }
            var camp = await _campRepository.GetByIdNoTracking(id);
            if (camp != null)
            {
                try
                {
                    await _photoService.DeletePhoto(camp.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete the photo");
                    return View(campVM);
                }
                if (campVM.Image != null)
                {
                    campVM.ImageUrl = await _photoService.AddPhoto(campVM.Image, campImageDefaultFlag);
                }
                else
                    campVM.ImageUrl = camp.Image;

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
                    Image = campVM.ImageUrl,
                    Address = campVM.Address,
                    AddressId = campVM.AddressId,
                    CampCategory = campVM.CampCategory,
                    PricePerNight = campVM.PricePerNight,
                    AvailabilityEndDate = campVM.AvailabilityEndDate,
                    AvailabilityStartDate = campVM.AvailabilityStartDate,
                };

                _campRepository.Update(updatedcamp);
                return RedirectToAction("Index");
            }
            else
            {
                return View(campVM);
            }
        }
    }

}


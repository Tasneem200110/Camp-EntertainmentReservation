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
            if (addresses != null)
            {
                var groupedAddresses = addresses
                    .GroupBy(a => a.Government) // Group by Government
                    .Select(g => new SelectListGroup
                    {
                        Name = g.Key
                    }).ToList();

                var addressList = new List<SelectListItem>();

                foreach (var group in groupedAddresses)
                {
                    addressList.Add(new SelectListItem
                    {
                        Value = group.Name, // You can set the value or any identifier for the government
                        
                        Group = group
                    });

                    // Add each city/district as a SelectListItem, but not as a group header
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

                ViewBag.AddressList = addressList; // Store the list of SelectListItem in ViewBag
            }


            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(Camp camp)
        //{

        //    bool isAddressIdValid = !string.IsNullOrEmpty(camp.AddressId?.ToString()) && int.TryParse(camp.AddressId.ToString(), out _);
        //    bool isGovernmentValid = !string.IsNullOrEmpty(camp.Address.Government);

        //    int errorCount = ModelState.Values.Sum(v => v.Errors.Count);

        //    bool moreThanTwoErrors = errorCount > 1;

        //    if (!isAddressIdValid && !isGovernmentValid || moreThanTwoErrors)
        //    {
        //        return View(camp);
        //    }
        //    if (isAddressIdValid)
        //    {
        //        var existingAddress = await _addressRepository.GetById((int)camp.AddressId);
        //        camp.Address = existingAddress;
        //    }
        //    else
        //    {
        //        var existingAddress = await _addressRepository.GetByAddressByGovernmentCityDistrict(camp.Address);
        //        if (existingAddress != null)        //If the added address already exist
        //        {
        //            camp.Address = existingAddress;
        //            camp.AddressId = existingAddress.AddressId;
        //        }
        //        else
        //        {
        //            _addressRepository.Add(camp.Address);
        //        }
        //    }
        //        _campRepository.Add(camp);
        //        return RedirectToAction("Index");
        //}


        [HttpPost]
        public async Task<IActionResult> Create(CreateCampViewModel campVM)
        {
            bool isAddressIdValid = !string.IsNullOrEmpty(campVM.AddressId?.ToString()) && int.TryParse(campVM.AddressId.ToString(), out _);
            bool isGovernmentValid = !string.IsNullOrEmpty(campVM.Address.Government);

            int errorCount = ModelState.Values.Sum(v => v.Errors.Count);

            bool modelNotValid = errorCount > 1;       //if the error is less than 2 then model is valid

            if (!modelNotValid)
            {
                var result = await _photoService.AddPhoto(campVM.Image);

                var camp = new Camp
                {
                    CampName = campVM.CampName,
                    Description = campVM.Description,
                    CampCategory = campVM.CampCategory,
                    Image = result.Url.ToString(),
                    PricePerNight = campVM.PricePerNight,
                    AvailabilityStartDate = campVM.AvailabilityStartDate,
                    AvailabilityEndDate = campVM.AvailabilityEndDate
                };

                if (isAddressIdValid)
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
            return View(campVM);

        }
    }

}


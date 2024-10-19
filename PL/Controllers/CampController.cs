using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL.Context;
using Microsoft.AspNetCore.Components.Web;

namespace PL.Controllers
{
    public class CampController : Controller
    {
        private readonly ICampRepository _campRepository;
        private readonly IAddressRepository _addressRepository;

        public CampController(ICampRepository campRepository, IAddressRepository addressRepository)
        {
            _campRepository = campRepository;
            _addressRepository = addressRepository;
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

        [HttpPost]
        public async Task<IActionResult> Create(Camp camp)
        {

            bool isAddressIdValid = !string.IsNullOrEmpty(camp.AddressId?.ToString()) && int.TryParse(camp.AddressId.ToString(), out _);
            bool isGovernmentValid = !string.IsNullOrEmpty(camp.Address.Government);

            int errorCount = ModelState.Values.Sum(v => v.Errors.Count);

            bool moreThanTwoErrors = errorCount > 2;

            if (!isAddressIdValid && !isGovernmentValid || moreThanTwoErrors)
            {
                return View(camp);
            }
            if (isAddressIdValid)
            {
                var existingAddress = await _addressRepository.GetById((int)camp.AddressId);
                camp.Address = existingAddress;
            }
            else
            {
                var existingAddress = await _addressRepository.GetByAddressByGovernmentCityDistrict(camp.Address);
                if (existingAddress != null)        //If the added address already exist
                {
                    camp.Address = existingAddress;
                    camp.AddressId = existingAddress.AddressId;
                }
                else
                {
                    _addressRepository.Add(camp.Address);
                }
            }
                _campRepository.Add(camp);
                return RedirectToAction("Index");
        }
    }

}


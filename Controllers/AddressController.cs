using ECommerceProject.Models;
using ECommerceProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace ECommerceProject.Controllers
{
    [Authorize(Roles = "Customer")]
    public class AddressController : Controller
	{

		private readonly IAddressService _addressService;
		public AddressController(IAddressService _addressService)
		{
			this._addressService = _addressService;
		}
		public ActionResult Index()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var addresses = _addressService.GetAddressesByUserId(userId);
			return View(addresses);
		}

		// GET: AddressController/Details/5
		/*public ActionResult Details(int id)
		{
			return View();
		}*/

		// GET: AddressController/Create
		public ActionResult Create()
		{
			return View("CreateEdit", new Address());

		}

		// POST: AddressController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Address address)
		{
			try
			{

				address.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
				int result = _addressService.CreateAddress(address);


				if (result >= 1)
				{
					return RedirectToAction(nameof(Index));
				}
				else
				{
					ViewBag.ErrorMsg = "Something went wrong...";
					return View("CreateEdit", address);
				}



			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return View();
			}
		}

		// GET: AddressController/Edit/5
		public ActionResult Edit(int id)
		{
			var address = _addressService.GetAddressById(id);
			if (address == null || address.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
			{
				return NotFound();
			}
			return View("CreateEdit", address);
		}

		// POST: AddressController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Address address)
		{
			try
			{

				var _address = new Address
				{
					AddressId = address.AddressId,
					UserId = address.UserId,
					FullName = address.FullName,
					MobileNumber = address.MobileNumber,
					Pincode = address.Pincode,
					Apartment = address.Apartment,
					Street = address.Street,
					Landmark = address.Landmark,
					City = address.City,
					State = address.State,
					Country = address.Country

				};

				int result = _addressService.UpdateAddress(_address);

				if (result >= 1)
				{
					return RedirectToAction(nameof(Index));
				}
				else
				{
					ViewBag.ErrorMsg = "Something went wrong...";
					return View("CreateEdit", address);
				}

			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return View("CreateEdit", address);
			}
		}

		// GET: AddressController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: AddressController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				_addressService.DeleteAddress(id);
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return View();
			}
		}
	}
}

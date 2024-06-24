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


		public ActionResult Create()
		{
			return View("CreateEdit", new Address());

		}


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

		public ActionResult Edit(int id)
		{
			var address = _addressService.GetAddressById(id);
			if (address == null || address.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
			{
				return NotFound();
			}
			return View("CreateEdit", address);
		}


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

		public ActionResult Delete(int id)
		{
			return View();
		}


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

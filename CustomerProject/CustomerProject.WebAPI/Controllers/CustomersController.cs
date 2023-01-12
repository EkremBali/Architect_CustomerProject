using CustomerProject.Business.Abstract;
using CustomerProject.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CustomerProject.WebAPI.Controllers
{
	//Yönlendirmenin /controller ismi/action ismi -> şeklinde olmasını sağladık.
	[Route("[controller]/[action]")]
	[ApiController]
	public class CustomersController:ControllerBase
	{
		private ICustomerService customerService;

		public CustomersController(ICustomerService customerService, IContactService contactService)
		{
			this.customerService = customerService;
		}

		[HttpGet]
		public async Task<IActionResult> GetCustomers()
		{
			var customers = await customerService.GetAllAsync(null);
			
			return Ok(customers);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCustomer(int id)
		{
			var customers = await customerService.GetByIdAsync(id);

			return Ok(customers);
		}

		[HttpGet("{search}")]
		public async Task<IActionResult> GetSearchedCustomers(string search)
		{
			var customers = await customerService.GetAllAsync(search);

			return Ok(customers);
		}


		[HttpPost]
		public async Task<IActionResult> AddCustomer(Customer customer)
		{
			if (customer.TC.Length != 11 || customer.BirthYear.ToString().Length != 4 || await customerService.IsHaveTC(customer.TC))
			{
				return BadRequest("Tc ve Doğum yılı haneleri yanlış veya TC kayıtlı");
			}

			if (NullControl(customer))
			{
				return BadRequest("Boş eleman bırakılamaz");
			}

			await customerService.CreateAsync(customer);

			return NoContent();
		}


		[HttpPut]
		public async Task<IActionResult> PutCustomer(Customer updatedCustomer)
		{
			var customer = await customerService.GetByIdAsync(updatedCustomer.Id);

			if(customer == null)
			{
				return NotFound();
			}

			if (updatedCustomer.TC.Length != 11 || updatedCustomer.BirthYear.ToString().Length != 4 || await customerService.IsHaveTC(updatedCustomer.TC))
			{
				return BadRequest("Tc ve Doğum yılı haneleri yanlış veya TC kayıtlı");
			}

			if (NullControl(updatedCustomer))
			{
				return BadRequest("Boş eleman bırakılamaz");
			}

			customer.Name = updatedCustomer.Name;
			customer.Surname = updatedCustomer.Surname;
			customer.TC = updatedCustomer.TC;
			customer.BirthPlace = updatedCustomer.BirthPlace;
			customer.BirthYear = updatedCustomer.BirthYear;

			await customerService.UpdateAsync(customer);

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCustomer(int id)
		{
			var customer = await customerService.GetByIdAsync(id);

			if(customer == null)
			{
				return NotFound();
			}

			await customerService.DeleteAsync(customer);

			return NoContent();
		}

		public bool NullControl(Customer customer)
		{
			if (string.IsNullOrEmpty(customer.Name) || string.IsNullOrEmpty(customer.Surname) || string.IsNullOrEmpty(customer.BirthPlace))
			{
				return true;
			}

			return false;
		}
	}

}

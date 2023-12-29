using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Data.Entity;
using Vb.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VbApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AddressesController : ControllerBase
	{
		private readonly VbDbContext dbContext;
		public AddressesController(VbDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpGet]
		public async Task<List<Address>> Get()
		{
			return await dbContext.Set<Address>()
				.ToListAsync();
		}


		[HttpGet("{id}")]
		public async Task<Address> GetById(int id)
		{
			var account = await dbContext.Set<Address>()
			.Where(x => x.Id == id).FirstOrDefaultAsync();

			return account;
		}

		[HttpPost]
		public async Task Post([FromBody] Address address)
		{
			await dbContext.Set<Address>().AddAsync(address);
			await dbContext.SaveChangesAsync();
		}


		[HttpPut("{id}")]
		public async Task Put(int id, [FromBody] Address address)
		{
			var fromdb = await dbContext.Set<Address>().Where(x => x.Id == id).FirstOrDefaultAsync();
			//UPDATING OTHER PARTS FEELS LOGICALLY WRONG
			fromdb.UpdateDate = DateTime.UtcNow;
			fromdb.UpdateUserId = address.UpdateUserId;
			fromdb.Address1 = address.Address1;
			fromdb.Address2 = address.Address2;
			fromdb.Country = address.Country;
			fromdb.City = address.City;
			fromdb.County = address.County;
			fromdb.PostalCode = address.PostalCode;

			await dbContext.SaveChangesAsync();
		}


		[HttpDelete("{id}")]
		public async Task Delete(int id)
		{
			var fromdb = await dbContext.Set<Account>().Where(x => x.Id == id).FirstOrDefaultAsync();
			fromdb.IsActive = false;
			fromdb.UpdateDate = DateTime.UtcNow;
			await dbContext.SaveChangesAsync();
		}
	}
}

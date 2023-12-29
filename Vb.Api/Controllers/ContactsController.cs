using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Data.Entity;
using Vb.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VbApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ContactsController : ControllerBase
	{
		private readonly VbDbContext dbContext;
		public ContactsController(VbDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpGet]
		public async Task<List<Contact>> Get()
		{
			return await dbContext.Set<Contact>()
				.ToListAsync();
		}


		[HttpGet("{id}")]
		public async Task<Contact> GetById(int id)
		{
			var contact = await dbContext.Set<Contact>()
			.Where(x => x.Id == id).FirstOrDefaultAsync();

			return contact;
		}

		[HttpPost]
		public async Task Post([FromBody] Contact contact)
		{
			await dbContext.Set<Contact>().AddAsync(contact);
			await dbContext.SaveChangesAsync();
		}


		[HttpPut("{id}")]
		public async Task Put(int id, [FromBody] Contact contact)
		{
			var fromdb = await dbContext.Set<Contact>().Where(x => x.Id == id).FirstOrDefaultAsync();
			//UPDATING OTHER PARTS FEELS LOGICALLY WRONG
			fromdb.UpdateDate = DateTime.UtcNow;
			fromdb.UpdateUserId = contact.UpdateUserId;
			fromdb.Information = contact.Information;
			fromdb.ContactType = contact.ContactType;
			await dbContext.SaveChangesAsync();
		}


		[HttpDelete("{id}")]
		public async Task Delete(int id)
		{
			var fromdb = await dbContext.Set<Contact>().Where(x => x.Id == id).FirstOrDefaultAsync();
			fromdb.IsActive = false;
			fromdb.UpdateDate = DateTime.UtcNow;
			await dbContext.SaveChangesAsync();
		}
	}
}

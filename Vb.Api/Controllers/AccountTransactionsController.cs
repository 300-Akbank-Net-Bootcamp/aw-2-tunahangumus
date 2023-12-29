using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Data.Entity;
using Vb.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VbApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountTransactionsController : ControllerBase
	{
		private readonly VbDbContext dbContext;
		public AccountTransactionsController(VbDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpGet]
		public async Task<List<AccountTransaction>> Get()
		{
			return await dbContext.Set<AccountTransaction>()
				.ToListAsync();
		}


		[HttpGet("{id}")]
		public async Task<AccountTransaction> GetById(int id)
		{
			var account_transaction = await dbContext.Set<AccountTransaction>()
			.Where(x => x.Id == id).FirstOrDefaultAsync();

			return account_transaction;
		}

		[HttpPost]
		public async Task Post([FromBody] AccountTransaction account_transaction)
		{
			await dbContext.Set<AccountTransaction>().AddAsync(account_transaction);
			await dbContext.SaveChangesAsync();
		}


		[HttpPut("{id}")]
		public async Task Put(int id, [FromBody] AccountTransaction account_transaction)
		{
			var fromdb = await dbContext.Set<AccountTransaction>().Where(x => x.Id == id).FirstOrDefaultAsync();
			fromdb.UpdateDate = DateTime.UtcNow;
			fromdb.Amount = account_transaction.Amount;
			fromdb.Description = account_transaction.Description;

			fromdb.UpdateUserId = account_transaction.UpdateUserId;

			await dbContext.SaveChangesAsync();
		}


		[HttpDelete("{id}")]
		public async Task Delete(int id)
		{
			var fromdb = await dbContext.Set<AccountTransaction>().Where(x => x.Id == id).FirstOrDefaultAsync();
			fromdb.IsActive = false;
			fromdb.UpdateDate = DateTime.UtcNow;
			await dbContext.SaveChangesAsync();
		}
	}
}

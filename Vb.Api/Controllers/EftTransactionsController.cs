using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vb.Data.Entity;
using Vb.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VbApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EftTransactionsController : ControllerBase
	{
		private readonly VbDbContext dbContext;
		public EftTransactionsController(VbDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpGet]
		public async Task<List<EftTransaction>> Get()
		{
			return await dbContext.Set<EftTransaction>()
				.ToListAsync();
		}


		[HttpGet("{id}")]
		public async Task<EftTransaction> GetById(int id)
		{
			var eftTransaction = await dbContext.Set<EftTransaction>()
			.Where(x => x.Id == id).FirstOrDefaultAsync();

			return eftTransaction;
		}

		[HttpPost]
		public async Task Post([FromBody] EftTransaction eftTransaction)
		{
			await dbContext.Set<EftTransaction>().AddAsync(eftTransaction);
			await dbContext.SaveChangesAsync();
		}


		[HttpPut("{id}")]
		public async Task Put(int id, [FromBody] EftTransaction eftTransaction)
		{
			var fromdb = await dbContext.Set<EftTransaction>().Where(x => x.Id == id).FirstOrDefaultAsync();
			//UPDATING OTHER PARTS FEELS LOGICALLY WRONG Description, senderaccount, IBAN .....
			fromdb.UpdateDate = DateTime.UtcNow;
			fromdb.UpdateUserId = eftTransaction.UpdateUserId;

			await dbContext.SaveChangesAsync();
		}


		[HttpDelete("{id}")]
		public async Task Delete(int id)
		{
			var fromdb = await dbContext.Set<EftTransaction>().Where(x => x.Id == id).FirstOrDefaultAsync();
			fromdb.IsActive = false;
			fromdb.UpdateDate = DateTime.UtcNow;
			await dbContext.SaveChangesAsync();
		}
	}
}

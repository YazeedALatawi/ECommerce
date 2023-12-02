namespace ECommerce.Models.Repositories
{
	public class ReceiptRepo : IOperations<Receipt>
	{
		ApplicationDBContext db;
		public ReceiptRepo(ApplicationDBContext db)
		{
			this.db = db;
		}

		public void Add(Receipt entity)
		{
			db.Receipts.Add(entity);
			db.SaveChanges();
		}

		public void delete(int id)
		{
			var result = Find(id);
			db.Receipts.Remove(result);
			db.SaveChanges();
		}

		public Receipt Find(int id)
		{
			var result = db.Receipts.SingleOrDefault(x => x.Id == id);
			return result;
		}

		public IList<Receipt> List()
		{
			return db.Receipts.ToList();
		}

		public void update(Receipt entity)
		{
			db.Receipts.Update(entity);
			db.SaveChanges();
		}
	}
}

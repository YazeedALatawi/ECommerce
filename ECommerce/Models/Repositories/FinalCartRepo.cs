namespace ECommerce.Models.Repositories
{
	public class FinalCartRepo : IOperations<FinalCart>
	{
		ApplicationDBContext db;
		public FinalCartRepo(ApplicationDBContext db)
		{
			this.db = db;
		}

		public void Add(FinalCart entity)
		{
			db.FinalCarts.Add(entity);
			db.SaveChanges();
		}

		public void delete(int id)
		{
			var result = Find(id);
			db.FinalCarts.Remove(result);
			db.SaveChanges();
		}

		public FinalCart Find(int id)
		{
			var result = db.FinalCarts.SingleOrDefault(x => x.Id == id);
			return result;
		}

		public IList<FinalCart> List()
		{
			return db.FinalCarts.ToList();
		}

		public void update(FinalCart entity)
		{
			db.FinalCarts.Update(entity);
			db.SaveChanges();
		}
	}
}

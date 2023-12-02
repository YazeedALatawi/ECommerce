namespace ECommerce.Models.Repositories
{
	public class OrderRepo : IOperations<Order>
	{
		ApplicationDBContext db;
		public OrderRepo(ApplicationDBContext db)
		{
			this.db = db;
		}

		public void Add(Order entity)
		{
			db.Orders.Add(entity);
			db.SaveChanges();
		}

		public void delete(int id)
		{
			var result = Find(id);
			db.Orders.Remove(result);
			db.SaveChanges();
		}

		public Order Find(int id)
		{
			var result = db.Orders.SingleOrDefault(o => o.Id == id);
			return result;
		}

		public IList<Order> List()
		{
			return db.Orders.ToList();
		}

		public void update(Order entity)
		{
			db.Orders.Update(entity);
			db.SaveChanges();
		}
	}
}

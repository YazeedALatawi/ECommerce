namespace ECommerce.Models.Repositories
{
	public class ShippingRepo : IOperations<Shipping>
	{
		ApplicationDBContext db;
		public ShippingRepo(ApplicationDBContext db)
		{
			this.db = db;
		}

		public void Add(Shipping entity)
		{
			db.Shippings.Add(entity);
			db.SaveChanges();
		}

		public void delete(int id)
		{
			var result = Find(id);
			db.Shippings.Remove(result);
			db.SaveChanges();
		}

		public Shipping Find(int id)
		{
			var result = db.Shippings.SingleOrDefault(x => x.Id == id);
			return result;
		}

        public IList<Shipping> findAllByCartId(int id)
        {
            throw new NotImplementedException();
        }

        public bool findByIdProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Shipping findByIdUser(string id)
        {
            throw new NotImplementedException();
        }

        public Shipping FindString(string id)
        {
            throw new NotImplementedException();
        }

        public IList<Shipping> List()
		{
			return db.Shippings.ToList();
		}

		public void update(Shipping entity)
		{
			db.Shippings.Update(entity);
			db.SaveChanges();
		}
	}
}

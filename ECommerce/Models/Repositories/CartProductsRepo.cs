namespace ECommerce.Models.Repositories
{
	public class CartProductsRepo : IOperations<CartProducts>
	{
		ApplicationDBContext db;
		public CartProductsRepo(ApplicationDBContext db)
		{
			this.db = db;
		}

		public void Add(CartProducts entity)
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

		public CartProducts Find(int id)
		{
			var result = db.FinalCarts.SingleOrDefault(x => x.Id == id);
			return result;
		}

        public IList<CartProducts> findAllByCartId(int id)
        {
			var result = db.FinalCarts.Where(a => a.cartId == id);
			return result.ToList();
        }

		// note
        public bool findByIdProduct(int id)
        {
			var result = db.FinalCarts.FirstOrDefault(x => x.productId == id);
			if(result != null)
			{
				return true;
			}

			return false;
        }

        public CartProducts findByIdUser(string id)
        {
            throw new NotImplementedException();
        }

        public CartProducts FindString(string id)
        {
            throw new NotImplementedException();
        }

        public IList<CartProducts> List()
		{
			return db.FinalCarts.ToList();
		}

        public IList<CartProducts> Search(string serach)
        {
            throw new NotImplementedException();
        }

        public void update(CartProducts entity)
		{
			db.FinalCarts.Update(entity);
			db.SaveChanges();
		}
	}
}

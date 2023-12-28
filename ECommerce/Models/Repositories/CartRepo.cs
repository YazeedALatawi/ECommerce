using Azure;
using Microsoft.CodeAnalysis;

namespace ECommerce.Models.Repositories
{
	public class CartRepo : IOperations<Cart>
	{
		ApplicationDBContext db;
		public CartRepo(ApplicationDBContext _db) 
		{
			db = _db;
		}
		public void Add(Cart entity)
		{
			db.Carts.Add(entity);
			db.SaveChanges();
		}

		public void delete(int id)
		{
			db.Carts.Remove(Find(id));
			db.SaveChanges();
		}

		public Cart Find(int id)
		{
			var result = db.Carts.FirstOrDefault(x => x.Id == id);
			return result;
		}

		public IList<Cart> List()
		{
			return db.Carts.ToList();
		}

		public void update(Cart entity)
		{
			db.Carts.Update(entity);
			db.SaveChanges();
		}

		public Cart findByIdUser(string id)
		{
			var cart = db.Carts.FirstOrDefault(x => x.userId == id && x.isDelete == false);
			return cart;
		}


        public IList<Cart> findAllByCartId(int id)
        {
            throw new NotImplementedException();
        }

        public Cart FindString(string id)
        {
            throw new NotImplementedException();
        }

        public bool findByIdProduct(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Cart> Search(string serach)
        {
            throw new NotImplementedException();
        }
    }
}

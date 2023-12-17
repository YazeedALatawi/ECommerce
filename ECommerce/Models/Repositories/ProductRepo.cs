using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Repositories
{
	public class ProductRepo : IOperations<Product>
	{
		ApplicationDBContext db;
		public ProductRepo(ApplicationDBContext db)
		{
			this.db = db;
		}

		public void Add(Product entity)
		{
			db.Products.Add(entity);
			db.SaveChanges();

		}

		public void delete(int id)
		{
			var result = Find(id);
			db.Products.Remove(result);
			db.SaveChanges();
		}

		public Product Find(int id)
		{
			var result = db.Products.Include(z => z.category).SingleOrDefault(x => x.Id == id);
			return result;
		}

        public IList<Product> findAllByCartId(int id)
        {
            throw new NotImplementedException();
        }

        public bool findByIdProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Product findByIdUser(string id)
        {
            throw new NotImplementedException();
        }

        public Product FindString(string id)
        {
            throw new NotImplementedException();
        }

        public IList<Product> List()
		{
			return db.Products.Include(x=> x.category).ToList();
		}

		public void update(Product entity)
		{
			db.Products.Update(entity);
			db.SaveChanges();
		}
	}
}

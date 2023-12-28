using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Repositories
{
	public class productOptionsRepo : IOperations<productOptions>
	{
		ApplicationDBContext _db;
		public productOptionsRepo(ApplicationDBContext db)
		{
			_db= db;
		}
		public void Add(productOptions entity)
		{
			_db.ProductOptions.Add(entity);
			_db.SaveChanges();
		}

		public void delete(int id)
		{
			_db.ProductOptions.Remove(Find(id));
			_db.SaveChanges();
		}

		public productOptions Find(int id)
		{
			var productOption = _db.ProductOptions.Find(id);
			return productOption;
		}

		public IList<productOptions> findAllByCartId(int id)
		{
			throw new NotImplementedException();
		}

		public bool findByIdProduct(int id)
		{
			throw new NotImplementedException();
		}

		public productOptions findByIdUser(string id)
		{
			throw new NotImplementedException();
		}

		public productOptions FindString(string id)
		{
			throw new NotImplementedException();
		}

		public IList<productOptions> List()
		{
			return _db.ProductOptions.Include(x => x.Options).ToList();
		}

        public IList<productOptions> Search(string serach)
        {
            throw new NotImplementedException();
        }

        public void update(productOptions entity)
		{
			_db.ProductOptions.Update(entity);
			_db.SaveChanges();
		}
	}
}

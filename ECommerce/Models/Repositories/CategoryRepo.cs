﻿using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Repositories
{
	public class CategoryRepo : IOperations<Category>
	{
		ApplicationDBContext db;

		public CategoryRepo(ApplicationDBContext db)
		{
			this.db = db;
		}
		public void Add(Category entity)
		{
			db.Categories.Add(entity);
			db.SaveChanges();
		}

		public void delete(int id)
		{
			var result = Find(id);
			db.Categories.Remove(result);
			db.SaveChanges();
		}

		public Category Find(int id)
		{
			var result = db.Categories.SingleOrDefault(c => c.Id == id);
			return result;
		}

        public IList<Category> findAllByCartId(int id)
        {
            throw new NotImplementedException();
        }

        public bool findByIdProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Category findByIdUser(string id)
        {
            throw new NotImplementedException();
        }

        public Category FindString(string id)
        {
            throw new NotImplementedException();
        }

        public IList<Category> List()
		{ 
			return db.Categories.ToList();
		}

        public IList<Category> Search(string serach)
        {
			var res = db.Categories.Where(a => a.Id.ToString().Contains(serach) || a.Name.Contains(serach)).ToList();
			return res;
        }

        public void update(Category entity)
		{
			db.Categories.Update(entity);
			db.SaveChanges();
		}
	}
}

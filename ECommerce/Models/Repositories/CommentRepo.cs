﻿namespace ECommerce.Models.Repositories
{
	public class CommentRepo : IOperations<Comment>
	{
		ApplicationDBContext db;
		public CommentRepo(ApplicationDBContext db)
		{
			this.db = db;
		}

		public void Add(Comment entity)
		{
			db.Comments.Add(entity);
			db.SaveChanges();
		}

		public void delete(int id)
		{
			var result = Find(id);
			db.Comments.Remove(result);
			db.SaveChanges();
		}

		public Comment Find(int id)
		{
			var result = db.Comments.SingleOrDefault(x => x.Id == id);
			return result;
		}

        public IList<Comment> findAllByCartId(int id)
        {
            throw new NotImplementedException();
        }

        public bool findByIdProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Comment findByIdUser(string id)
        {
            throw new NotImplementedException();
        }

        public Comment FindString(string id)
        {
            throw new NotImplementedException();
        }

        public IList<Comment> List()
		{
			return db.Comments.ToList();
		}

        public IList<Comment> Search(string serach)
        {
            throw new NotImplementedException();
        }

        public void update(Comment entity)
		{
			db.Update(entity);
			db.SaveChanges();
		}
	}
}

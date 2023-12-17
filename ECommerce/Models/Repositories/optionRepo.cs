using Microsoft.EntityFrameworkCore;

namespace ECommerce.Models.Repositories
{
    public class optionRepo : IOperations<options>
    {
        ApplicationDBContext _db;
        public optionRepo(ApplicationDBContext db)
        {
            _db = db;
        }
        public void Add(options entity)
        {
            _db.options.Add(entity);
            _db.SaveChanges();
        }

        public void delete(int id)
        {
            _db.options.Remove(Find(id));
            _db.SaveChanges();
        }

        public options Find(int id)
        {
            var productOption = _db.options.Find(id);
            return productOption;
        }

        public IList<options> findAllByCartId(int id)
        {
            throw new NotImplementedException();
        }

        public bool findByIdProduct(int id)
        {
            throw new NotImplementedException();
        }

        public options findByIdUser(string id)
        {
            throw new NotImplementedException();
        }

        public options FindString(string id)
        {
            throw new NotImplementedException();
        }

        public IList<options> List()
        {
            return _db.options.ToList();
        }

        public void update(options entity)
        {
            _db.options.Update(entity);
            _db.SaveChanges();
        }
    }
}


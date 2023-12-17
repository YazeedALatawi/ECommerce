namespace ECommerce.Models.Repositories
{
    public class UserRepo : IOperations<User>
    {
        ApplicationDBContext _db;
        public UserRepo(ApplicationDBContext db)
        {
            _db = db;
        }
        public void Add(User entity)
        {
            throw new NotImplementedException();
        }

        public void delete(int id)
        {
            throw new NotImplementedException();
        }

        public User Find(int id)
        {
            throw new NotImplementedException();
        }

        public IList<User> findAllByCartId(int id)
        {
            throw new NotImplementedException();
        }

        public bool findByIdProduct(int id)
        {
            throw new NotImplementedException();
        }

        public User findByIdUser(string id)
        {
            return _db.Users.FirstOrDefault(x => x.Id == id);
        }

        public User FindString(string id)
        {
            var user = _db.Users.FirstOrDefault(x => x.Id == id);
            return user;
        }

        public IList<User> List()
        {
            throw new NotImplementedException();
        }

        public void update(User entity)
        {
            _db.Users.Update(entity);
            _db.SaveChanges();
        }
    }
}

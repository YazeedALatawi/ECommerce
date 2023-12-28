namespace ECommerce.Models.Repositories
{
    public class CartPrdouctOptionsRepo : IOperations<CartPrdouctOptions>
    {
        ApplicationDBContext _db;
        public CartPrdouctOptionsRepo(ApplicationDBContext db)
        {
            _db = db;
        }

        public void Add(CartPrdouctOptions entity)
        {
            _db.CartProductsOptions.Add(entity);
            _db.SaveChanges();
        }

        public void delete(int id)
        {
            var model = Find(id);
            _db.CartProductsOptions.Remove(model);
            _db.SaveChanges();
        }

        public CartPrdouctOptions Find(int id)
        {
            var model = _db.CartProductsOptions.FirstOrDefault(x => x.Id == id);
            return model;
        }

        public IList<CartPrdouctOptions> findAllByCartId(int id)
        {
            throw new NotImplementedException();
        }

        public bool findByIdProduct(int id)
        {
            throw new NotImplementedException();
        }

        public CartPrdouctOptions findByIdUser(string id)
        {
            throw new NotImplementedException();
        }

        public CartPrdouctOptions FindString(string id)
        {
            throw new NotImplementedException();
        }

        public IList<CartPrdouctOptions> List()
        {
            return _db.CartProductsOptions.ToList();
        }

        public IList<CartPrdouctOptions> Search(string serach)
        {
            throw new NotImplementedException();
        }

        public void update(CartPrdouctOptions entity)
        {
            _db.CartProductsOptions.Update(entity);
            _db.SaveChanges();
        }
    }
}

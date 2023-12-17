namespace ECommerce.Models.Repositories
{
	public interface IOperations<Entity>
	{
		IList<Entity> List();
		Entity Find(int id);
		void Add(Entity entity);
		void delete(int id);
		void update(Entity entity);
		Entity findByIdUser(string id);
		IList<Entity> findAllByCartId(int id);
        Entity FindString(string id);
		bool findByIdProduct(int id);
    }
}

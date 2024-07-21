namespace DatabaseADO.Repositories;

public interface IRepository<T> where T : class
{
    int Create(T entity, string identifier);
    T GetById(int id, string identifier);
    void Update(T entity, string identifier);
    void Delete(int id, string identifier);
}

using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace DatabaseADO.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly string _connectionString;
    protected readonly ILogger<Repository<T>> _logger;

    protected Repository(string connectionString, ILogger<Repository<T>> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public virtual int Create(T entity, string identifier)
    {
        throw new NotImplementedException("Create method not implemented.");
    }

    public virtual T GetById(int id, string identifier)
    {
        throw new NotImplementedException("GetById method not implemented.");
    }

    public virtual void Update(T entity, string identifier)
    {
        throw new NotImplementedException("Update method not implemented.");
    }

    public virtual void Delete(int id, string identifier)
    {
        throw new NotImplementedException("Delete method not implemented.");
    }

    protected void LogError(Exception ex, string procedure, string identifier)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var commandText = @"INSERT INTO [LearnDB].[ErrorLogs] (ErrorMessage, ErrorProcedure, ErrorLine, Identifier)
                                VALUES (@ErrorMessage, @ErrorProcedure, @ErrorLine, @Identifier)";
            using (var command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                command.Parameters.AddWithValue("@ErrorProcedure", procedure);
                command.Parameters.AddWithValue("@ErrorLine", ex.StackTrace);
                command.Parameters.AddWithValue("@Identifier", identifier);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}

using DatabaseADO.Entities;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace DatabaseADO.Repositories;

public class ComputerRepository : Repository<Computer>
{
    public ComputerRepository(string connectionString, ILogger<ComputerRepository> logger)
        : base(connectionString, logger)
    {
    }

    public override int Create(Computer computer, string identifier)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var commandText = @"INSERT INTO [LearnDB].[Computers] (Motherboard, CPUcores, HasWiFi, HasLTE, ReleaseDate, Price, VideoCard)
                                VALUES (@Motherboard, @CPUcores, @HasWiFi, @HasLTE, @ReleaseDate, @Price, @VideoCard);
                                SELECT CAST(SCOPE_IDENTITY() as int)";
            using (var command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@Motherboard", computer.Motherboard);
                command.Parameters.AddWithValue("@CPUcores", computer.CPUcores);
                command.Parameters.AddWithValue("@HasWiFi", computer.HasWiFi);
                command.Parameters.AddWithValue("@HasLTE", computer.HasLTE);
                command.Parameters.AddWithValue("@ReleaseDate", computer.ReleaseDate);
                command.Parameters.AddWithValue("@Price", computer.Price);
                command.Parameters.AddWithValue("@VideoCard", computer.VideoCard);

                connection.Open();
                try
                {
                    var result = (int)command.ExecuteScalar();
                    return result;
                }
                catch (Exception ex)
                {
                    LogError(ex, "CreateComputer", identifier);
                    throw;
                }
            }
        }
    }

    public override Computer GetById(int computerId, string identifier)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var commandText = @"SELECT * FROM [LearnDB].[Computers] WHERE ComputerId = @ComputerId";
            using (var command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@ComputerId", computerId);

                connection.Open();
                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Computer
                            {
                                ComputerId = reader.GetInt32(reader.GetOrdinal("ComputerId")),
                                Motherboard = reader.GetString(reader.GetOrdinal("Motherboard")),
                                CPUcores = reader.GetInt32(reader.GetOrdinal("CPUcores")),
                                HasWiFi = reader.GetBoolean(reader.GetOrdinal("HasWiFi")),
                                HasLTE = reader.GetBoolean(reader.GetOrdinal("HasLTE")),
                                ReleaseDate = reader.GetDateTime(reader.GetOrdinal("ReleaseDate")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                VideoCard = reader.GetString(reader.GetOrdinal("VideoCard"))
                            };
                        }
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex, "GetComputerById", identifier);
                    throw;
                }
            }
        }
    }

    public override void Update(Computer computer, string identifier)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var commandText = @"UPDATE [LearnDB].[Computers] SET 
                                Motherboard = @Motherboard,
                                CPUcores = @CPUcores,
                                HasWiFi = @HasWiFi,
                                HasLTE = @HasLTE,
                                ReleaseDate = @ReleaseDate,
                                Price = @Price,
                                VideoCard = @VideoCard
                              WHERE ComputerId = @ComputerId";
            using (var command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@ComputerId", computer.ComputerId);
                command.Parameters.AddWithValue("@Motherboard", computer.Motherboard);
                command.Parameters.AddWithValue("@CPUcores", computer.CPUcores);
                command.Parameters.AddWithValue("@HasWiFi", computer.HasWiFi);
                command.Parameters.AddWithValue("@HasLTE", computer.HasLTE);
                command.Parameters.AddWithValue("@ReleaseDate", computer.ReleaseDate);
                command.Parameters.AddWithValue("@Price", computer.Price);
                command.Parameters.AddWithValue("@VideoCard", computer.VideoCard);

                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    LogError(ex, "UpdateComputer", identifier);
                    throw;
                }
            }
        }
    }

    public override void Delete(int computerId, string identifier)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var commandText = @"DELETE FROM [LearnDB].[Computers] WHERE ComputerId = @ComputerId";
            using (var command = new SqlCommand(commandText, connection))
            {
                command.Parameters.AddWithValue("@ComputerId", computerId);

                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    LogError(ex, "DeleteComputer", identifier);
                    throw;
                }
            }
        }
    }
}

using DatabaseADO.Entities;
using DatabaseADO.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DatabaseADO;

public class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = ConfigureServices();
        var computerRepository = serviceProvider.GetService<IRepository<Computer>>();

        // Example CRUD operations
        string identifier = "ExampleIdentifier";

        try
        {


            var newComputer = new Computer
            {
                Motherboard = "New Motherboard",
                CPUcores = 4,
                HasWiFi = true,
                HasLTE = false,
                ReleaseDate = DateTime.Now,
                Price = 1200.00m,
                VideoCard = "New Video Card"
            };

            var id = computerRepository.Create(newComputer, identifier);
            Console.WriteLine($"New Computer ID: {id}");
            var computer = computerRepository.GetById(id, identifier);
            Console.WriteLine($"Computer Details: {computer?.Motherboard}");

            computer.Motherboard = "Updated Motherboard";
            computerRepository.Update(computer, identifier);
            Console.WriteLine("Computer updated.");

            computerRepository.Delete(id, identifier);
            Console.WriteLine("Computer deleted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
    private static IServiceProvider ConfigureServices()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging(configure => configure.AddConsole());
        services.AddScoped<IRepository<Computer>>(provider =>
            new ComputerRepository(
                configuration.GetConnectionString("DefaultConnection"),
                provider.GetRequiredService<ILogger<ComputerRepository>>()
            )
        );

        return services.BuildServiceProvider();
    }
}

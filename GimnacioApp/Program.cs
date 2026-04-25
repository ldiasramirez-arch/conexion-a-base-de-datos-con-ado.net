// Program.cs
using GimnasioApp.Database;
using GimnasioApp.Repository;
using GimnasioApp.Screens;
using GimnasioApp.Services;

namespace GimnasioApp;

class Program
{
    static void Main(string[] args)
    {
        // 1. Configurar la conexión y crear la base de datos/tabla si no existen
        var dbConfig = new DatabaseConfig();
        dbConfig.InitializeDatabase();

        // 2. Conectar las capas mediante inyección de dependencias
        var miembroRepository = new MiembroRepository(dbConfig);
        var miembroService = new MiembroService(miembroRepository);
        var menuPrincipal = new MenuPrincipal(miembroService);

        // 3. Lanzar la aplicación mostrando el menú principal
        menuPrincipal.MostrarMenu();
    }
}

// Database/DatabaseConfig.cs
using System.Data.SQLite;

namespace GimnasioApp.Database;

public class DatabaseConfig
{
    private readonly string _connectionString;

    public DatabaseConfig()
    {
        // Ruta donde se guardará la base de datos SQLite (en el mismo directorio del proyecto)
        string dbPath = Path.Combine(Directory.GetCurrentDirectory(), "gimnasio.db");
        _connectionString = $"Data Source={dbPath};Version=3;";
    }

    // Método para obtener la conexión ADO.NET
    public SQLiteConnection GetConnection()
    {
        return new SQLiteConnection(_connectionString);
    }

    // Método para inicializar la base de datos (crear tabla Miembro si no existe)
    public void InitializeDatabase()
    {
        using var connection = GetConnection();
        connection.Open();

        using var command = new SQLiteCommand(connection);
        // Sentencia SQL para crear la tabla
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Miembro (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                nombre_completo TEXT NOT NULL,
                cedula TEXT NOT NULL UNIQUE,
                telefono TEXT NOT NULL
            );
        ";

        command.ExecuteNonQuery();
        connection.Close();
    }
}
// Repository/MiembroRepository.cs
using GimnasioApp.Database;
using GimnasioApp.Models;
using System.Data.SQLite;

namespace GimnasioApp.Repository;

public class MiembroRepository
{
    private readonly DatabaseConfig _dbConfig;

    // Inyección de dependencias por constructor
    public MiembroRepository(DatabaseConfig dbConfig)
    {
        _dbConfig = dbConfig;
    }

    // 1. Insertar nuevo miembro
    public int Insertar(Miembro miembro)
    {
        using var connection = _dbConfig.GetConnection();
        connection.Open();

        using var command = new SQLiteCommand(connection);
        command.CommandText = @"
            INSERT INTO Miembro (nombre_completo, cedula, telefono)
            VALUES (@NombreCompleto, @Cedula, @Telefono);
        ";

        // Agregar parámetros para evitar inyección SQL
        command.Parameters.AddWithValue("@NombreCompleto", miembro.NombreCompleto);
        command.Parameters.AddWithValue("@Cedula", miembro.Cedula);
        command.Parameters.AddWithValue("@Telefono", miembro.Telefono);

        int filasAfectadas = command.ExecuteNonQuery();
        connection.Close();
        return filasAfectadas;
    }

    // 2. Listar todos los miembros
    public List<Miembro> ListarTodos()
    {
        List<Miembro> miembros = new();

        using var connection = _dbConfig.GetConnection();
        connection.Open();

        using var command = new SQLiteCommand("SELECT * FROM Miembro;", connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            miembros.Add(new Miembro
            {
                Id = reader.GetInt32(0),
                NombreCompleto = reader.GetString(1),
                Cedula = reader.GetString(2),
                Telefono = reader.GetString(3)
            });
        }

        connection.Close();
        return miembros;
    }

    // 3. Buscar miembro por cédula
    public Miembro? BuscarPorCedula(string cedula)
    {
        Miembro? miembro = null;

        using var connection = _dbConfig.GetConnection();
        connection.Open();

        using var command = new SQLiteCommand(connection);
        command.CommandText = "SELECT * FROM Miembro WHERE cedula = @Cedula;";
        command.Parameters.AddWithValue("@Cedula", cedula);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            miembro = new Miembro
            {
                Id = reader.GetInt32(0),
                NombreCompleto = reader.GetString(1),
                Cedula = reader.GetString(2),
                Telefono = reader.GetString(3)
            };
        }

        connection.Close();
        return miembro;
    }

    // 4. Actualizar teléfono de un miembro
    public int ActualizarTelefono(int id, string nuevoTelefono)
    {
        using var connection = _dbConfig.GetConnection();
        connection.Open();

        using var command = new SQLiteCommand(connection);
        command.CommandText = @"
            UPDATE Miembro
            SET telefono = @NuevoTelefono
            WHERE Id = @Id;
        ";

        command.Parameters.AddWithValue("@NuevoTelefono", nuevoTelefono);
        command.Parameters.AddWithValue("@Id", id);

        int filasAfectadas = command.ExecuteNonQuery();
        connection.Close();
        return filasAfectadas;
    }

    // 5. Eliminar miembro
    public int Eliminar(int id)
    {
        using var connection = _dbConfig.GetConnection();
        connection.Open();

        using var command = new SQLiteCommand(connection);
        command.CommandText = "DELETE FROM Miembro WHERE Id = @Id;";
        command.Parameters.AddWithValue("@Id", id);

        int filasAfectadas = command.ExecuteNonQuery();
        connection.Close();
        return filasAfectadas;
    }
}
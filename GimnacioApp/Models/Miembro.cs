// Models/Miembro.cs
namespace GimnasioApp.Models;

public class Miembro
{
    // Clave primaria autoincremental para identificar cada miembro
    public int Id { get; set; }
    
    // Nombre completo del miembro (no puede estar vacío)
    public string NombreCompleto { get; set; } = string.Empty;
    
    // Cédula del miembro (única por registro)
    public string Cedula { get; set; } = string.Empty;
    
    // Teléfono del miembro
    public string Telefono { get; set; } = string.Empty;
}
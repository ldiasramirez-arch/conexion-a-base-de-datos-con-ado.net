// Services/MiembroService.cs
using GimnasioApp.Models;
using GimnasioApp.Repository;

namespace GimnasioApp.Services;

public class MiembroService
{
    private readonly MiembroRepository _miembroRepository;

    // Inyección de dependencias por constructor
    public MiembroService(MiembroRepository miembroRepository)
    {
        _miembroRepository = miembroRepository;
    }

    // Registrar miembro: validar que la cédula no exista
    public bool RegistrarMiembro(Miembro miembro)
    {
        var miembroExistente = _miembroRepository.BuscarPorCedula(miembro.Cedula);
        if (miembroExistente != null)
        {
            return false; // Cédula ya registrada
        }

        _miembroRepository.Insertar(miembro);
        return true;
    }

    // Listar todos los miembros
    public List<Miembro> ObtenerTodosLosMiembros()
    {
        return _miembroRepository.ListarTodos();
    }

    // Buscar por cédula
    public Miembro? BuscarMiembroPorCedula(string cedula)
    {
        return _miembroRepository.BuscarPorCedula(cedula);
    }

    // Actualizar teléfono: validar que el miembro exista
    public bool ActualizarTelefonoMiembro(int id, string nuevoTelefono)
    {
        var filasAfectadas = _miembroRepository.ActualizarTelefono(id, nuevoTelefono);
        return filasAfectadas > 0;
    }

    // Eliminar miembro: validar que exista
    public bool EliminarMiembro(int id)
    {
        var filasAfectadas = _miembroRepository.Eliminar(id);
        return filasAfectadas > 0;
    }
}
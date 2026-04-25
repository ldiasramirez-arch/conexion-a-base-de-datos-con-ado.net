// Screens/MenuPrincipal.cs
using GimnasioApp.Models;
using GimnasioApp.Services;
using Spectre.Console;

namespace GimnasioApp.Screens;

public class MenuPrincipal
{
    private readonly MiembroService _miembroService;

    // Inyección de dependencias por constructor
    public MenuPrincipal(MiembroService miembroService)
    {
        _miembroService = miembroService;
    }

    public void MostrarMenu()
    {
        // Limpiar la consola y mostrar título
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Gimnasio App")
                .Centered()
                .Color(Color.Blue));

        // Menú interactivo
        while (true)
        {
            var opcion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Selecciona una opción[/]")
                    .PageSize(6)
                    .AddChoices(new[] {
                        "Registrar nuevo miembro",
                        "Listar todos los miembros",
                        "Buscar miembro por cédula",
                        "Actualizar teléfono de miembro",
                        "Eliminar miembro",
                        "Salir"
                    }));

            switch (opcion)
            {
                case "Registrar nuevo miembro":
                    RegistrarMiembroScreen();
                    break;
                case "Listar todos los miembros":
                    ListarMiembrosScreen();
                    break;
                case "Buscar miembro por cédula":
                    BuscarMiembroScreen();
                    break;
                case "Actualizar teléfono de miembro":
                    ActualizarTelefonoScreen();
                    break;
                case "Eliminar miembro":
                    EliminarMiembroScreen();
                    break;
                case "Salir":
                    AnsiConsole.MarkupLine("[green]¡Hasta luego![/]");
                    return;
            }
        }
    }

    // Pantalla de registro
    private void RegistrarMiembroScreen()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[blue]=== REGISTRAR NUEVO MIEMBRO ===[/]\n");

        var miembro = new Miembro
        {
            NombreCompleto = AnsiConsole.Ask<string>("Ingresa el [yellow]nombre completo[/]:"),
            Cedula = AnsiConsole.Ask<string>("Ingresa la [yellow]cédula[/]:"),
            Telefono = AnsiConsole.Ask<string>("Ingresa el [yellow]teléfono[/]:")
        };

        bool exito = _miembroService.RegistrarMiembro(miembro);
        if (exito)
        {
            AnsiConsole.MarkupLine("[green]Miembro registrado correctamente![/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Error: La cédula ya está registrada.[/]");
        }

        AnsiConsole.MarkupLine("\nPresiona cualquier tecla para continuar...");
        Console.ReadKey();
    }

    // Pantalla de listado
    private void ListarMiembrosScreen()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[blue]=== LISTA DE MIEMBROS ===[/]\n");

        var miembros = _miembroService.ObtenerTodosLosMiembros();
        if (miembros.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No hay miembros registrados aún.[/]");
        }
        else
        {
            // Crear tabla con Spectre.Console
            var tabla = new Table();
            tabla.AddColumn("[cyan]ID[/]");
            tabla.AddColumn("[cyan]Nombre Completo[/]");
            tabla.AddColumn("[cyan]Cédula[/]");
            tabla.AddColumn("[cyan]Teléfono[/]");

            foreach (var miembro in miembros)
            {
                tabla.AddRow(
                    miembro.Id.ToString(),
                    miembro.NombreCompleto,
                    miembro.Cedula,
                    miembro.Telefono);
            }

            AnsiConsole.Write(tabla);
        }

        AnsiConsole.MarkupLine("\nPresiona cualquier tecla para continuar...");
        Console.ReadKey();
    }

    // Pantalla de búsqueda
    private void BuscarMiembroScreen()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[blue]=== BUSCAR MIEMBRO POR CÉDULA ===[/]\n");

        string cedula = AnsiConsole.Ask<string>("Ingresa la [yellow]cédula[/] a buscar:");
        var miembro = _miembroService.BuscarMiembroPorCedula(cedula);

        if (miembro != null)
        {
            AnsiConsole.MarkupLine("\n[green]Miembro encontrado:[/]");
            AnsiConsole.Write(new Panel(
                $"ID: {miembro.Id}\n" +
                $"Nombre: {miembro.NombreCompleto}\n" +
                $"Cédula: {miembro.Cedula}\n" +
                $"Teléfono: {miembro.Telefono}")
                .BorderColor(Color.Green));
        }
        else
        {
            AnsiConsole.MarkupLine("[red]No se encontró ningún miembro con esa cédula.[/]");
        }

        AnsiConsole.MarkupLine("\nPresiona cualquier tecla para continuar...");
        Console.ReadKey();
    }

    // Pantalla de actualización de teléfono
    private void ActualizarTelefonoScreen()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[blue]=== ACTUALIZAR TELÉFONO DE MIEMBRO ===[/]\n");

        string cedula = AnsiConsole.Ask<string>("Ingresa la [yellow]cédula[/] del miembro:");
        var miembro = _miembroService.BuscarMiembroPorCedula(cedula);

        if (miembro != null)
        {
            string nuevoTelefono = AnsiConsole.Ask<string>($"Ingresa el [yellow]nuevo teléfono[/] para {miembro.NombreCompleto}:");
            bool exito = _miembroService.ActualizarTelefonoMiembro(miembro.Id, nuevoTelefono);
            
            if (exito)
            {
                AnsiConsole.MarkupLine("[green]Teléfono actualizado correctamente![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Error al actualizar el teléfono.[/]");
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[red]No se encontró ningún miembro con esa cédula.[/]");
        }

        AnsiConsole.MarkupLine("\nPresiona cualquier tecla para continuar...");
        Console.ReadKey();
    }

    // Pantalla de eliminación
    private void EliminarMiembroScreen()
    {
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[blue]=== ELIMINAR MIEMBRO ===[/]\n");

        string cedula = AnsiConsole.Ask<string>("Ingresa la [yellow]cédula[/] del miembro a eliminar:");
        var miembro = _miembroService.BuscarMiembroPorCedula(cedula);

        if (miembro != null)
        {
            // Confirmación antes de eliminar
            bool confirmar = AnsiConsole.Confirm($"¿Seguro que quieres eliminar a [yellow]{miembro.NombreCompleto}[/]?");
            if (confirmar)
            {
                bool exito = _miembroService.EliminarMiembro(miembro.Id);
                if (exito)
                {
                    AnsiConsole.MarkupLine("[green]Miembro eliminado correctamente![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Error al eliminar el miembro.[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]Operación cancelada.[/]");
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[red]No se encontró ningún miembro con esa cédula.[/]");
        }

        AnsiConsole.MarkupLine("\nPresiona cualquier tecla para continuar...");
        Console.ReadKey();
    }
}
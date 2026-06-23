using Eva_aplicacionQR.Modelos;

namespace Eva_aplicacionQR.servicios;

/// <summary>
/// Servicio que maneja la lógica de asistencia.
/// Guarda los registros en memoria (lista estática) hasta que implementemos MySQL en el Hito 4.
/// Al ser estático, todos las páginas de la app comparten el mismo historial.
/// </summary>
public static class AsistenciaService
{
    // Lista estática que actúa como "base de datos temporal" en memoria
    public static List<RegistroAsistencia> Historial { get; } = new List<RegistroAsistencia>();

    /// <summary>
    /// Agrega un nuevo registro de asistencia al historial.
    /// </summary>
    public static void RegistrarAsistencia(string codigoQR)
    {
        var nuevoRegistro = new RegistroAsistencia
        {
            CodigoClase = codigoQR,
            FechaHora = DateTime.Now
        };

        Historial.Add(nuevoRegistro);
        System.Diagnostics.Debug.WriteLine($"[Servicio] Registrado: {codigoQR} a las {nuevoRegistro.FechaHoraFormateada}");
    }

    /// <summary>
    /// Devuelve el total de clases a las que el usuario ha asistido.
    /// </summary>
    public static int TotalAsistencias => Historial.Count;
}

namespace Eva_aplicacionQR.Modelos;

/// <summary>
/// Representa un registro de asistencia guardado localmente.
/// </summary>
public class RegistroAsistencia
{
    public string CodigoClase { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; }
    public string EstadoTexto => "Presente";

    // Propiedad calculada para mostrar la fecha de forma legible en la UI
    public string FechaHoraFormateada => FechaHora.ToString("dd/MM/yyyy  HH:mm:ss");
}

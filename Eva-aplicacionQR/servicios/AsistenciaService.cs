using Eva_aplicacionQR.Modelos;
using MySqlConnector;

namespace Eva_aplicacionQR.servicios;


public static class AsistenciaService
{
    // Si pruebas en el Emulador Android, la IP debe ser 10.0.2.2 para acceder a tu localhost (XAMPP/WAMP)
    // Si pruebas la app en "Windows Machine", la IP debe ser localhost o 127.0.0.1
    private const string ConnectionString = "Server=10.0.2.2;Database=eva_asistencia;Uid=root;Pwd=;";

    public static async Task RegistrarAsistenciaAsync(string codigoQR)
    {
        try
        {
            using var connection = new MySqlConnection(ConnectionString);
            await connection.OpenAsync();

            var query = "INSERT INTO RegistroAsistencia (CodigoClase, FechaHora) VALUES (@codigo, @fecha)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@codigo", codigoQR);
            command.Parameters.AddWithValue("@fecha", DateTime.Now);

            await command.ExecuteNonQueryAsync();
            System.Diagnostics.Debug.WriteLine($"[Servicio] Registrado en DB: {codigoQR}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Error DB] {ex.Message}");
            throw; // Re-lanzar para manejar el error en la UI si es necesario
        }
    }

    public static async Task<List<RegistroAsistencia>> ObtenerHistorialAsync()
    {
        var historial = new List<RegistroAsistencia>();
        try
        {
            using var connection = new MySqlConnection(ConnectionString);
            await connection.OpenAsync();

            var query = "SELECT CodigoClase, FechaHora FROM RegistroAsistencia ORDER BY FechaHora DESC";
            using var command = new MySqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                historial.Add(new RegistroAsistencia
                {
                    CodigoClase = reader.GetString("CodigoClase"),
                    FechaHora = reader.GetDateTime("FechaHora")
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Error DB] {ex.Message}");
        }
        
        return historial;
    }
}

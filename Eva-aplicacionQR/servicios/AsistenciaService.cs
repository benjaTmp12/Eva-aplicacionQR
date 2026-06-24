using Eva_aplicacionQR.Modelos;
using MySqlConnector;
using Microsoft.Maui.Devices;

namespace Eva_aplicacionQR.servicios;


public static class AsistenciaService
{
    // Construye la cadena de conexión según la plataforma:
    // - Android (emulador): 10.0.2.2 para acceder al localhost del host
    // - Windows/otros: localhost
    private static string GetConnectionString()
    {
        var server = DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost";
        return $"Server={server};Database=eva_asistencia;Uid=root;Pwd=;";
    }

    // Guarda localmente y trata de enviar al servidor. Siempre guarda local para asegurar persistencia offline.
    public static async Task RegistrarAsistenciaAsync(string codigoQR)
    {
        // Guardar localmente primero
        var registroLocal = new RegistroAsistencia { CodigoClase = codigoQR, FechaHora = DateTime.Now };
        int localId = await LocalDbService.SaveLocalAsync(registroLocal);

        try
        {
            using var connection = new MySqlConnection(GetConnectionString());
            await connection.OpenAsync();

            var query = "INSERT INTO RegistroAsistencia (CodigoClase, FechaHora) VALUES (@codigo, @fecha)";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@codigo", codigoQR);
            command.Parameters.AddWithValue("@fecha", registroLocal.FechaHora);

            await command.ExecuteNonQueryAsync();
            System.Diagnostics.Debug.WriteLine($"[Servicio] Registrado en DB remota: {codigoQR}");

            // Marcar como sincronizado localmente
            await LocalDbService.MarkAsSyncedAsync(localId);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Error DB] {ex.Message}");
            // No relanzamos: el registro queda en local para reintentar luego
        }
    }

    // Obtener historial desde la base local (fuente principal para la UI offline-first)
    public static async Task<List<RegistroAsistencia>> ObtenerHistorialAsync()
    {
        try
        {
            return await LocalDbService.GetAllAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Error Local DB] {ex.Message}");
            return new List<RegistroAsistencia>();
        }
    }

    // Intentar sincronizar registros pendientes con el servidor
    public static async Task SyncPendingAsync()
    {
        var pendientes = await LocalDbService.GetPendingAsync();
        foreach (var p in pendientes)
        {
            try
            {
                using var connection = new MySqlConnection(GetConnectionString());
                await connection.OpenAsync();

                var query = "INSERT INTO RegistroAsistencia (CodigoClase, FechaHora) VALUES (@codigo, @fecha)";
                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@codigo", p.CodigoClase);
                command.Parameters.AddWithValue("@fecha", p.FechaHora);

                await command.ExecuteNonQueryAsync();
                System.Diagnostics.Debug.WriteLine($"[Sync] Sincronizado remoto: {p.CodigoClase}");
                await LocalDbService.MarkAsSyncedAsync(p.Id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Sync Error] {ex.Message}");
                // Si falla, continuar con el siguiente
            }
        }
    }
}

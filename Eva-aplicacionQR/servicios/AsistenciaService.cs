using System;

namespace Eva_aplicacionQR.servicios
{
    public class AsistenciaService
    {
        public bool RegistrarAsistencia(string codigoQR)
        {
            // En el Hito 4 implementaremos la conexión real a MySQL.
            // Por ahora, simulamos que el guardado fue exitoso y mostramos un log en la consola de depuración.
            System.Diagnostics.Debug.WriteLine($"[Servicio] RegistrarAsistencia llamado para el código: {codigoQR}");
            return true;
        }
    }
}

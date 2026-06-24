using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Eva_aplicacionQR.Modelos;
using SQLite;
using Microsoft.Maui.Storage;

namespace Eva_aplicacionQR.servicios;

[Table("RegistroAsistenciaLocal")]
public class RegistroAsistenciaLocal
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string CodigoClase { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; }
    public bool Sincronizado { get; set; }
}

public static class LocalDbService
{
    private static SQLiteAsyncConnection? _database;
    private static readonly string DbFileName = "asistencia_local.db3";

    public static async Task InitAsync()
    {
        if (_database != null) return;

        var folder = FileSystem.AppDataDirectory; // espacio de la app
        var path = Path.Combine(folder, DbFileName);
        _database = new SQLiteAsyncConnection(path);
        await _database.CreateTableAsync<RegistroAsistenciaLocal>();
    }

    public static async Task<int> SaveLocalAsync(RegistroAsistencia registro)
    {
        await InitAsync();
        var local = new RegistroAsistenciaLocal
        {
            CodigoClase = registro.CodigoClase,
            FechaHora = registro.FechaHora,
            Sincronizado = false
        };
        var result = await _database!.InsertAsync(local);
        return local.Id; // return id generado
    }

    public static async Task<List<RegistroAsistencia>> GetAllAsync()
    {
        await InitAsync();
        var list = await _database!.Table<RegistroAsistenciaLocal>().OrderByDescending(r => r.FechaHora).ToListAsync();
        return list.Select(l => new RegistroAsistencia { CodigoClase = l.CodigoClase, FechaHora = l.FechaHora }).ToList();
    }

    public static async Task<List<RegistroAsistenciaLocal>> GetPendingAsync()
    {
        await InitAsync();
        return await _database!.Table<RegistroAsistenciaLocal>().Where(r => !r.Sincronizado).OrderBy(r => r.FechaHora).ToListAsync();
    }

    public static async Task MarkAsSyncedAsync(int id)
    {
        await InitAsync();
        var item = await _database!.FindAsync<RegistroAsistenciaLocal>(id);
        if (item == null) return;
        item.Sincronizado = true;
        await _database.UpdateAsync(item);
    }

    public static async Task DeleteAsync(int id)
    {
        await InitAsync();
        await _database!.DeleteAsync<RegistroAsistenciaLocal>(id);
    }

    public static async Task ClearAllAsync()
    {
        await InitAsync();
        await _database!.DeleteAllAsync<RegistroAsistenciaLocal>();
    }
}

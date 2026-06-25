using ZXing.Net.Maui;
using Eva_aplicacionQR.servicios;

namespace Eva_aplicacionQR.Views;

public partial class ScannerPage : ContentPage
{
    private bool _camaraActiva = false;

    public ScannerPage()
    {
        InitializeComponent();
        barcodeReader.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormat.QrCode,
            AutoRotate = true,
            Multiple = false
        };
    }

    private async void OnResetClicked(object? sender, EventArgs e)
    {
        if (!_camaraActiva)
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                // await DisplayAlert("Permiso Denegado", "La aplicación necesita acceso a la cámara.", "OK");
                return;
            }

            _camaraActiva = true;
            barcodeReader.IsDetecting = true;
            CameraPlaceholder.IsVisible = false;
            LaserLine.IsVisible = true;
            ResetBtn.Text = "Detener Cámara";
            ResetBtn.BackgroundColor = Color.FromArgb("#EF4444");
            StatusLabel.Text = "Escaneando...";
            StatusLabel.TextColor = Color.FromArgb("#34D399");
        }
        else
        {
            _camaraActiva = false;
            barcodeReader.IsDetecting = false;
            CameraPlaceholder.IsVisible = true;
            LaserLine.IsVisible = false;
            ResetBtn.Text = "Activar Cámara";
            ResetBtn.BackgroundColor = Color.FromArgb("#334155");
            StatusLabel.Text = "Esperando...";
            StatusLabel.TextColor = Color.FromArgb("#60A5FA");
            ResultLabel.Text = "Código: Ninguno";
            TimestampLabel.Text = "Hora: --:--";
        }
    }


    private void OnBarcodesDetected(object? sender, BarcodeDetectionEventArgs e)
    {
        var primerCodigo = e.Results.FirstOrDefault();
        if (primerCodigo == null) return;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            ProcesarCodigo(primerCodigo.Value);
        });
    }

    private async void ProcesarCodigo(string textoCodigo)
    {
        barcodeReader.IsDetecting = false;
        _camaraActiva = false;
        CameraPlaceholder.IsVisible = true;
        LaserLine.IsVisible = false;
        ResetBtn.Text = "Activar Cámara";
        ResetBtn.BackgroundColor = Color.FromArgb("#334155");

        ResultLabel.Text = $"Código: {textoCodigo}";
        TimestampLabel.Text = $"Hora: {DateTime.Now:HH:mm:ss}";

        try
        {
            await AsistenciaService.RegistrarAsistenciaAsync(textoCodigo);
            StatusLabel.Text = "Asistencia Registrada";
            StatusLabel.TextColor = Color.FromArgb("#34D399");
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(250));
        }
        catch (Exception)
        {
            StatusLabel.Text = "Error: Sin conexión BD";
            StatusLabel.TextColor = Color.FromArgb("#EF4444");
        }
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        // Navegar hacia atrás
        await Shell.Current.GoToAsync("..");
    }
}

using ZXing.Net.Maui;
using Eva_aplicacionQR.servicios;

namespace Eva_aplicacionQR
{
    public partial class MainPage : ContentPage
    {
        private readonly AsistenciaService _asistenciaService = new AsistenciaService();
        private bool _camaraActiva = false;

        public MainPage()
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
                    await DisplayAlertAsync("Permiso Denegado", "Se requiere acceso a la cámara", "OK");
                    return;
                }

                _camaraActiva = true;
                barcodeReader.IsDetecting = true;
                CameraPlaceholder.IsVisible = false;
                
                ResetBtn.Text = "Detener";
                ResetBtn.BackgroundColor = Color.FromArgb("#EF4444");
                ResetBtn.TextColor = Color.FromArgb("#FFFFFF");
                StatusLabel.Text = "Escaneando";
            }
            else
            {
                _camaraActiva = false;
                barcodeReader.IsDetecting = false;
                CameraPlaceholder.IsVisible = true;
                
                ResetBtn.Text = "Activar";
                ResetBtn.BackgroundColor = Color.FromArgb("#111827");
                ResetBtn.TextColor = Color.FromArgb("#FFFFFF");
                
                StatusLabel.Text = "Esperando...";
                ResultLabel.Text = "--";
                TimestampLabel.Text = "";
            }
        }

        private void OnSimulateClicked(object? sender, EventArgs e)
        {
            ProcesarCodigo("CLASE_01");
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

        private void ProcesarCodigo(string textoCodigo)
        {
            barcodeReader.IsDetecting = false;
            _camaraActiva = false;
            CameraPlaceholder.IsVisible = true;
            
            ResetBtn.Text = "Activar";
            ResetBtn.BackgroundColor = Color.FromArgb("#111827");
            ResetBtn.TextColor = Color.FromArgb("#FFFFFF");

            StatusLabel.Text = "Registrado";
            ResultLabel.Text = textoCodigo;
            TimestampLabel.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");

            _asistenciaService.RegistrarAsistencia(textoCodigo);
        }
    }
}

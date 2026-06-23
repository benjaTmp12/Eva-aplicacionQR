using Eva_aplicacionQR.servicios;

namespace Eva_aplicacionQR.Views;

public partial class HistoryPage : ContentPage
{
    public HistoryPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ActualizarUI();
    }

    private async void ActualizarUI()
    {
        var historial = await AsistenciaService.ObtenerHistorialAsync();
        TotalLabel.Text = $"Total: {historial.Count} clases";

        if (historial.Count > 0)
        {
            SinRegistrosLayout.IsVisible = false;
            RegistrosLista.IsVisible = true;
            RegistrosLista.ItemsSource = historial; // Ya viene ordenado de BD
        }
        else
        {
            SinRegistrosLayout.IsVisible = true;
            RegistrosLista.IsVisible = false;
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}

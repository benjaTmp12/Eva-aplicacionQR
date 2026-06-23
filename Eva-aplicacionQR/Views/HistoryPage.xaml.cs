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

    private void ActualizarUI()
    {
        var historial = AsistenciaService.Historial;
        TotalLabel.Text = $"Total: {AsistenciaService.TotalAsistencias} clases";

        if (historial.Count > 0)
        {
            SinRegistrosLayout.IsVisible = false;
            RegistrosLista.IsVisible = true;
            RegistrosLista.ItemsSource = historial.OrderByDescending(r => r.FechaHora).ToList();
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

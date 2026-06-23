namespace Eva_aplicacionQR;

public partial class RegistroPage : ContentPage
{
    public RegistroPage()
    {
        InitializeComponent();
    }

    private async void Registrar_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Registro", "Usuario registrado correctamente", "OK");
        await Navigation.PopAsync();
    }
}
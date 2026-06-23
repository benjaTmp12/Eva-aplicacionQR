namespace Eva_aplicacionQR.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Usamos navegación relativa. Para evitar que el usuario vuelva atrás, 
        // más adelante podemos limpiar el stack de navegación.
        await Shell.Current.GoToAsync("MenuPage");
    }
}

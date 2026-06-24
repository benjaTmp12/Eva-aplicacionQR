namespace Eva_aplicacionQR.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        string username = UserEntry.Text;
        string password = PasswordEntry.Text;

        // Credenciales estáticas solicitadas
        if (username == "admin" && password == "1234")
        {
            // Usamos navegación relativa. Para evitar que el usuario vuelva atrás, 
            // más adelante podemos limpiar el stack de navegación.
            await Shell.Current.GoToAsync("MenuPage");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error de Autenticación", "El usuario o la contraseña no son válidos.", "OK");
        }
    }
}

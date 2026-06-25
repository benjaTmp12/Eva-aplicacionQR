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

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlertAsync("Campos vacíos", "Por favor completa el usuario y la contraseña.", "OK");
            return;
        }

        if (username == "admin" && password == "1234")
        {
            await Shell.Current.GoToAsync("MenuPage");
        }
        else
        {
            await DisplayAlertAsync("Error de Autenticación", "El usuario o la contraseña no son válidos.", "OK");
        }
    }
}

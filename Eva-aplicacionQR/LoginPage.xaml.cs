namespace Eva_aplicacionQR;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void Login_Clicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        if (email == "admin@gmail.com" && password == "1234")
        {
            await Navigation.PushAsync(new MainPage());
        }
        else
        {
            await DisplayAlert("Error", "Correo o contraseña incorrectos", "OK");
        }
    }

    private async void Registro_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegistroPage());
    }
}
using Eva_aplicacionQR.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Eva_aplicacionQR
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NewPage1());
        }
    }
}
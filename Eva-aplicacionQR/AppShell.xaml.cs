using Eva_aplicacionQR.Views;

namespace Eva_aplicacionQR
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            // Registrar las rutas de las páginas secundarias
            Routing.RegisterRoute("MenuPage", typeof(MenuPage));
            Routing.RegisterRoute("ScannerPage", typeof(ScannerPage));
            Routing.RegisterRoute("HistoryPage", typeof(HistoryPage));
        }
    }
}


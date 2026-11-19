using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using SistemaDelivery.DataBase;
using SistemaDelivery.View;

namespace SistemaDelivery;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ConfigurarCulturaPtBr();

        try
        {
            // Cria o banco e aplica as migrations (Code First). Os usuários de teste vêm do seed (HasData).
            DeliveryDBContext.Inicializar();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "Não foi possível preparar o banco de dados.\n\n" +
                "Confira se o SQL Server LocalDB está instalado (Visual Studio Installer).\n\n" +
                ex.GetBaseException().Message,
                "Yummy", MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
            return;
        }

        new LoginWindow().Show();
    }

    // Sem isso o WPF formata números/datas em en-US (R$20.00 em vez de R$20,00)
    private static void ConfigurarCulturaPtBr()
    {
        var cultura = new CultureInfo("pt-BR");
        CultureInfo.DefaultThreadCurrentCulture = cultura;
        CultureInfo.DefaultThreadCurrentUICulture = cultura;
        Thread.CurrentThread.CurrentCulture = cultura;
        Thread.CurrentThread.CurrentUICulture = cultura;

        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(cultura.IetfLanguageTag)));
    }
}

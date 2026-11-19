using System.Windows;
using SistemaDelivery.Control;
using SistemaDelivery.Model;

namespace SistemaDelivery.View;

// Janela principal: só alterna entre as duas telas. Não tem regra de negócio.
public partial class DeliveryWindow : Window
{
    private readonly PedidosView _pedidos = new();
    private readonly CardapioView _cardapio = new();

    public DeliveryWindow(Usuario usuario)
    {
        InitializeComponent();
        Title = $"Yummy - {usuario.Nome}";
        rbPedidos.IsChecked = true; // dispara Pedidos_Checked e mostra a tela inicial
    }

    private void Pedidos_Checked(object sender, RoutedEventArgs e) => Conteudo.Content = _pedidos;

    private void Cardapio_Checked(object sender, RoutedEventArgs e) => Conteudo.Content = _cardapio;

    private void Sair_Click(object sender, RoutedEventArgs e)
    {
        var login = new LoginWindow();
        Application.Current.MainWindow = login;
        login.Show();
        Close();
    }
}

using System.Windows;
using System.Windows.Controls;
using SistemaDelivery.Control;
using SistemaDelivery.Model;

namespace SistemaDelivery.View;

public partial class PedidosView : UserControl
{
    private readonly PedidoService _pedidos = new();

    public PedidosView()
    {
        InitializeComponent();
        Loaded += (_, _) => Carregar(); // recarrega sempre que a tela volta a ser exibida
    }

    private void Carregar()
    {
        try
        {
            var lista = _pedidos.Listar(txtBusca.Text);
            lstPedidos.ItemsSource = lista;
            txtVazio.Visibility = lista.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        catch (Exception ex)
        {
            Mensagens.Tratar(Window.GetWindow(this), ex);
        }
    }

    private void Busca_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (IsLoaded)
            Carregar();
    }

    private void NovoPedido_Click(object sender, RoutedEventArgs e) => AbrirModal(null);

    private void AbrirPedido_Click(object sender, RoutedEventArgs e)
    {
        if (((FrameworkElement)sender).DataContext is not Pedido resumo)
            return;

        try
        {
            // a lista traz só o resumo; o modal precisa dos itens
            AbrirModal(_pedidos.ObterComItens(resumo.IdPedido));
        }
        catch (Exception ex)
        {
            Mensagens.Tratar(Window.GetWindow(this), ex);
        }
    }

    private void AbrirModal(Pedido? pedido)
    {
        var dono = Window.GetWindow(this);
        var modal = new PedidoModal(pedido) { Owner = dono };

        if (modal.ShowDialog() == true)
            Carregar();
    }
}

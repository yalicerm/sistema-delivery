using System.Windows;
using System.Windows.Controls;
using SistemaDelivery.Control;
using SistemaDelivery.Model;

namespace SistemaDelivery.View;

public partial class CardapioView : UserControl
{
    private readonly ProdutoService _produtos = new();

    public CardapioView()
    {
        InitializeComponent();
        Loaded += (_, _) => Carregar();
    }

    private void Carregar()
    {
        try
        {
            var lista = _produtos.Listar(txtBusca.Text);
            lstProdutos.ItemsSource = lista;
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

    private void NovoProduto_Click(object sender, RoutedEventArgs e) => AbrirModal(null);

    private void EditarProduto_Click(object sender, RoutedEventArgs e)
    {
        if (((FrameworkElement)sender).DataContext is Produto produto)
            AbrirModal(produto);
    }

    private void ExcluirProduto_Click(object sender, RoutedEventArgs e)
    {
        if (((FrameworkElement)sender).DataContext is not Produto produto)
            return;

        var dono = Window.GetWindow(this);
        if (!Mensagens.Confirmar(dono, $"Excluir \"{produto.Nome}\" do cardápio?"))
            return;

        try
        {
            _produtos.Excluir(produto.IdProduto);
            Carregar();
        }
        catch (Exception ex)
        {
            Mensagens.Tratar(dono, ex);
        }
    }

    private void AbrirModal(Produto? produto)
    {
        var modal = new ProdutoModal(produto) { Owner = Window.GetWindow(this) };

        if (modal.ShowDialog() == true)
            Carregar();
    }
}

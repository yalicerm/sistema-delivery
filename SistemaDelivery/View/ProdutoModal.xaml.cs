using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SistemaDelivery.Control;
using SistemaDelivery.Model;

namespace SistemaDelivery.View;

// Modal "Novo produto" / "Editar produto" (produto == null -> novo).
public partial class ProdutoModal : Window
{
    private readonly ProdutoService _produtos = new();
    private readonly Produto? _produto;

    public ProdutoModal(Produto? produto = null)
    {
        InitializeComponent();
        _produto = produto;

        cmbCategoria.ItemsSource = CategoriaProduto.Todas;

        if (produto is null)
            return;

        txtTitulo.Text = "Editar produto";
        txtNome.Text = produto.Nome;
        cmbCategoria.SelectedItem = produto.Categoria;
        txtPreco.Text = Formatos.Moeda(produto.Valor);
        txtTempo.Text = Formatos.Minutos(produto.TempoPreparoMinutos);
    }

    // Bebida não tem tempo de preparo: o campo é desabilitado (e a lista mostra "-")
    private void Categoria_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var ehBebida = CategoriaProduto.EhBebida(cmbCategoria.SelectedItem as string);

        txtTempo.IsEnabled = !ehBebida;
        lblTempo.Text = ehBebida ? "Tempo de preparo" : "Tempo de preparo*";
        if (ehBebida)
            txtTempo.Text = "";
    }

    private void Salvar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _produtos.Salvar(_produto?.IdProduto, txtNome.Text, cmbCategoria.SelectedItem as string,
                             txtPreco.Text, txtTempo.Text);
            DialogResult = true;
        }
        catch (Exception ex)
        {
            Mensagens.Tratar(this, ex);
        }
    }

    private void Fechar_Click(object sender, RoutedEventArgs e) => DialogResult = false;

    private void Cabecalho_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ButtonState == MouseButtonState.Pressed)
            DragMove();
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
            DialogResult = false;
    }
}

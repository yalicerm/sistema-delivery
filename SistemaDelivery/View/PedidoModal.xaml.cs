using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using SistemaDelivery.Control;
using SistemaDelivery.Control.FormasPagamento;
using SistemaDelivery.Model;

namespace SistemaDelivery.View;

// Modal "Novo pedido" / "Editar pedido" (pedido == null -> novo).
// Se o pedido já foi entregue/cancelado, abre somente para visualização.
public partial class PedidoModal : Window
{
    private readonly PedidoService _pedidos = new();
    private readonly ProdutoService _produtos = new();
    private readonly ObservableCollection<ItemPedido> _itens = new();
    private readonly Pedido? _pedido;

    public PedidoModal(Pedido? pedido = null)
    {
        InitializeComponent();
        _pedido = pedido;

        lstItens.ItemsSource = _itens;
        cmbProdutos.ItemsSource = _produtos.Listar();

        PrepararTela();
        AtualizarResumo();
    }

    private void PrepararTela()
    {
        if (_pedido is null)
        {
            btnCancelarPedido.Visibility = Visibility.Collapsed;
            btnConcluir.Visibility = Visibility.Collapsed;
            return;
        }

        txtNome.Text = _pedido.NomeComanda;
        foreach (var item in _pedido.Itens)
            _itens.Add(item);

        if (_pedido.Status == StatusPedido.EmAndamento)
        {
            txtTitulo.Text = "Editar pedido";
            // Pedido em andamento: escolhe a forma de pagamento para concluir
            cmbPagamento.ItemsSource = FormasPagamentoDisponiveis.Todas;
            pnlPagamento.Visibility = Visibility.Visible;
            return;
        }

        // Somente leitura
        txtTitulo.Text = "Visualizar pedido";
        MostrarPagamentoRealizado(_pedido.Pagamento);
        txtNome.IsReadOnly = true;
        lstItens.IsHitTestVisible = false;
        pnlProdutos.Visibility = Visibility.Collapsed;
        btnSalvar.Visibility = Visibility.Collapsed;
        btnConcluir.Visibility = Visibility.Collapsed;
        btnCancelarPedido.Visibility = Visibility.Collapsed;
    }

    private void Produtos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (cmbProdutos.SelectedItem is not Produto produto)
            return;

        var existente = _itens.FirstOrDefault(i => i.ProdutoId == produto.IdProduto);
        if (existente is null)
        {
            _itens.Add(new ItemPedido
            {
                ProdutoId = produto.IdProduto,
                Produto = produto,
                Quantidade = 1,
                PrecoUnitario = produto.Valor
            });
        }
        else
        {
            existente.Quantidade++;
        }

        cmbProdutos.SelectedIndex = -1; // volta para "Selecione um produto"
        AtualizarLista();
    }

    private void Aumentar_Click(object sender, RoutedEventArgs e)
    {
        if (ItemDoBotao(sender) is not { } item) return;
        item.Quantidade++;
        AtualizarLista();
    }

    private void Diminuir_Click(object sender, RoutedEventArgs e)
    {
        if (ItemDoBotao(sender) is not { } item) return;
        if (item.Quantidade > 1)
            item.Quantidade--;
        AtualizarLista();
    }

    private void Remover_Click(object sender, RoutedEventArgs e)
    {
        if (ItemDoBotao(sender) is not { } item) return;
        _itens.Remove(item);
        AtualizarLista();
    }

    private void Salvar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _pedidos.Salvar(_pedido?.IdPedido, txtNome.Text, _itens);
            DialogResult = true;
        }
        catch (Exception ex)
        {
            Mensagens.Tratar(this, ex);
        }
    }

    private void Concluir_Click(object sender, RoutedEventArgs e)
    {
        if (_pedido is null)
            return;

        if (cmbPagamento.SelectedItem is not IFormaPagamento forma)
        {
            Mensagens.Aviso(this, "Selecione a forma de pagamento para concluir o pedido.");
            return;
        }

        var valorFinal = forma.CalcularValorFinal(PedidoService.CalcularTotal(_itens));
        if (!Mensagens.Confirmar(this, $"Concluir o pedido com pagamento via {forma.Nome} de {Formatos.Moeda(valorFinal)}?"))
            return;

        try
        {
            _pedidos.Salvar(_pedido.IdPedido, txtNome.Text, _itens); // guarda alterações pendentes
            _pedidos.Concluir(_pedido.IdPedido, forma);              // registra o pagamento e marca como entregue
            DialogResult = true;
        }
        catch (Exception ex)
        {
            Mensagens.Tratar(this, ex);
        }
    }

    private void CancelarPedido_Click(object sender, RoutedEventArgs e)
    {
        if (_pedido is null || !Mensagens.Confirmar(this, "Cancelar este pedido? Essa ação não pode ser desfeita."))
            return;

        try
        {
            _pedidos.Cancelar(_pedido.IdPedido);
            DialogResult = true;
        }
        catch (Exception ex)
        {
            Mensagens.Tratar(this, ex);
        }
    }

    private void Fechar_Click(object sender, RoutedEventArgs e) => DialogResult = false;

    // ---------- apoio de tela ----------

    private static ItemPedido? ItemDoBotao(object sender)
        => (sender as FrameworkElement)?.DataContext as ItemPedido;

    // ItemPedido não notifica mudanças; Refresh() redesenha as linhas com a nova quantidade
    private void AtualizarLista()
    {
        CollectionViewSource.GetDefaultView(_itens).Refresh();
        AtualizarResumo();
    }

    private void AtualizarResumo()
    {
        var total = PedidoService.CalcularTotal(_itens);
        txtTempo.Text = Formatos.Minutos(PedidoService.CalcularTempoEspera(_itens));
        txtTotal.Text = Formatos.Moeda(total);

        // Mostra "Valor a pagar" só quando a forma escolhida muda o valor (ex: desconto no Pix)
        var valorFinal = (cmbPagamento.SelectedItem as IFormaPagamento)?.CalcularValorFinal(total) ?? total;
        MostrarValorAPagar(valorFinal != total ? valorFinal : null);
    }

    private void Pagamento_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        => AtualizarResumo();

    private void MostrarValorAPagar(decimal? valor)
    {
        var visibilidade = valor is null ? Visibility.Collapsed : Visibility.Visible;
        lblValorPagar.Visibility = visibilidade;
        txtValorPagar.Visibility = visibilidade;
        txtValorPagar.Text = valor is null ? "" : Formatos.Moeda(valor.Value);
    }

    // Pedido entregue: mostra como foi pago (sem ComboBox)
    private void MostrarPagamentoRealizado(Pagamento? pagamento)
    {
        if (pagamento is null)
            return;

        pnlPagamento.Visibility = Visibility.Visible;
        lblPagamento.Text = "Pagamento";
        cmbPagamento.Visibility = Visibility.Collapsed;
        txtPagamentoRealizado.Visibility = Visibility.Visible;
        txtPagamentoRealizado.Text =
            $"Pago via {pagamento.FormaPagamento}: {Formatos.Moeda(pagamento.ValorPago)} em {pagamento.DataPagamento:dd/MM/yyyy HH:mm}";
    }

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

namespace SistemaDelivery.Control.FormasPagamento
{
    // Lista das formas de pagamento oferecidas na tela (ComboBox do PedidoModal).
    // Para oferecer uma forma nova, basta criar a classe e incluir aqui.
    public static class FormasPagamentoDisponiveis
    {
        public static readonly IReadOnlyList<IFormaPagamento> Todas = new IFormaPagamento[]
        {
            new PagamentoPix(),
            new PagamentoCartao(),
            new PagamentoDinheiro()
        };
    }
}

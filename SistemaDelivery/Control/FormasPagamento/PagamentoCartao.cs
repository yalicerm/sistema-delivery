namespace SistemaDelivery.Control.FormasPagamento
{
    // Cartão cobra o valor normal do pedido
    public class PagamentoCartao : IFormaPagamento
    {
        public string Nome => "Cartão";

        public decimal CalcularValorFinal(decimal valorPedido)
        {
            return valorPedido;
        }
    }
}

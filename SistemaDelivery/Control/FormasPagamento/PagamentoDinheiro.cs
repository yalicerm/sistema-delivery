namespace SistemaDelivery.Control.FormasPagamento
{
    // Dinheiro cobra o valor normal do pedido
    public class PagamentoDinheiro : IFormaPagamento
    {
        public string Nome => "Dinheiro";

        public decimal CalcularValorFinal(decimal valorPedido)
        {
            return valorPedido;
        }
    }
}

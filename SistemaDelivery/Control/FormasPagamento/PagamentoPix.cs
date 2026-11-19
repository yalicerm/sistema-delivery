namespace SistemaDelivery.Control.FormasPagamento
{
    // Pix tem 5% de desconto
    public class PagamentoPix : IFormaPagamento
    {
        private const decimal PercentualDesconto = 0.05m;

        public string Nome => "Pix";

        public decimal CalcularValorFinal(decimal valorPedido)
        {
            return Math.Round(valorPedido * (1 - PercentualDesconto), 2);
        }
    }
}

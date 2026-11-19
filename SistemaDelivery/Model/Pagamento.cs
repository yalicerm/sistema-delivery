using System;

namespace SistemaDelivery.Model
{
    // Relacionamento 1x1 com Pedido: cada pedido tem no máximo um pagamento
    // e cada pagamento pertence a exatamente um pedido.
    public class Pagamento
    {
        public int IdPagamento { get; set; }
        public string FormaPagamento { get; set; } = string.Empty; // "Pix", "Cartão" ou "Dinheiro"
        public decimal ValorPago { get; set; }                      // Valor final, já com desconto (se houver)
        public DateTime DataPagamento { get; set; }

        // Chave estrangeira (FK) para o Pedido - é ela que liga o 1x1
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;
    }
}

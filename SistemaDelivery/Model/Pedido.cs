using System;

namespace SistemaDelivery.Model {

    public class Pedido
    {
        public int IdPedido { get; set; }
        public string NomeComanda { get; set; } = string.Empty; // Nome do cliente/comanda no balcão
        public DateTime DataHora { get; set; }
        public string Status { get; set; } = StatusPedido.EmAndamento; // Valores em StatusPedido
        public decimal ValorTotal { get; set; }

        //Criar uma coleção de ItemPedido
        public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();

        //Relacionamento 1x1: o pagamento do pedido (fica null enquanto o pedido não for pago)
        public Pagamento? Pagamento { get; set; }
    }

}

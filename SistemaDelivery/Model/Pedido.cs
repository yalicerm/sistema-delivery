using System;

namespace SistemaDelivery.Model {

    public class Pedido
    {
        public int IdPedido { get; set; }
        public DateTime DataHora { get; set; }
        public string Status { get; set; } = string.Empty; // "Finalizado", "Cancelado"
        public decimal ValorTotal { get; set; }

        //Criar uma coleção de ItemPedido
        public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
    }

}

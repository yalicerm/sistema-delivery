using System;

namespace SistemaDelivery.Model
{
    //Classe ItemPedido é responsável por armazenar os itens de um pedido, ou seja, os produtos que foram comprados
    public class ItemPedido
    {
        public int IdItem { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }

        // Criando variável para facilitar na atribuição das chaves estrangeiras (FK)
        //ItemPedido recebe ID do Pedido e o ID do produto
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;

        public int ProdutoId { get; set; }
        public Produto Produto { get; set; } = null!;
    }
}
using System;

public class Pedido
{
    public int IdPedido { get; set; }
    public DateTime DataHora { get; set; }
    public string Status { get; set; } // "Finalizado", "Cancelado"
    public decimal ValorTotal { get; set; }

    //Criar uma coleção de ItemPedido
    public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
}
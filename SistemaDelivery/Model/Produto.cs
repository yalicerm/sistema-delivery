using System;

namespace SistemaDelivery.Model
{
    //Classe produto é abstrata pois vamos trabalhar com as classes Pratos e Bebida
    public abstract class Produto
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }

}

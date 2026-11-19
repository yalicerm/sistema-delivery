using System;

namespace SistemaDelivery.Model
{
    public class Bebida : Produto
    {
        public int capacidadeML { get; set; }
        public bool EhAlcoolica { get; set; }

        //Bebida não tem tempo de preparo (usa o 0 padrão da classe Produto)
        public override string Categoria =>
            EhAlcoolica ? CategoriaProduto.BebidaAlcoolica : CategoriaProduto.BebidaNaoAlcoolica;
    }
}

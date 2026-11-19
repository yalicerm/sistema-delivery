using System;

namespace SistemaDelivery.Model
{
    //Classe Lanche herda da classe produto, pois lanche é um tipo de produto
    public class Lanche : Produto
    {
        public int tempoDePreparo { get; set; }

        public override string Categoria => CategoriaProduto.Lanche;
        public override int TempoPreparoMinutos => tempoDePreparo;
    }
}

using System;

namespace SistemaDelivery.Model
{
    //Classe prato herda da classe produto, pois prato é um tipo de produto
    public class Lanche : Produto
    {
        public int tempoDePreparo { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaDelivery.Model
{
    //Classe produto é abstrata pois vamos trabalhar com as classes Lanche, Acompanhamento e Bebida
    public abstract class Produto
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Valor { get; set; }

        //Polimorfismo: cada classe filha informa a sua categoria e o seu tempo de preparo.
        //[NotMapped] = não vira coluna no banco, é calculado a partir do tipo do produto.
        [NotMapped]
        public abstract string Categoria { get; }

        [NotMapped]
        public virtual int TempoPreparoMinutos => 0;
    }

}

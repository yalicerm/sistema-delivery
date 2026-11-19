using System;

namespace SistemaDelivery.Model
{
    public class Acompanhamento : Produto
    {
        public int tempoPreparado { get; set; }

        public override string Categoria => CategoriaProduto.Acompanhamento;
        public override int TempoPreparoMinutos => tempoPreparado;
    }
}

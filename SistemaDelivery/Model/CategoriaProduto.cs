namespace SistemaDelivery.Model
{
    // Categorias exibidas na tela de cardápio. Cada uma corresponde a uma classe filha de Produto.
    public static class CategoriaProduto
    {
        public const string Lanche = "Lanche";
        public const string Acompanhamento = "Acompanhamento";
        public const string BebidaNaoAlcoolica = "Bebida não-alcoólica";
        public const string BebidaAlcoolica = "Bebida alcoólica";

        public static readonly IReadOnlyList<string> Todas = new[]
        {
            Lanche, Acompanhamento, BebidaNaoAlcoolica, BebidaAlcoolica
        };

        public static bool EhBebida(string? categoria)
            => categoria == BebidaNaoAlcoolica || categoria == BebidaAlcoolica;
    }
}

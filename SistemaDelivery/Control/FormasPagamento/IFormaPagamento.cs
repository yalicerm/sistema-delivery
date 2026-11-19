namespace SistemaDelivery.Control.FormasPagamento
{
    /*
     * Padrão de Projeto STRATEGY: cada forma de pagamento é uma "estratégia" com a sua própria regra de cálculo.
     * SOLID - OCP (Aberto/Fechado): para criar uma nova forma de pagamento (ex: Vale-Refeição),
     * basta criar uma nova classe que implementa esta interface, sem alterar as classes que já existem.
     */
    public interface IFormaPagamento
    {
        string Nome { get; }                                 // Nome salvo no banco (ex: "Pix")
        decimal CalcularValorFinal(decimal valorPedido);     // Regra de cálculo de cada forma
    }
}

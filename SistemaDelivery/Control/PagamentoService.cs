using SistemaDelivery.Control.FormasPagamento;
using SistemaDelivery.Model;

namespace SistemaDelivery.Control
{
    /*
     * Cria o Pagamento de um Pedido usando a forma de pagamento escolhida (Strategy).
     * O serviço não sabe se é Pix, Cartão ou Dinheiro: ele só chama CalcularValorFinal(...)
     * da interface IFormaPagamento (SOLID - DIP e OCP).
     *
     * Uso na View:  var pagamento = new PagamentoService().RegistrarPagamento(pedido, new PagamentoPix());
     * O pagamento fica ligado ao pedido (pedido.Pagamento) e é salvo no banco junto com ele.
     */
    public class PagamentoService
    {
        public Pagamento RegistrarPagamento(Pedido pedido, IFormaPagamento formaPagamento)
        {
            // Regra do 1x1: um pedido não pode ser pago duas vezes
            if (pedido.Pagamento != null)
                throw new InvalidOperationException("Este pedido já foi pago.");

            if (pedido.ValorTotal <= 0)
                throw new InvalidOperationException("O pedido não tem valor a pagar.");

            var pagamento = new Pagamento
            {
                FormaPagamento = formaPagamento.Nome,
                ValorPago = formaPagamento.CalcularValorFinal(pedido.ValorTotal),
                DataPagamento = DateTime.Now,
                Pedido = pedido
            };

            pedido.Pagamento = pagamento;
            return pagamento;
        }
    }
}

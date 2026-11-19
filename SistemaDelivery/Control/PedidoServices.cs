using Microsoft.EntityFrameworkCore;
using SistemaDelivery.DataBase;
using SistemaDelivery.Model;

namespace SistemaDelivery.Control;

// Regras do balcão: criar, editar, concluir e cancelar pedidos (Entity Framework).
// A busca por nome da comanda usa uma consulta SQL escrita à mão (FromSqlInterpolated).
public class PedidoService
{
    public List<Pedido> Listar(string? busca = null)
    {
        using var db = new DeliveryDBContext();

        if (string.IsNullOrWhiteSpace(busca))
            return db.Pedidos.AsNoTracking().OrderByDescending(p => p.IdPedido).ToList();

        var termo = $"%{busca.Trim()}%";
        return db.Pedidos
                 .FromSqlInterpolated($"SELECT * FROM Pedidos WHERE NomeComanda LIKE {termo}")
                 .AsNoTracking()
                 .ToList()
                 .OrderByDescending(p => p.IdPedido)
                 .ToList();
    }

    public Pedido ObterComItens(int idPedido)
    {
        using var db = new DeliveryDBContext();
        return db.Pedidos.AsNoTracking()
                 .Include(p => p.Itens).ThenInclude(i => i.Produto)
                 .FirstOrDefault(p => p.IdPedido == idPedido)
               ?? throw new RegraDeNegocioException("Pedido não encontrado.");
    }

    public static decimal CalcularTotal(IEnumerable<ItemPedido> itens)
        => itens.Sum(i => i.PrecoUnitario * i.Quantidade);

    public static int CalcularTempoEspera(IEnumerable<ItemPedido> itens)
        => itens.Select(i => i.Produto?.TempoPreparoMinutos ?? 0).DefaultIfEmpty(0).Max();

    public Pedido Salvar(int? idPedido, string? nomeComanda, IEnumerable<ItemPedido> itens)
    {
        var nome = (nomeComanda ?? "").Trim();
        if (nome.Length == 0)
            throw new RegraDeNegocioException("Informe o nome da comanda.");

        var lista = itens.ToList();
        if (lista.Count == 0)
            throw new RegraDeNegocioException("Adicione ao menos um produto ao pedido.");
        if (lista.Any(i => i.Quantidade <= 0))
            throw new RegraDeNegocioException("A quantidade de cada produto deve ser maior que zero.");

        using var db = new DeliveryDBContext();

        Pedido pedido;
        if (idPedido is null)
        {
            pedido = new Pedido { DataHora = DateTime.Now, Status = StatusPedido.EmAndamento };
            db.Pedidos.Add(pedido);
        }
        else
        {
            pedido = db.Pedidos.Include(p => p.Itens).FirstOrDefault(p => p.IdPedido == idPedido.Value)
                     ?? throw new RegraDeNegocioException("Pedido não encontrado.");

            if (pedido.Status != StatusPedido.EmAndamento)
                throw new RegraDeNegocioException("Somente pedidos em andamento podem ser editados.");

            db.ItensPedidos.RemoveRange(pedido.Itens.ToList());
            pedido.Itens.Clear();
        }

        pedido.NomeComanda = nome;

        foreach (var item in lista)
        {
            if (!db.Produtos.Any(p => p.IdProduto == item.ProdutoId))
                throw new RegraDeNegocioException("Um dos produtos do pedido não existe mais no cardápio.");

            pedido.Itens.Add(new ItemPedido
            {
                ProdutoId = item.ProdutoId,
                Quantidade = item.Quantidade,
                PrecoUnitario = item.PrecoUnitario 
            });
        }

        pedido.ValorTotal = CalcularTotal(pedido.Itens);

        db.SaveChanges();
        return pedido;
    }

    public void Concluir(int idPedido) => AlterarStatus(idPedido, StatusPedido.Entregue);

    public void Cancelar(int idPedido) => AlterarStatus(idPedido, StatusPedido.Cancelado);

    private static void AlterarStatus(int idPedido, string novoStatus)
    {
        using var db = new DeliveryDBContext();

        var pedido = db.Pedidos.Find(idPedido)
                     ?? throw new RegraDeNegocioException("Pedido não encontrado.");

        if (pedido.Status != StatusPedido.EmAndamento)
            throw new RegraDeNegocioException("Este pedido já foi finalizado.");

        pedido.Status = novoStatus;
        db.SaveChanges();
    }
}

using System.Globalization;
using Microsoft.EntityFrameworkCore;
using SistemaDelivery.DataBase;
using SistemaDelivery.Model;

namespace SistemaDelivery.Control
{
    // CRUD de Produtos (cardápio) usando o Entity Framework
    public class ProdutoService
    {
        private static readonly CultureInfo PtBr = new("pt-BR");

        public List<Produto> Listar(string? busca = null)
        {
            using var db = new DeliveryDBContext();
            var produtos = db.Produtos.AsNoTracking().OrderBy(p => p.IdProduto).ToList();

            if (string.IsNullOrWhiteSpace(busca))
                return produtos;

            // A busca é feita em memória porque Categoria não é coluna do banco (vem do tipo do produto)
            var termo = busca.Trim();
            return produtos
                .Where(p => p.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase)
                         || p.Categoria.Contains(termo, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void Salvar(int? id, string? nome, string? categoria, string? preco, string? tempo)
        {
            nome = (nome ?? "").Trim();
            if (nome.Length == 0)
                throw new RegraDeNegocioException("Informe o nome do produto.");
            if (categoria is null || !CategoriaProduto.Todas.Contains(categoria))
                throw new RegraDeNegocioException("Selecione uma categoria.");

            var valor = LerPreco(preco);
            var minutos = CategoriaProduto.EhBebida(categoria) ? 0 : LerTempo(tempo);

            using var db = new DeliveryDBContext();

            var idAtual = id ?? 0;
            if (db.Produtos.Any(p => p.Nome == nome && p.IdProduto != idAtual))
                throw new RegraDeNegocioException("Já existe um produto com esse nome.");

            Produto produto;
            if (id is null)
            {
                produto = CriarPorCategoria(categoria);
                db.Produtos.Add(produto);
            }
            else
            {
                produto = db.Produtos.Find(id.Value)
                          ?? throw new RegraDeNegocioException("Produto não encontrado.");

                // Cada categoria é uma classe diferente (herança), então não dá para trocar Lanche por Bebida etc.
                if (produto.GetType() != CriarPorCategoria(categoria).GetType())
                    throw new RegraDeNegocioException(
                        "Não é possível trocar o tipo do produto (lanche, acompanhamento ou bebida). Exclua e cadastre novamente.");
            }

            produto.Nome = nome;
            produto.Valor = valor;

            switch (produto)
            {
                case Lanche lanche:
                    lanche.tempoDePreparo = minutos;
                    break;
                case Acompanhamento acompanhamento:
                    acompanhamento.tempoPreparado = minutos;
                    break;
                case Bebida bebida:
                    bebida.EhAlcoolica = categoria == CategoriaProduto.BebidaAlcoolica;
                    break;
            }

            db.SaveChanges();
        }

        public void Excluir(int id)
        {
            using var db = new DeliveryDBContext();

            if (db.ItensPedidos.Any(i => i.ProdutoId == id))
                throw new RegraDeNegocioException(
                    "Este produto já foi usado em pedidos e não pode ser excluído.");

            var produto = db.Produtos.Find(id)
                          ?? throw new RegraDeNegocioException("Produto não encontrado.");

            db.Produtos.Remove(produto);
            db.SaveChanges();
        }

        private static Produto CriarPorCategoria(string categoria) => categoria switch
        {
            CategoriaProduto.Lanche => new Lanche(),
            CategoriaProduto.Acompanhamento => new Acompanhamento(),
            _ => new Bebida()
        };

        private static decimal LerPreco(string? texto)
        {
            var limpo = (texto ?? "").Replace("R$", "").Trim();
            if (limpo.Length == 0)
                throw new RegraDeNegocioException("Informe o preço.");

            var culturaDoTexto = limpo.Contains(',') ? PtBr : CultureInfo.InvariantCulture;

            if (!decimal.TryParse(limpo, NumberStyles.Number, culturaDoTexto, out var valor) || valor <= 0)
                throw new RegraDeNegocioException("Preço inválido. Exemplo: 20,00");

            return decimal.Round(valor, 2);
        }

        private static int LerTempo(string? texto)
        {
            var limpo = (texto ?? "").ToLowerInvariant().Replace("min", "").Trim();

            if (!int.TryParse(limpo, out var minutos) || minutos <= 0)
                throw new RegraDeNegocioException("Informe o tempo de preparo em minutos. Exemplo: 20");

            return minutos;
        }
    }
}

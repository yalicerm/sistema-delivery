using System;
using Microsoft.EntityFrameworkCore;
using SistemaDelivery.Model;

namespace SistemaDelivery.DataBase
{
    public class DeliveryDBContext : DbContext
    {
        //Definindo as tabelas do nosso banco de dados (Criadas através do Migrations)
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Lanche> Lanches { get; set; }
        public DbSet<Acompanhamento> Acompanhamentos { get; set; }
        public DbSet<Bebida> Bebidas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedidos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }


        // Configurando a conexão com o banco de dados (Encontra no README do próprio pacote SQL)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer(ConexaoBanco.StringConexao);

            }
        }

        //Fazendo uso de Fluent API para configurar as entidades do banco de dados
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Configurações das chaves primárias das Classes Produto, Pedido e ItemPedido
            modelBuilder.Entity<Produto>().HasKey(p => p.IdProduto);
            modelBuilder.Entity<Pedido>().HasKey(p => p.IdPedido);
            modelBuilder.Entity<ItemPedido>().HasKey(i => i.IdItem);

            //Configurações das chaves estrangeiras das Classes ItemPedido

            // Configurando o relacionamento entre ItemPedido e Pedido
            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade); //Apaga os itens do pedido quando o pedido for deletado

            // Configurando o relacionamento entre ItemPedido e Produto
            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Produto)
                .WithMany()
                .HasForeignKey(i => i.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict); //Impede apagar um produto se ele estiver associado a algum item de pedido

            // Configurando o relacionamento 1x1 entre Pedido e Pagamento
            modelBuilder.Entity<Pagamento>().HasKey(pg => pg.IdPagamento);
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Pagamento)
                .WithOne(pg => pg.Pedido)
                .HasForeignKey<Pagamento>(pg => pg.PedidoId) //A FK fica no Pagamento; o EF cria um índice ÚNICO nela (garante o 1x1)
                .OnDelete(DeleteBehavior.Cascade);           //Apagou o pedido, apaga o pagamento dele

            modelBuilder.Entity<Pagamento>(pagamento =>
            {
                pagamento.Property(pg => pg.FormaPagamento).HasMaxLength(20).IsRequired();
                pagamento.Property(pg => pg.ValorPago).HasColumnType("decimal(10,2)");
            });

            // Fazendo algunas configurações adicionais --> Conversão de decimal (Evitar bugs número no banco dados)
            modelBuilder.Entity<Produto>()
                .Property(p => p.Valor)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Pedido>()
                .Property(p => p.ValorTotal)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Pedido>()
                .Property(p => p.NomeComanda)
                .HasMaxLength(100);

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.PrecoUnitario)
                .HasColumnType("decimal(10,2)");

            //Configuração da tabela de Usuarios
            modelBuilder.Entity<Usuario>(usuario =>
            {
                usuario.Property(u => u.Cpf).HasMaxLength(11).IsRequired(); //Definindo o tamanho máximo do CPF como 11 caracteres e tornando o campo obrigatório
                usuario.HasIndex(u => u.Cpf).IsUnique(); //Garantindo que o CPF seja único no banco de dados
                usuario.Property(u => u.Nome).HasMaxLength(100).IsRequired(); //Definindo o tamanho máximo do Nome como 100 caracteres e tornando o campo obrigatório
                usuario.Property(u => u.Senha).HasMaxLength(50).IsRequired(); //Definindo o tamanho máximo da Senha como 50 caracteres e tornando o campo obrigatório
                usuario.Property(u => u.Cargo).HasMaxLength(20).IsRequired(); //Definindo o tamanho máximo do Cargo como 20 caracteres e tornando o campo obrigatório

                //Criando alguns Usuários Iniciais para testar a página login
                usuario.HasData(
                    new Usuario { Id = 1, Cpf = "11111111111", Senha = "gerente123", Nome = "Gerente Teste", Cargo = "Gerente" },
                    new Usuario { Id = 2, Cpf = "22222222222", Senha = "atendente123", Nome = "Atendente Teste", Cargo = "Atendente" });
            });
        }

        //Chamado ao abrir o sistema (App.xaml.cs): cria o banco e aplica as migrations pendentes (Code First).
        //Assim ninguém precisa rodar o Update-Database manualmente para usar o sistema.
        public static void Inicializar()
        {
            using var db = new DeliveryDBContext();
            db.Database.Migrate();
        }

    }
}





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


        // Configurando a conexão com o banco de dados (Encontra no README do próprio pacote SQL)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DeliveryDB;Trusted_Connection=True;");

            }
        }

        //Fazendo uso de Fluent API para configurar as entidades do banco de dados
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Configurações das chaves primárias das Classes Produto, Pedido e ItemPedido
            modelBuilder.Entity<Produto>().HasKey(p => p.Id);
            modelBuilder.Entity<Pedido>().HasKey(p => p.IdPedido);
            modelBuilder.Entity<ItemPedido>().HasKey(i => i.IdItem);

            //Configurações das chaves estrangeiras das Classes ItemPedido

            // Configurando o relacionamento entre ItemPedido e Pedido
            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(i => i.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade); //Apaga os itens do pedido quando o pedido for deletado

            // Configurando o relacionamento entre ItemPedido e Produto
            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Produto)
                .WithMany()
                .HasForeignKey(i => i.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict); //Impede apagar um produto se ele estiver associado a algum item de pedido

            // Fazendo algunas configurações adicionais --> Conversão de decimal (Evitar bugs número no banco dados)
            modelBuilder.Entity<Produto>()
                .Property(p => p.Valor)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Pedido>()
                .Property(p => p.ValorTotal)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.PrecoUnitario)
                .HasColumnType("decimal(10,2)");
        }

    }
}




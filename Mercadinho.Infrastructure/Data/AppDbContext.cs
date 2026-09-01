using System;
using System.Collections.Generic;
using System.Text;
using Market.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Mercadinho.Infrastructure.Data
{
    public class AppDbContext : DbContext //Abrir e fechar conexões SQL server,
                                          //Rastrear Change tracking (alterações nos objetos em memória), comandos de lietura e gravação (LINQ-> SQL) e gravação(saveChanges())*/
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) /* É uma "caixa de configurações". 
                                                                    * Nela virão a string de conexão (onde o banco está instalado, usuário e senha) 
                                                                    * e o tipo de provedor (SQL Server).*/

                                                             :
                                                             base(options) { }/*Pega essas configurações e as repassa diretamente para o construtor da classe-mãe (DbContext), 
                                                                                                       * permitindo que a biblioteca inicialize a 
                                                                                                       * conexão sem você precisar escrever código manual de rede.*/
        // Tabelas que serão criadas no SQL Server
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Batch> Batches { get; set; }


        //A partir de agora dar as instruções detalhadas de como cada coluna deve ser criada no banco de dados.
        // 1. Assinatura do método: gancho de customização do modelo de banco
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            // 2. Execução da configuração padrão da classe-mãe
            base.OnModelCreating(modelBuilder);

            // 3. Mapeamento da entidade produto e da prop PrecoDeCusto
            modelBuilder.Entity<Product>()
            .Property(p => p.CostPrice)
            .HasPrecision(18, 2); // Define DECIMAL(18, 2) no SQL Server
                                  // Evita arredondamentos involuntários e elimina o alerta de compilação do EF Core sobre tipos decimais sem escala definida.
            modelBuilder.Entity<Product>()
                .Property(p => p.SellingPrice)
                .HasPrecision(18, 2);
        }
    }
}

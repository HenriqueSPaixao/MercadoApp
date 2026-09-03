using Market.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = ""; //"" == string.Empty
        public string Barcode { get; set; } = string.Empty;
        public decimal CostPrice {  get; set; }
        public decimal SellingPrice { get; set; }
        public bool Active {  get; set; }

        //Chave estrangeira para Categoria
        public int CategoryId {  get; set; }
        public int Stock {  get; set; }
        public Category Category { get; set; } = null!;/*Sem essa linha: Você só tem o número do identificador (produto.CategoriaId = 1). 
                                                  * Se quiser saber o nome da categoria, precisará fazer outra busca manual no banco.
                                                  * Com essa linha: Você pode ler qualquer dado da categoria diretamente pelo produto: 
                                                  * string nome = produto.Categoria.Nome; // Ex: "Bebidas"*/

        public ICollection<Batch> Batches { get; set; } = new List<Batch>(); /* um produto pode ter vários lotes cadastrados para ele
                                                                         * ICollection serve para armazenar multiplos   objetos Lote para produto
                                                                         */

        public void AtualizarPrecos (decimal NewCostPrice, decimal NewSellingPrice) 
        {
            if (NewSellingPrice <= NewCostPrice)
                throw new InvalidOperationException("Preço de venda não pode ser menor ou igual ao preço de custo");
                
            CostPrice = NewCostPrice;
            SellingPrice = NewSellingPrice;
        }
              
    }
}

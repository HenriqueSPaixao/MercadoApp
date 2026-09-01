using System;
using System.Collections.Generic;
using System.Text;

namespace Market.Domain.Entities
{
    public class Batch
    {
        public int Id { get; set; }
        public string Registration { get; set; } = "";
        public DateTime FabricationDate {  get; set; }
        public DateTime ValidityDate { get; set; }
        public int Amount {  get; set; }

        //Foreing key
        public int ProductId {  get; set; } /*(Propriedade FK): É a linha que expõe a coluna do SQL diretamente no 
                                             * seu código C# para você manipular números de ID sem carregar objetos inteiros na memória.
                                             essa formatação identifica automaticamente*/

        public required Product Product { get; set; } //É a linha que avisa ao EF Core que existe um relacionamento entre as tabelas.
        //public Product Product { get; set; } = null!; Mesma coisa


        //regra de comportamento
        public bool IsExpired() => DateTime.UtcNow > ValidityDate; /* Expression-Bodied Member
                                                                      * public bool EstaVencido()
                                                                        {
                                                                            return DateTime.UtcNow > DataValidade;
                                                                        }   
                                                                        Metodo de consulta, não
                                                                        Métodos de Ação/Comportamento (Comandos): Como DarBaixaEstoque(), VenderItem(), CadastrarLote().
                                                                        Objetivo: Executar uma operação que altera o estado do sistema. usar throw*/

    }
}

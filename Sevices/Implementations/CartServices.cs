using Market.Domain.Entities;
using Market.Infrastructure.Data;
using Market.Services.DTOs;
using Market.Sevices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace Market.Services.Services
{
    public class CartServices : ICartServices
    {
        // Armazena a referência para a sessão com o banco de dados (EF Core).
        // O modificador 'readonly' garante que essa variável só pode ser atribuída no construtor.
        private readonly AppDbContext _context;

        // CONSTRUTOR: Injeção de Dependência.
        // O ASP.NET Core entrega uma instância pronta do AppDbContext com a conexão aberta.
        public CartServices(AppDbContext context)
        {
            _context = context;
        }
        // [CREATE] Cria um novo carrinho vazio no banco e devolve os dados
        //
        //
        // dele
        public async Task<CartResponseDto> CreateCartAsync()
        {
            // PASSO 1: Criamos o objeto na memória RAM do computador.
            // Neste momento, o banco de dados NÃO sabe que ele existe.
            // O carrinho nasce com valores padrão: Id = 0, IsClosed = false e Lista de Itens vazia.
            var cart = new Cart();
            // PASSO 2: Avisamos o Entity Framework Core para monitorar este objeto.
            // O EF coloca o carrinho na sua lista interna de "coisas a fazer" (Change Tracker).
            // Atenção: AINDA NÃO foi enviado nenhum comando para o banco de dados aqui.
            _context.Carts.Add(cart);

            // PASSO 3: Agora sim a mágica do banco de dados acontece.
            // 1. O EF Core monta o comando SQL: "INSERT INTO Carts (...) VALUES (...)"
            // 2. Envia para o SQL Server via rede.
            // 3. A palavra 'await' libera a CPU do servidor para atender outros clientes enquanto o banco processa.
            // 4. O banco salva o registro, gera o ID oficial (ex: Id = 1, 2, 3...) e preenche de volta no 'cart.Id'.
            await _context.SaveChangesAsync();

            // PASSO 4: Não devolvemos a classe do banco (Entidade) diretamente para o cliente por segurança.
            // Convertemos a Entidade 'cart' (que agora já tem um ID gerado pelo banco) 
            // em um objeto de resposta limpo ('CartResponseDto').
            return MapToDto(cart);
            throw new NotImplementedException();
        }

        // Método auxiliar privado que converte a Entidade de Domínio (Cart) no DTO de resposta (CartResponseDto)
        private static CartResponseDto MapToDto(Cart cart) /*CartResponseDto não é uma variável, ele é o tipo de retorno da função (assim como int, string ou bool).
                                                            Pense no método MapToDto como um "tradutor" ou "preenchedor de formulário".*/
        {
            {
                // 1. Instancia e devolve diretamente um novo objeto DTO de resposta.
                // Usamos a sintaxe de 'Object Initializer' (chaves logo após o new) para preencher os campos.
                return new CartResponseDto
                {
                    // 2. Copia o identificador único gerado pelo banco para o DTO.
                    Id = cart.Id,

                    // 3. Informa ao cliente se o carrinho está aberto para compras ou já foi finalizado.
                    IsClosed = cart.IsClosed,

                    // 4. Copia o valor total em dinheiro calculado pela regra de negócio da Entidade.
                    TotalAmount = cart.TotalAmount,

                    // 5. Copia a quantidade somada de todos os itens presentes no carrinho.
                    TotalQuantity = cart.TotalQuantity,

                    // 6. LINQ .Select: Funciona como um laço "foreach" de transformação.
                    // Pega cada elemento 'item' da lista interna da Entidade e projeta em um novo 'CartItemResponseDto'.
                    Items = cart.Items.Select(item => new CartItemResponseDto // Para cada item declarado ele salva em uma lista
                    {
                        // Identificador do produto vinculado àquele item.
                        ProductId = item.ProductId,

                        // Operador Ternário (condição ? se_verdadeiro : se_falso):
                        // Proteção contra falhas: se o EF Core carregou o produto (.ThenInclude), pega o Nome.
                        // Se o produto veio nulo por algum motivo, preenche com texto vazio ("") para evitar erro de tela preta (NullReferenceException).
                        ProductName = item.Product != null ? item.Product.Name : string.Empty,

                        // Preço unitário registrado no momento em que o item entrou no carrinho.
                        UnitPrice = item.UnitPrice,

                        // Quantidade de unidades daquele produto.
                        Quantity = item.Quantity,

                        // Valor calculado daquele item específico (UnitPrice * Quantity).
                        Subtotal = item.Subtotal

                        // 7. .ToList(): Materializa a transformação imediatamente.
                        // Transforma o fluxo de leitura gerado pelo .Select em uma List<CartItemResponseDto> alocada na memória RAM.
                    }).ToList()
                };
            }
        }

        // Adiciona a palavra 'async' na assinatura para permitir o uso de 'await'
        public async Task AddItemAsync(int cartId, AddItemDto dto)
        {
            // PASSO 1: Busca o carrinho no banco de dados.
            // O '.Include(c => c.Items)' é OBRIGATÓRIO aqui!
            // Por que? Para o C# carregar a lista de produtos que já estão dentro do carrinho.
            // Sem isso, a lista 'Items' viria vazia da memória e o sistema não saberia
            // se o produto já existia lá dentro para apenas somar a quantidade.
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            // PASSO 2: Validação defensiva do carrinho.
            // Se o ID digitado não existir no banco, interrompe imediatamente.
            if (cart == null)
                throw new KeyNotFoundException($"Carrinho com ID {cartId} não encontrado.");

            // PASSO 3: Busca o produto no banco de dados pelo ProductId que veio no DTO.
            // Usamos 'FindAsync' que é uma busca rápida e otimizada por chave primária.
            var product = await _context.Products.FindAsync(dto.ProductId);

            // PASSO 4: Validação defensiva do produto.
            // Garante que o cliente não tente adicionar um produto fantasma que não existe na loja.
            if (product == null)
                throw new KeyNotFoundException($"Produto com ID {dto.ProductId} não encontrado.");

            // PASSO 5: DELEGAÇÃO PARA O DOMÍNIO (DDD).
            // O serviço NÃO faz cálculos e NÃO adiciona na lista na mão.
            // Ele apenas entrega o produto e a quantidade para a entidade 'Cart'.
            // A própria entidade valida:
            //  - Se o carrinho está fechado (lança erro se estiver).
            //  - Se a quantidade é maior que zero.
            //  - Se o produto já está na lista (se já estiver, só incrementa a quantidade).
            //  - Se for um produto novo, cria um novo 'CartItem' e adiciona na lista.
            cart.AddItem(product, dto.Quantity);

            // PASSO 6: Persistência no banco de dados.
            // O EF Core (Change Tracker) percebe o que aconteceu na memória e gera o SQL correto:
            //  - Se foi um item novo: executa "INSERT INTO CartItems..."
            //  - Se o item já existia: executa "UPDATE CartItems SET Quantity = ... "
            // O 'await' libera a CPU enquanto o banco grava as alterações no disco.
            await _context.SaveChangesAsync();
        }

        public Task CloseCartAsync(int cartId)
        {
            throw new NotImplementedException();
        }

        public Task<CartResponseDto> GetCartByIdAsync(int cartId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveItemAsync(int cartId, int productId)
        {
            var cart = await _context.Carts
                   .Include(c => c.Items)
                   .FirstOrDefaultAsync(c => c.Id == productId);
            if (cart == null)
            {
                throw new NotImplementedException($"Cart {cartId} not found\n");
            }
            cart.RemoveItem(productId);
            await _context.SaveChangesAsync();
        }

        // Esvazia os itens, mas mantém o registro do carrinho no banco
        public async Task ClearCartAsync(int cartId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null)
                throw new KeyNotFoundException($"Carrinho com ID {cartId} não encontrado.");

            cart.Clear();

            await _context.SaveChangesAsync();
        }

        // Remove o carrinho por completo (registro pai e itens filhos)
        public async Task DeleteCartAsync(int cartId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.Id == cartId);

            if (cart == null)
                throw new KeyNotFoundException($"Carrinho com ID {cartId} não encontrado.");

            _context.Carts.Remove(cart);

            await _context.SaveChangesAsync();
        }
    }
}

using Market.Domain.Entities;
using Market.Infrastructure.Data;
using Market.Domain.DTOs;
using Market.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Market.Domain.Implementations;

public class ProductServices : IProductServices
{
    private readonly AppDbContext _context;

    public ProductServices(AppDbContext context)
    {
        _context = context;
    }

    // =========================================================================
    // 1. CRIAR PRODUTO
    // =========================================================================
    public async Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto)
    {
        // PASSO 1: Instancia a entidade de domínio com os dados validados do DTO
        var product = new Product
        {
            Name = dto.Name,
            SellingPrice = dto.SellingPrice,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId
        };

        // PASSO 2: Registra a nova entidade no rastreador do EF Core (Estado: Added)
        _context.Products.Add(product);

        // PASSO 3: Envia o INSERT INTO para o banco e preenche o product.Id gerado
        await _context.SaveChangesAsync();

        // PASSO 4: Devolve o DTO formatado para quem chamou a API
        return MapToDto(product);
    }

    // =========================================================================
    // 2. BUSCAR PRODUTO POR ID
    // =========================================================================
    public async Task<ProductResponseDto> GetProductByIdAsync(int id)
    {
        // PASSO 1: Busca o produto trazendo a Categoria associada (Eager Loading via JOIN)
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        // PASSO 2: Validação defensiva caso o registro não exista
        if (product == null)
            throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");

        // PASSO 3: Mapeia e retorna
        return MapToDto(product);
    }

    // =========================================================================
    // 3. LISTAR TODOS OS PRODUTOS
    // =========================================================================
    public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
    {
        // PASSO 1: AsNoTracking otimiza a leitura na memória (não ativa o Change Tracker)
        var products = await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .ToListAsync();

        // PASSO 2: Converte a lista inteira de entidades para lista de DTOs via LINQ
        return products.Select(MapToDto).ToList();
    }

    // =========================================================================
    // 4. ATUALIZAR PRODUTO
    // =========================================================================
    public async Task<ProductResponseDto> UpdateProductAsync(int id, UpdateProductDto dto)
    {
        // PASSO 1: Busca o produto existente no banco para ser rastreado
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");

        // PASSO 2: Atualiza as propriedades na memória
        product.Name = dto.Name;
        product.SellingPrice = dto.SellingPrice;
        product.Stock = dto.Stock;
        product.CategoryId = dto.CategoryId;

        // PASSO 3: O Change Tracker detecta quais campos mudaram e gera o UPDATE SQL correspondente
        await _context.SaveChangesAsync();

        return MapToDto(product);
    }

    // =========================================================================
    // 5. DELETAR PRODUTO
    // =========================================================================
    public async Task DeleteProductAsync(int id)
    {
        // PASSO 1: Localiza o produto
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            throw new KeyNotFoundException($"Produto com ID {id} não encontrado.");

        // PASSO 2: Marca como Deleted no Change Tracker
        _context.Products.Remove(product);

        // PASSO 3: Executa "DELETE FROM Products WHERE Id = @id"
        await _context.SaveChangesAsync();
    }

    // =========================================================================
    // 6. MÉTODO PRIVADO: MAPEAMENTO ENTIDADE -> DTO
    // =========================================================================
    private static ProductResponseDto MapToDto(Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            SellingPrice = product.SellingPrice,
            Stock = product.Stock,
            CategoryId = product.CategoryId,
            CategoryName = product.Category != null ? product.Category.Name : string.Empty
        };
    }
}
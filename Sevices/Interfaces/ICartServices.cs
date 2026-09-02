using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Market.Services.DTOs;// Permite enxergar o CartResponseDto e AddItemDto

namespace Market.Sevices.Interfaces
{
    public interface ICartServices
    {
        //task é promessa que será implementado mas de forma assincrona: 
        //A Task existe em classes normais, consoles, controllers ou interfaces. Ela trata de tempo de espera de hardware/rede (I/O), e não de arquitetura de código.
        /*O SQL Server leva, por exemplo, 30 milissegundos para procurar o carrinho no disco e responder via rede.

        Sem Task (Síncrono): A linha de execução do processador (Thread) fica totalmente paralisada, de braços cruzados, esperando o banco responder.
        Se chegarem 100 requisições simultâneas, o servidor trava.

        Com Task (Assíncrono): O C# avisa ao sistema operacional: "Vou disparar essa busca no banco. 
        Libere o processador para atender outras pessoas. Quando o SQL Server responder, me avise para eu continuar daqui."*/
    Task<CartResponseDto> CreateCartAsync();// para o processador não perder tempo esperando uma operação sincrona
    Task<CartResponseDto> GetCartByIdAsync(int cartId);
    Task AddItemAsync(int cartId, AddItemDto dto);
    Task ClearCartAsync(int cartId);
    Task DeleteCartAsync(int cartId);
    Task RemoveItemAsync(int cartId, int itemId);
    Task CloseCartAsync(int cartId);

}
}

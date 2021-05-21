using ApiCatalogoJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repositories
{
    public interface IOrderRepository : IDisposable
    {
        Task<List<Order>> Obter(int pagina, int quantidade);
        Task<Order> Obter(Guid id);
        Task Inserir(Order order);
        Task Remover(Guid id);
    }
}

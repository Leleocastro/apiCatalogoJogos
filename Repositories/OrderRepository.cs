using ApiCatalogoJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private static Dictionary<Guid, Order> orders = new Dictionary<Guid, Order>()
        {
            
        };
        public Task<List<Order>> Obter(int pagina, int quantidade)
        {
            return Task.FromResult(orders.Values.Skip((pagina - 1) * quantidade).Take(quantidade).ToList());
        }

        public Task<Order> Obter(Guid id)
        {
            if (!orders.ContainsKey(id))
                return Task.FromResult<Order>(null);

            return Task.FromResult(orders[id]);
        }

        public Task Inserir(Order order)
        {
            orders.Add(order.Id, order);
            return Task.CompletedTask;
        }

        public Task Remover(Guid id)
        {
            orders.Remove(id);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //Fechar conexão com o banco
        }
    }
}

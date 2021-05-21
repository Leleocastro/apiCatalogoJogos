using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Services
{
    public interface IOrderService : IDisposable
    {
        Task<List<OrderViewModel>> Obter(int pagina, int quantidade);
        Task<OrderViewModel> Obter(Guid id);
        Task<OrderViewModel> Inserir(OrderInputModel order);
        Task Remover(Guid id);
    }
}

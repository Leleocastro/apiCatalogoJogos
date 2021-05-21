using ApiCatalogoJogos.Entities;
using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Repositories;
using ApiCatalogoJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderViewModel>> Obter(int pagina, int quantidade)
        {
            var orders = await _orderRepository.Obter(pagina, quantidade);
            
            List<CartItem> cartItems = new List<CartItem>();


            return orders.Select(order => new OrderViewModel
            {
                Id = order.Id,
                Date = order.Date,
                Jogos = order.Jogos.Select(cartItem => new CartItem 
                { 
                    Id = cartItem.Id,
                    ImageUrl = cartItem.ImageUrl,
                    JogoId = cartItem.JogoId,
                    Nome = cartItem.Nome,
                    Preco = cartItem.Preco,
                    Frete = cartItem.Frete,
                    Quantidade = cartItem.Quantidade
                }).ToList(),
                Total = order.Total
            }).ToList();
        }
        public async Task<OrderViewModel> Obter(Guid id)
        {
            var order = await _orderRepository.Obter(id);

            if (order == null)
                return null;

            return new OrderViewModel
            {
                Id = order.Id,
                Total = order.Total,
                Jogos = order.Jogos,
                Date = order.Date
            };
        }

        public async Task<OrderViewModel> Inserir(OrderInputModel order)
        {
            List<CartItem> cartItems = new List<CartItem>();
            foreach (CartItem cart in order.Jogos) 
            {
                var cartInsert = new CartItem
                {
                    Id = cart.Id,
                    ImageUrl = cart.ImageUrl,
                    JogoId = cart.JogoId,
                    Nome = cart.Nome,
                    Preco = cart.Preco,
                    Frete = cart.Frete,
                    Quantidade = cart.Quantidade,
                };
                cartItems.Add(cartInsert);
            }
            var orderInsert = new Order
            {
                Id = Guid.NewGuid(),
                Total = order.Total,
                Jogos = cartItems,
                Date = order.Date
            };

            await _orderRepository.Inserir(orderInsert);

            return new OrderViewModel
            {
                Id = orderInsert.Id,
                Total = order.Total,
                Jogos = cartItems,
                Date = order.Date
            };
        }

        public async Task Remover(Guid id)
        {
            var order = await _orderRepository.Obter(id);

            if (order == null)
                throw new Exception();

            await _orderRepository.Remover(id);
        }

        public void Dispose()
        {
            _orderRepository?.Dispose();
        }
    }
}

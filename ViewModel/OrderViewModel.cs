using ApiCatalogoJogos.Entities;
using System;
using System.Collections.Generic;

namespace ApiCatalogoJogos.ViewModel
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        public List<CartItem> Jogos { get; set; }
        public double Total { get; set; }
        public string Date { get; set; }
    }
}

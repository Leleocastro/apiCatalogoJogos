using System;
using System.Collections.Generic;

namespace ApiCatalogoJogos.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public List<CartItem> Jogos { get; set; }
        public double Total { get; set; }
        public string Date { get; set; }
    }
}

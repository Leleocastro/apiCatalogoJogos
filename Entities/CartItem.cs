using System;
using System.Collections.Generic;

namespace ApiCatalogoJogos.Entities
{
    public class CartItem
    {
        public string Id { get; set; }
        public string JogoId { get; set; }
        public string Nome { get; set; }
        public string ImageUrl { get; set; }
        public int Quantidade { get; set; }
        public double Preco { get; set; }
        public double Frete { get; set; }
    }
}
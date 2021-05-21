using ApiCatalogoJogos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.InputModel
{
    public class OrderInputModel
    {
        [Required]
        public List<CartItem> Jogos { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public double Total { get; set; }

    }
}

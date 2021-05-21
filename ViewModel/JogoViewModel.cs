using System;

namespace ApiCatalogoJogos.ViewModel
{
    public class JogoViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string ImageUrl { get; set; }
        public double Preco { get; set; }
    }
}

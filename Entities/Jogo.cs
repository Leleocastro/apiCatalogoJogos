using System;

namespace ApiCatalogoJogos.Entities
{
    public class Jogo
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string ImageUrl { get; set; }
        public double Preco { get; set; }
    }
}

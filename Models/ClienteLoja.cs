namespace RedeLojas.Models
{
    public class ClienteLoja
    {
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public int LojaId { get; set; }
        public Loja Loja { get; set; }
    }
}
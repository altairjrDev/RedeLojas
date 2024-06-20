using System.ComponentModel.DataAnnotations.Schema;

namespace RedeLojas.Models
{
    [Table("Lojas")]
    public class Loja
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public List<ClienteLoja> ClienteLojas { get; set; }
    }
}

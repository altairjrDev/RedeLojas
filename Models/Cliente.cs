using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedeLojas.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O Nome é obrigatório")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O CPF é obrigatório")]
        public string CPF { get; set; }
        public DateTime? DataNascimento { get; set; }

        public List<ClienteLoja> ClienteLojas { get; set; }
    
    }
}

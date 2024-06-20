using RedeLojas.Models;

namespace RedeLojas.ViewModel
{
    public class LojaViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<int> ClientesSelecionados { get; set; }
        public List<Cliente> TodosClientes { get; set; }
    
    
    }
}


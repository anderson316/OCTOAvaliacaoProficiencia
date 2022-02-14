using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistribuidoraBebidas
{
    public class Cliente
    {
        public int ClienteID { get; set; }
        public string Nome { get; set; }
        public string? CPF { get; set; }
        public string? CNPJ { get; set; }
        public string RG { get; set; }
        public string IncricaoEstado { get; set; }
        public string Email { get; set; }
        public virtual List<Telefone> Telefones { get; set; }
        public virtual List<Endereco> Enderecos { get; set; }
    }
}

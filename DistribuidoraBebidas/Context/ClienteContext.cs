using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DistribuidoraBebidas.Context
{
    public class ClienteContext : DbContext
    {
        public ClienteContext():base ("DistribuidoraDB")
        {
            Database.SetInitializer<ClienteContext>(new CreateDatabaseIfNotExists<ClienteContext>());
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Telefone> Telefones { get; set; }
    }
}

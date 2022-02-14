using DistribuidoraBebidas.Context;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DistribuidoraBebidas
{
    public class ClientesService
    {
        public static List<Cliente> GetAll(string nome)
        {
            using (var db = new ClienteContext())
            {
                var clientes = db.Clientes
                        .Include(b => b.Enderecos)
                        .Include(b => b.Telefones)
                        .ToList();

                if (!string.IsNullOrEmpty(nome))
                {
                    var teste = clientes[0].Nome.Contains(nome);
                    var clientesfiltrados = clientes.Where(n => n.Nome.Contains(nome)).ToList();
                    return clientesfiltrados;
                }

                return clientes;
            }
        }

        public static string GetPaginas(int paginaAtual, string nome)
        {
            using (var db = new ClienteContext())
            {
                var clientes = db.Clientes
                    .Include(b => b.Enderecos)
                    .Include(b => b.Telefones)
                    .ToList();
                if (!string.IsNullOrEmpty(nome))
                {
                    clientes = clientes.Where(n => n.Nome.Contains(nome)).ToList();
                }
                var numeroPaginas = (int)Math.Ceiling(clientes.Count / (decimal)20);
                var proximaPagina = false;
                var podeVoltar = false;

                if (paginaAtual < numeroPaginas)
                {
                    proximaPagina = true;
                }
                if (paginaAtual > 1)
                {
                    podeVoltar = true;
                }
                string json = JsonConvert.SerializeObject(new
                {
                    paginaAtual,
                    numeroPaginas,
                    proximaPagina,
                    podeVoltar
                });
                return json;
            }
        }

        public static Cliente GetPorID(int id)
        {
            using (var db = new ClienteContext())
            {
                var cliente = db.Clientes
                    .Include(b => b.Enderecos)
                    .Include(b => b.Telefones)
                    .Where(c => c.ClienteID == id);
                if (cliente == null)
                {
                    return null;
                }
                return cliente.First();
            }
        }

        public static void CriarCliente(Cliente cliente)
        {
            using (var db = new ClienteContext())
            {
                try
                {
                    if (ChecarValidacoes(cliente))
                    {
                        db.Set<Cliente>().Add(cliente);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public static void EditarCliente(int id, Cliente cliente)
        {
            using (var db = new ClienteContext())
            {
                try
                {
                    if (ChecarValidacoes(cliente))
                    {
                        var entidade = db.Clientes.Find(id);
                        if (entidade == null)
                        {
                            throw new ArgumentException("ID não encontrado");
                            //return;
                        }
                        db.Entry<Cliente>(entidade).CurrentValues.SetValues(cliente);
                        //db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public static void DeletarCliente(int id)
        {
            using (var db = new ClienteContext())
            {
                try
                {
                    var entidade = db.Clientes.Find(id);
                    if (entidade == null)
                    {
                        throw new ArgumentException("ID não encontrado");
                    }
                    db.Clientes.Attach(entidade);
                    db.Clientes.Remove(entidade);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public static bool ChecarValidacoes(Cliente cliente)
        {
            if (String.IsNullOrEmpty(cliente.Nome) || String.IsNullOrEmpty(cliente.RG) || String.IsNullOrEmpty(cliente.Email))
            {
                throw new ArgumentNullException("Nome e documentos pessoais são obrigatórios");
            }
            else if (String.IsNullOrEmpty(cliente.CPF) && String.IsNullOrEmpty(cliente.CNPJ))
            {
                throw new ArgumentNullException("CPF ou cnpjFormatado são obrigatórios");
            }
            else if (cliente.Telefones.Count <= 0)
            {
                throw new ArgumentNullException("É obrigatório no minímo um telefone");
            }
            else if (cliente.Enderecos.Count <= 0)
            {
                throw new ArgumentNullException("É obrigatório no minímo um endereço");
            }
            else
            {
                if (new EmailAddressAttribute().IsValid(cliente.Email) == false)
                {
                    throw new ArgumentNullException("Email informado inválido");
                }
                if (!String.IsNullOrEmpty(cliente.CPF))
                {
                    if (!ValidarCPF(cliente.CPF))
                        throw new ArgumentNullException("CPF informado inválido");
                }
                if (!String.IsNullOrEmpty(cliente.CNPJ))
                {
                    if (!ValidarCNPJ(cliente.CNPJ))
                        throw new ArgumentNullException("cnpjFormatado informado inválido");
                }
                return true;
            }
        }

        private static bool ValidarCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;

            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                    return false;

            string cpfTemporario = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(cpfTemporario[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            cpfTemporario = cpfTemporario + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(cpfTemporario[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        private static bool ValidarCNPJ(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;

            string cnpjTemporario = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(cnpjTemporario[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            cnpjTemporario = cnpjTemporario + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(cnpjTemporario[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }
    }
}

namespace DistribuidoraBebidas
{
    public enum TiposEndereco { Cobranca, Comercial, Correspondencia, Entrega, Residencial}
    public class Endereco
    {
        public int EnderecoID { get; set; }
        public string TipoLogradouro { get; set; }
        public string Logradouro { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public TiposEndereco TipoEndereco { get; set; }
    }
}
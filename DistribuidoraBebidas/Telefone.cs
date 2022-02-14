namespace DistribuidoraBebidas
{
    public enum TiposTelefone {Celular, Residencial, Comercial};
    public class Telefone
    {
        public int TelefoneID { get; set; }
        public int DDD { get; set; }
        public string Numero { get; set; }
        public string Observacoes { get; set; }
        public TiposTelefone TipoTelefone { get; set; }
    }
}
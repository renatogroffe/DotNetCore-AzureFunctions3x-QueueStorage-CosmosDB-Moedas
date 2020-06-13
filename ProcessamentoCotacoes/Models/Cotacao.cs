namespace ProcessamentoCotacoes.Models
{
    public class Cotacao
    {
        private string _codigo;
        public string Codigo
        {
            get => _codigo;
            set => _codigo = value?.Trim().ToUpper();
        }        

        public double? Valor { get; set; }       
    }
}
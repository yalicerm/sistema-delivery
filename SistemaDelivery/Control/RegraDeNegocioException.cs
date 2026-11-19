namespace SistemaDelivery.Control
{
    // Exceção usada por todos os serviços quando uma regra de negócio é violada
    // (ex: "Informe o CPF", "Produto não encontrado"). A View mostra a mensagem para o usuário.
    public class RegraDeNegocioException : Exception
    {
        public RegraDeNegocioException(string mensagem) : base(mensagem) { }
    }
}

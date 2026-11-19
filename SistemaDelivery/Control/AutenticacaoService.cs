using System;
using SistemaDelivery.Model;
using SistemaDelivery.DataBase;

/*
    View chama o Autenticar(cpf, senha), que devolve o usuário logado.
    Se o login der errado (campo vazio, CPF inválido, usuário não encontrado ou senha incorreta),
    ele lança uma RegraDeNegocioException com a mensagem para mostrar na tela.
 */

namespace SistemaDelivery.Control
{
    public class AutenticacaoService
    {
        //Chamou a interface IUsuarioRepository para buscar o usuário no banco de dados através do CPF e Senha (SOLID)
        private readonly IUsuarioRepository _usuarioRepository;

        //Construtor usado pela View: usa o LoginRepository (SQL puro)
        public AutenticacaoService() : this(new LoginRepository()) { }

        public AutenticacaoService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public Usuario Autenticar(string? cpf, string? senha)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                throw new RegraDeNegocioException("Informe o CPF.");
            if (string.IsNullOrEmpty(senha))
                throw new RegraDeNegocioException("Informe a senha.");

            //Melhorar a busca no banco dados, tirando os . e - do CPF
            string cpfFormatado = cpf.Replace(".", "").Replace("-", "").Trim();

            //Verificando se o CPF tem 11 dígitos
            if (cpfFormatado.Length != 11)
                throw new RegraDeNegocioException("CPF inválido. Digite os 11 números do CPF.");

            //Se tudo deu certo, ele vai chamar o método BuscarUsuario da interface IUsuarioRepository para buscar o usuário no banco de dados através do CPF e Senha
            return _usuarioRepository.BuscarUsuario(cpfFormatado, senha)
                   ?? throw new RegraDeNegocioException("CPF ou senha inválidos.");
        }
    }
}

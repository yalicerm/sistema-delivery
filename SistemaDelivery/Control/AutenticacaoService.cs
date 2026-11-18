using System;
using SistemaDelivery.Model;
using SistemaDelivery.DataBase;

/* 
    View chama o Autenticar(cpf,senha) que devolve o login deu certo
    Se voltar Null, o login deu errado (usuario não encontrada ou cpf/senha incorreta)
 */

namespace SistemaDelivery.Control
{
    public class AutenticacaoService
    {
        //Chamou a interface IUsuarioRepository para buscar o usuário no banco de dados através do CPF e Senha (SOLID)
        private readonly IUsuarioRepository _usuarioRepository;

        public AutenticacaoService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public Usuario? Autenticar(string cpf, string senha)
        {
            //Melhorar a busca no banco dados, tirando os . e - do CPF
            string cpfFormatado = cpf.Replace(".", "").Replace("-", "").Trim();

            //Verificando se o CPF tem 11 dígitos ou s eo campo está vazio
            if (cpfFormatado.Length != 11 || string.IsNullOrEmpty(cpfFormatado))
            {
                throw new ArgumentException("CPF inválido");
            }

            //Se tudo deu certo, ele vai chamar o método BuscarUsuario da interface IUsuarioRepository para buscar o usuário no banco de dados através do CPF e Senha
            return _usuarioRepository.BuscarUsuario(cpfFormatado, senha);
        }
    }
}

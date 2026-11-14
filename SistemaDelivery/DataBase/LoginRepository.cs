using System;
using SistemaDelivery.Model;
using Microsoft.Data.SqlClient;

namespace SistemaDelivery.DataBase
{
    public class LoginRepository : IUsuarioRepository
    {
        //Comando SQL para buscar o usuário no banco de dados através do CPF
        private const string comandoSQL = @"SELECT * FROM Usuarios WHERE Cpf = @Cpf AND Senha = @Senha";

        // Variável para estabelecer a conexão com o banco de dados
        private readonly string _stringConexao;

        public LoginRepository(string stringConexao = ConexaoBanco.StringConexao)
        {
            _stringConexao = stringConexao;
        }

        //Chamando a interface IUsuarioRepository para buscar o usuário no banco de dados através do CPF
        public Usuario? BuscarUsuario(string cpf, string senha)
        {
            using var conexao = new SqlConnection(_stringConexao);
            using var comando = new SqlCommand(comandoSQL, conexao);

            //Usando Paramêtros (@Cpf e @Senha) para evitar SQL Injection
            comando.Parameters.AddWithValue("@Cpf", cpf);
            comando.Parameters.AddWithValue("@Senha", senha);

            conexao.Open();
            using var leitor = comando.ExecuteReader();

            if (!leitor.Read()) //Caso não encontre nenhum usuário, ele vai retornar null
            {
                return null;
            }
            {
                return new Usuario
                {
                    Id = leitor.GetInt32(leitor.GetOrdinal("Id")),
                    Nome = leitor.GetString(leitor.GetOrdinal("Nome")),
                    Cpf = leitor.GetString(leitor.GetOrdinal("Cpf")),
                    Senha = leitor.GetString(leitor.GetOrdinal("Senha"))
                };
            }
             
        }

    }
}

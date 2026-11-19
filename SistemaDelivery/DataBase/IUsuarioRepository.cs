using SistemaDelivery.Model;
using System;
using System.Collections.Generic;
using System.Text;

/*
 * SOLID -> O AutenticacaoService na pasta Control vai depender desta interface e não da classe LoginRepository inteira.
 * Com isso, o SOLID permite trocar a implementação sem mexer no serviço inteiro.
 */

namespace SistemaDelivery.DataBase
{
    public interface IUsuarioRepository
    {
        Usuario? BuscarUsuario(string cpf, string senha);
    }
}

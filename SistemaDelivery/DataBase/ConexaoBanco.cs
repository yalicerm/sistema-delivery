using System;
using System.Collections.Generic;
using System.Text;

/*
 * Classe responsável por armazenar a string de conexão com o banco de dados.
 * Uso do Clean Code para evitar que a string de conexão fique espalhada pelo código principal (LoginRepository.cs e DeliveryRepository.cs)
 */
namespace SistemaDelivery.DataBase
{
    public static class ConexaoBanco
    {
        public const string StringConexao = @"Server=(localdb)\mssqllocaldb;Database=DeliveryDB;Trusted_Connection=true;";
    }
}

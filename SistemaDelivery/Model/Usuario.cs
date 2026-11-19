using System;

namespace SistemaDelivery.Model
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty; //Pode ser um Atendende ou um Gerente que acessa o sistema

    }
}

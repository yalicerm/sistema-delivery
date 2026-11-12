using System;

public class Usuario
{
    public int Id { get; set; }
    public string Cpf { get; set; }
    public string Senha { get; set; }
    public string Nome { get; set; }
    public string Cargo { get; set; } //Pode ser um Atendende ou um Gerente que acessa o sistema

}
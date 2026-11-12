using System;

//Classe produto é abstrata pois vamos trabalhar com as classes Pratos e Bebida
public abstract class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Valor { get; set; }
}
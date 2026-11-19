using System.Globalization;
using System.Windows;
using System.Windows.Data;
using SistemaDelivery.Control;
using SistemaDelivery.Model;

namespace SistemaDelivery.View;

public static class Formatos
{
    private static readonly CultureInfo PtBr = new("pt-BR");

    public static string Moeda(decimal valor) => "R$" + valor.ToString("N2", PtBr);

    public static string Minutos(int minutos) => minutos > 0 ? $"{minutos}min" : "-";
}

public class MoedaConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is decimal valor ? Formatos.Moeda(valor) : "";

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

public class MinutosConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is int minutos ? Formatos.Minutos(minutos) : "-";

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

public class StatusTextoConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is string status ? $"Pedido {status.ToLowerInvariant()}" : "";

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

public static class Mensagens
{
    public static void Aviso(Window dono, string texto)
        => MessageBox.Show(dono, texto, "Yummy", MessageBoxButton.OK, MessageBoxImage.Warning);

    public static bool Confirmar(Window dono, string texto)
        => MessageBox.Show(dono, texto, "Yummy", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;

    public static void Tratar(Window dono, Exception ex)
    {
        if (ex is RegraDeNegocioException)
        {
            Aviso(dono, ex.Message);
            return;
        }

        MessageBox.Show(dono,
            "Ocorreu um erro inesperado.\n\n" + ex.GetBaseException().Message,
            "Yummy", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

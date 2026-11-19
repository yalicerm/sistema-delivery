using System.Windows;
using SistemaDelivery.Control;
using SistemaDelivery.Model;

namespace SistemaDelivery.View;

// View: só trata eventos de tela. A validação e a regra de login estão em AutenticacaoService.
public partial class LoginWindow : Window
{
    private readonly AutenticacaoService _autenticacao = new();

    public LoginWindow()
    {
        InitializeComponent();
        txtCpf.Focus();
    }

    private void Entrar_Click(object sender, RoutedEventArgs e)
    {
        txtErro.Visibility = Visibility.Collapsed;

        try
        {
            var senha = tglSenha.IsChecked == true ? txtSenhaVisivel.Text : pwdSenha.Password;
            var usuario = _autenticacao.Autenticar(txtCpf.Text, senha);

            var principal = new DeliveryWindow(usuario);
            Application.Current.MainWindow = principal;
            principal.Show();
            Close();
        }
        catch (RegraDeNegocioException ex)
        {
            MostrarErro(ex.Message);
        }
        catch (Exception ex)
        {
            MostrarErro("Não foi possível acessar o banco de dados: " + ex.GetBaseException().Message);
        }
    }

    // Alterna entre PasswordBox (oculta) e TextBox (visível), mantendo o texto digitado
    private void MostrarSenha_Click(object sender, RoutedEventArgs e)
    {
        var visivel = tglSenha.IsChecked == true;

        if (visivel)
            txtSenhaVisivel.Text = pwdSenha.Password;
        else
            pwdSenha.Password = txtSenhaVisivel.Text;

        txtSenhaVisivel.Visibility = visivel ? Visibility.Visible : Visibility.Collapsed;
        pwdSenha.Visibility = visivel ? Visibility.Collapsed : Visibility.Visible;
    }

    private void MostrarErro(string mensagem)
    {
        txtErro.Text = mensagem;
        txtErro.Visibility = Visibility.Visible;
    }
}

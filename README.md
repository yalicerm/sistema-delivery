<h1 align="center">🍔 Yummy – Sistema de Delivery de Balcão</h1>

<p align="center">
  Sistema desktop para o atendimento de balcão de uma hamburgueria: cardápio, pedidos (comandas) e pagamento com Pix, Cartão ou Dinheiro.<br>
  Desenvolvido em <b>C# / WPF / Entity Framework Core / SQL Server</b>, aplicando <b>MVC</b>, <b>SOLID</b>, <b>Clean Code</b> e o padrão <b>Strategy</b>.
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet" alt=".NET 10">
  <img src="https://img.shields.io/badge/C%23-WPF-239120?logo=csharp" alt="C# WPF">
  <img src="https://img.shields.io/badge/EF%20Core-10.0.10-6C3483" alt="EF Core">
  <img src="https://img.shields.io/badge/SQL%20Server-LocalDB-CC2927?logo=microsoftsqlserver" alt="SQL Server LocalDB">
  <img src="https://img.shields.io/badge/licen%C3%A7a-MIT-blue" alt="Licença MIT">
</p>

<p align="center">
  <img src="docs/telas/03-pedidos.png" alt="Tela de pedidos do Yummy" width="85%">
</p>

---

## 📌 Sobre o projeto

O **Yummy** foi desenvolvido como Atividade Avaliativa da disciplina de **Programação Orientada a Objetos II (POO II)** do curso de Engenharia de Software da **UVV**.

O sistema é usado pelo atendente ou gerente no balcão: ele faz login, mantém o cardápio, abre comandas para os clientes, acompanha o total e o tempo de espera de cada pedido e, ao concluir, registra a forma de pagamento. O Pix tem 5% de desconto.

## 🚀 Funcionalidades

- **Login** por CPF e senha, com consulta **SQL escrita à mão** (ADO.NET)
- **Cardápio:** cadastrar, editar, buscar e excluir lanches, acompanhamentos e bebidas
- **Pedidos:** abrir comanda, adicionar produtos, alterar quantidades e remover itens
- **Cálculos automáticos:** total do pedido e tempo de espera (maior tempo de preparo entre os itens)
- **Concluir pedido** escolhendo **Pix, Cartão ou Dinheiro** (padrão Strategy, com desconto no Pix)
- **Cancelar pedido** e **visualizar** pedidos entregues ou cancelados, com o pagamento realizado
- **Busca** de pedidos pelo nome da comanda (SQL manual) e de produtos por nome ou categoria
- **Banco criado automaticamente** ao abrir o sistema (Code First + Migrations)

## 🖼️ Telas

<table>
  <tr>
    <td align="center"><img src="docs/telas/01-login.jpg" alt="Login" width="100%"><br><sub>Login</sub></td>
    <td align="center"><img src="docs/telas/02-login-erro.jpg" alt="Login com erro" width="100%"><br><sub>Mensagem de erro no login</sub></td>
  </tr>
  <tr>
    <td align="center"><img src="docs/telas/03-pedidos.png" alt="Pedidos" width="100%"><br><sub>Pedidos com status</sub></td>
    <td align="center"><img src="docs/telas/04-cardapio.png" alt="Cardápio" width="100%"><br><sub>Cardápio</sub></td>
  </tr>
</table>

<table>
  <tr>
    <td align="center"><img src="docs/telas/05-produto-modal.png" alt="Cadastro de produto" width="100%"><br><sub>Cadastro de produto</sub></td>
    <td align="center"><img src="docs/telas/06-pedido-pagamento.png" alt="Pedido com pagamento" width="100%"><br><sub>Pedido com Pix: desconto em "Valor a pagar"</sub></td>
    <td align="center"><img src="docs/telas/07-pedido-entregue.png" alt="Pedido entregue" width="100%"><br><sub>Pedido entregue com o pagamento</sub></td>
  </tr>
</table>

## 🏗️ Arquitetura (MVC)

```
LoginWindow / PedidosView / CardapioView / Modais         ← View (WPF)
                 │
                 ▼
AutenticacaoService / ProdutoService / PedidoService      ← Control (regras de negócio)
PagamentoService + Strategy (IFormaPagamento)
                 │
                 ▼
DeliveryDBContext (EF Core)  ·  LoginRepository (SQL)     ← DataBase
                 │
                 ▼
         SQL Server LocalDB  →  banco DeliveryDB
```

| Camada | Pasta | Responsabilidade |
|---|---|---|
| **Model** | `Model/` | Entidades: `Produto` (abstrata), `Lanche`, `Acompanhamento`, `Bebida`, `Pedido`, `ItemPedido`, `Pagamento`, `Usuario` |
| **View** | `View/` | Telas WPF. Só exibem dados e capturam eventos, sem regra de negócio |
| **Control** | `Control/` | Regras e validações; lançam `RegraDeNegocioException` com a mensagem para a tela |
| **DataBase** | `DataBase/` | `DeliveryDBContext` (Fluent API + seed), `ConexaoBanco` e `LoginRepository` (SQL manual) |

## 🗄️ Modelo de dados

```mermaid
classDiagram
direction LR
class Produto {
  <<abstract>>
  +int IdProduto
  +string Nome
  +decimal Valor
  +string Categoria
  +int TempoPreparoMinutos
}
class Lanche {
  +int tempoDePreparo
}
class Acompanhamento {
  +int tempoPreparado
}
class Bebida {
  +int capacidadeML
  +bool EhAlcoolica
}
class Pedido {
  +int IdPedido
  +string NomeComanda
  +DateTime DataHora
  +string Status
  +decimal ValorTotal
}
class ItemPedido {
  +int IdItem
  +int Quantidade
  +decimal PrecoUnitario
}
class Pagamento {
  +int IdPagamento
  +string FormaPagamento
  +decimal ValorPago
  +DateTime DataPagamento
}
class IFormaPagamento {
  <<interface>>
  +string Nome
  +CalcularValorFinal(decimal) decimal
}
Produto <|-- Lanche
Produto <|-- Acompanhamento
Produto <|-- Bebida
Pedido "1" *-- "1..*" ItemPedido
ItemPedido "0..*" o-- "1" Produto
Pedido "1" *-- "0..1" Pagamento
IFormaPagamento <|.. PagamentoPix
IFormaPagamento <|.. PagamentoCartao
IFormaPagamento <|.. PagamentoDinheiro
```

| Relacionamento | Onde | Como |
|---|---|---|
| **Herança** | `Produto` → `Lanche`, `Acompanhamento`, `Bebida` | Classe abstrata + TPH (coluna `Discriminator`) |
| **1x1** | `Pedido` ↔ `Pagamento` | FK `PedidoId` com índice único |
| **1xN** | `Pedido` → `ItemPedido`, `Produto` → `ItemPedido` | FKs `PedidoId` e `ProdutoId` |
| **NxN** | `Pedido` ↔ `Produto` | Classe associativa `ItemPedido` (quantidade e preço) |
| **Composição** | `Pedido` ◆ `ItemPedido`, `Pedido` ◆ `Pagamento` | `OnDelete(Cascade)` |
| **Agregação** | `ItemPedido` ◇ `Produto` | `OnDelete(Restrict)` |

**Migrations (Code First):** `DeliveryBalcaoInicial` → `ConfiguraUsuarioESeed` → `AdicionaPagamento` → `AdicionaComandaPedido`

## 🧩 SOLID e Clean Code

| Princípio | Onde |
|---|---|
| **S** – Responsabilidade única | `LoginRepository` só acessa o banco; `AutenticacaoService` só valida o login; `ConexaoBanco` só guarda a conexão; cada forma de pagamento tem uma única regra |
| **O** – Aberto/Fechado | Nova forma de pagamento = nova classe que implementa `IFormaPagamento`, sem alterar os serviços |
| **L** – Substituição de Liskov | `Lanche`, `Acompanhamento` e `Bebida` funcionam onde se espera um `Produto` (lista, `ItemPedido`, cálculo do tempo) |
| **I** – Segregação de interfaces | Interfaces pequenas: `IUsuarioRepository` (1 método) e `IFormaPagamento` (2 membros) |
| **D** – Inversão de dependência | `AutenticacaoService` recebe `IUsuarioRepository` pelo construtor; `PagamentoService` depende de `IFormaPagamento` |

**Clean Code:**
- Nomes que explicam a intenção (`CalcularTempoEspera`, `BuscarEmAndamento`)
- Métodos curtos (`LerPreco`, `CriarPorCategoria`)
- Constantes em vez de textos e números soltos (`StatusPedido`, `CategoriaProduto`, `PercentualDesconto`)
- Cláusulas de guarda (o método valida e sai logo no início)
- Exceção de domínio única (`RegraDeNegocioException`)
- Parâmetros SQL contra SQL Injection
- `using` para liberar conexões
- Camadas separadas por namespace

## 🛠️ Tecnologias

| Tecnologia | Versão |
|---|---|
| C# / .NET (WPF) | .NET 10 (`net10.0-windows`) |
| Entity Framework Core (+ SqlServer, Tools, Design) | 10.0.10 |
| Microsoft.Data.SqlClient (SQL manual) | 7.1.2 |
| SQL Server LocalDB | MSSQLLocalDB |

## ▶️ Como executar

**Pré-requisitos:** Visual Studio com a carga de trabalho **Desenvolvimento para desktop com .NET** e o componente **SQL Server Express LocalDB**. Para conferir o LocalDB:

```bash
sqllocaldb info
```

1. Clone o repositório:
   ```bash
   git clone https://github.com/yalicerm/sistema-delivery.git
   ```
2. Abra `SistemaDelivery/SistemaDelivery.slnx` no Visual Studio.
3. Pressione **F5**. Na primeira execução o sistema cria o banco `DeliveryDB` e aplica as migrations sozinho.
4. Entre com um dos usuários de teste:

| Cargo | CPF | Senha |
|---|---|---|
| Gerente | `111.111.111-11` | `gerente123` |
| Atendente | `222.222.222-22` | `atendente123` |

> As senhas ficam em texto puro por simplicidade acadêmica. Em um sistema real, elas seriam guardadas com hash.

## 📁 Estrutura de pastas

```
SistemaDelivery/
├── App.xaml(.cs)        Início: cultura pt-BR, cria o banco e abre o login
├── Model/               Entidades + constantes (StatusPedido, CategoriaProduto)
├── DataBase/            DeliveryDBContext, ConexaoBanco, IUsuarioRepository, LoginRepository
├── Control/             Serviços, RegraDeNegocioException e FormasPagamento/ (Strategy)
├── View/                Telas WPF, Converters, Styles e Assets
└── Migrations/          Gerado pelo Entity Framework (Code First)
```

## 👩‍💻 Equipe

| Integrante | Responsabilidade principal |
|---|---|
| Alice Resende | Banco de dados, entidades, migrations, login com SQL manual, pagamento e integração |
| Luisa Mello | Telas WPF, identidade visual e serviços ligados às telas |
| Nicole Bassani | |

## 🤖 Uso de Inteligência Artificial (Nível IA-2)

A IA foi usada como ferramenta de apoio sob supervisão humana contínua. Ela ajudou no diagnóstico de erros (por exemplo, os erros de compilação que impediam o `Add-Migration`), na revisão do código e na geração inicial de alguns trechos. Todo o código foi revisado, testado e adaptado pela equipe. As decisões de projeto e o domínio da solução são das autoras. Os detalhes, com os prompts e a comparação de um trecho com e sem IA, estão na documentação oficial do projeto.

## 📄 Licença

Distribuído sob a licença MIT. Veja [LICENSE](LICENSE).

## 📌 Sobre o Projeto
Este projeto foi desenvolvido como Atividade Avaliativa para a disciplina de Programação Orientada a Objetos II (POO II) do curso de Engenharia da Software da UVV. 

O objetivo principal é conceber um Sistema de Delivery do zero, aplicando conceitos avançados de engenharia de software, incluindo **Design Patterns (Strategy)**, **Clean Code**, **Entity Frameworks** e os princípios **SOLID**. O sistema cadastra resturantes e produtos além de processar pedidos desde a escolha dos produtos pelo cliente e finalização do pagamento.

## 🏗️ Arquitetura e Padrões
O projeto segue a estruturação em camadas **MVC (Model-View-Controller/Service)** para garantir uma rigorosa separação de responsabilidades:

- **Models:** Entidades de domínio (Produto, Pedido, Cliente) mapeadas para a base de dados via Code First.
- **Views:** Interfaces gráficas usando WPF construídas para as perspetivas do Cliente e do Administrador.
- **Services:** Regras de negócio e operações CRUD isoladas, garantindo o *Single Responsibility Principle* (SRP).
- **Interfaces (SOLID):** Implementação do padrão *Strategy* no processamento de pagamentos (Cartão e Pix), respeitando o *Open/Closed Principle* (OCP).

## 🚀 Funcionalidades
- **Painel do Administrador:** Gestão de cardápio (CRUD de pratos e bebidas).
- **Painel do Cliente:** Navegação no menu, adição de itens ao carrinho e finalização de pedido.
- **Sistema de Pagamentos:** Processamento polimórfico de pagamentos via Pix e Cartão de Crédito.

## 🤖 Uso de Inteligência Artificial (Declaração)
> **Nível IA-2 (Automação Guiada)**

No desenvolvimento deste projeto, a Inteligência Artificial foi utilizada como ferramenta de assistência sob supervisão humana contínua. 
- **Planeamento:** Utilização de IA estruturar o diagrama UML e validar a arquitetura relacional.
- **Implementação:** Assistência na geração de protótipos de código (ex: *boilerplates* do Entity Framework e configurações iniciais WPF) com posterior revisão crítica, refatoração manual e adaptação às regras de negócio da equipe.
- **Controlo de Qualidade:** O domínio intelectual sobre a solução final manteve-se exclusivamente com os autores.

## 🛠️ Como Executar o Projeto

**Pré-requisitos:**
- Visual Studio (com workload de desenvolvimento Desktop .NET).
- Servidor MySQL local ou em cloud configurado.


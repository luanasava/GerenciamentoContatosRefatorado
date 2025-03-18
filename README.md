# Microsserviços com RabbitMQ 

Este repositório contém a refatoração de um aplicativo monolítico em uma arquitetura de microsserviços, com comunicação assíncrona utilizando o RabbitMQ para gerenciar os eventos entre os microsserviços. O objetivo é separar as funcionalidades por contexto, como cadastro, atualização, consulta e exclusão de contatos, garantindo uma comunicação eficiente e escalável entre os microsserviços.

## Objetivos do Projeto

- **Arquitetura de Microsserviços:** O aplicativo foi refatorado em microsserviços separados, cada um responsável por um contexto específico. Por exemplo, um microsserviço de cadastro que envia dados para outro microsserviço, responsável por persistir os dados no banco de dados.
- **Comunicação Assíncrona com RabbitMQ:** Foi implementada a comunicação assíncrona entre os microsserviços, utilizando o RabbitMQ para gerenciar as filas de mensagens. A comunicação entre os microsserviços ocorre da seguinte forma:
    1. O microsserviço produtor recebe os dados (como o cadastro de um novo contato).
    2. O dado é enviado para uma fila no RabbitMQ.
    3. O microsserviço consumidor recebe a mensagem da fila e persiste os dados no banco de dados.

## Fluxo de Trabalho

1. **Microsserviço Produtor:** Recebe os dados, valida e os envia para a fila do RabbitMQ.
2. **Fila no RabbitMQ:** Gerencia as mensagens (dados) que serão consumidas pelos microsserviços consumidores.
3. **Microsserviço Consumidor:** Recebe as mensagens da fila e persiste as informações no banco de dados, realizando a ação necessária (criação, atualização ou exclusão).

## Requisitos Técnicos

### 1. **Divisão em Microsserviços**

O aplicativo monolítico foi dividido em microsserviços menores, cada um responsável por uma parte específica do processo, tais como:

- Cadastro de contatos.
- Consulta de contatos.
- Atualização de contatos.
- Exclusão de contatos.

### 2. **Padrões de Comunicação**

- **Circuit Breaker:** Foi implementado o padrão Circuit Breaker onde necessário, para garantir a resiliência e disponibilidade dos microsserviços.
- **Mensageria com RabbitMQ:** O RabbitMQ foi configurado para gerenciar a fila de mensagens entre os microsserviços. Foram implementados os produtores (producers) e consumidores (consumers) para os eventos de criação, atualização e exclusão de contatos.


### 3. **Pipeline de CI/CD**

- Foi configurada uma pipeline de integração contínua (CI) para garantir que os testes unitários e de integração sejam executados corretamente em cada mudança de código.
- A pipeline também garante que a aplicação seja testada, monitorada e documentada antes de ser promovida para produção.

- ### 4. **Tecnologias Utilizadas**

- **Linguagem:** C# — Plataforma robusta e moderna para desenvolvimento backend.
- **Framework:** .NET Core — Framework multiplataforma para alto desempenho e escalabilidade.
- **Banco de Dados:** SQL Server — Sistema relacional para armazenar dados persistentes.
- **ORM:** Entity Framework Core — Facilita a interação com o banco de dados, simplificando operações de leitura e escrita.
- **Testes:** xUnit — Framework para testes unitários e de integração, garantindo a qualidade do código.
- **RabbitMQ:** Mensageria para comunicação assíncrona entre microsserviços.
- **Prometheus:** Monitoramento da aplicação.
- **Grafana:** Visualização de métricas coletadas pelo Prometheus.

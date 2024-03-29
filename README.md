# VN User Behavior Test
Microsserviço para controle de Comportamento do Usuário visando atender cenário de teste prático VN.

### Before you start
Este é um projeto de demonstração de um microsserviço desenhado seguindo princípios DDD, SOLID e RESTFUL.

Este projeto foi pensado visando atender o cenário proposto no teste de admissão da VN. Para tal fim, expõe 4 (quatro) endpoints (detalhados na seção **Docs** abaixo), sendo:


 | Verbo | Endpoint       | Descrição                         |
 | ----- | -------------- | --------------------------------- |
 | GET   | /api/v1.0/Behavior/ip/{ip} | Utilize para consultar um comportamento pelo IP (retorna uma lista)  |
 | GET   | /api/v1.0/Behavior/page/{pageName}      | Utilize para consultar um comportamento pelo nome de uma página (retorna uma lista) |
 | GET   | /api/v1.0/Behavior/ip/{ip}/page/{pageName} | Utilize para consultar um comportamento exclusivo por sua chave composta (retorna um único objeto) |
 | POST  | /api/v1.0/Behavior | Utilize para criar um comportamento |

### Optional Requisite

* [Docker](https://www.docker.com/community-edition)
* [Docker Compose](https://docs.docker.com/compose/install)

### Up and running
O jeito mais prático de iniciar e testar o projeto é utilizando o docker.

O projeto está configurado com um conteiner contendo ambas bases de dados (MSSQL e Couchbase) bem como a imagem para o RabbitMQ e possui mecanismo *Makefile*, que facilia a execução dos comandos mais comuns. Se já possuir Docker e Docker-compose instalado, recomendo que utilize esta opção.

```console
$ make build
$ make up
$ make migrate
```

Caso não deseje utilizar o Docker, poderá também utilizar o Kestrel para iniciar e executar o projeto. Note que, neste cenário, ambas bases de dados deverão estar instaladas e acessíveis e seus respectivos hosts deverão ser definidos no arquivo de configuração da API (appsettings.Development.json) antes de prosseguir com os testes.

Note também que, executando o projeto a partir do Visual Studio, deve-se selecionar a configuração *Multi*.

### Docs
A documentação da API (Swagger) poderá ser acessada em:

[http://localhost:8081/docs](http://localhost:8081/docs)

### The Skeleton
O Projeto está estruturado seguindo a orientação DDD, como pode ser visto abaixo:

* VN.Example.Platform
    * Neste projeto estão contidas as camadas de Domínio (Domain) e Aplicação (Application) onde é possível encontrar as regras de negócio (Domain) e o orquestrador dos domínios (Application).
* VN.Example.Infrastructure
    * Este projeto engloba todos os recursos de infraestrutura, tal qual providers (RabbitMQ), Inversão de Controle e implementações de banco de dados (MSSQL e Couchbase); os projetos estão divididos nas Solution Folders Databases e Providers, dispostos a seguir:
        * VN.Example.Infrastructure
            * Guarda toda a implementação de IoC, utilizando-se do container de dependência nativo do .NET Core para tal fim.
        * Databases/VN.Example.Infrastructure.Database.MSSQL
            * Guarda a implementação base da interface de repositório do domínio para o SQL Server utilizando-se de Entity Framework Core para tal fim.
        * Databases/VN.Example.Infrastructure.Database.Couchbase
            * Guarda a implementação base da interface de repositório do domínio para o Couchbase (NoSQL)
        * Providers/VN.Example.Infrastructure.Provider.MessageBus
            * Guarda a implementação base do serviço de mensageria (RabbitMQ)
* Host/VN.Example.Host.Web
    * Implementação da API MVC para telas (front) e api de apoio (back)
* Host/VN.Example.Host.BehaviorCreated
    * Implementação de console application contendo o subscriber da fila de mensageria (RabbitMQ) para execução assíncrona (lê da fila e salva nos bancos)
* Solution Items
    * Arquivos diversos de apoio ao projeto (tal qual docker-compose file, makefile, etc.)

### Testing before the test
A solução contém também um projeto de Testes do domínio, a título de exemplo, criado em xUnit e fazendo uso da biblioteca Moq.

Caso esteja usando Docker e Docker-compose, pode usar o comando Make abaixo para executar os testes diretamente de um terminal:

```console
$ make test
```
### Attention, please
* Visando utilizar apenas o container de dependência nativo do .NET Core, foi adotado um método "elegante" para injetar mais de uma classe concreta referente à mesma interface de repositório (ver *IoC.ExampleModule*). É favor notar que este método visa ser o mais simples possível (utiliza de strings para determinar qual injeção utilizar) para a situação e deve ser melhorado em uma implementação real (ou deve-se utilizar container de terceiros com suporte à esta funcionalidade).
* É favor ter em mente que regras de negócio, regras de segurança ou políticas de resiliência não foram levados em consideração durante o desenvolvimento desta solução.
* Foi incluído um recurso de health check para o RabbitMQ (a título de exemplo) que pode ser acessado no link: [http://localhost:8081/healthchecks-ui#/healthchecks](http://localhost:8081/healthchecks-ui#/healthchecks)
* Foram utilizados alguns princípios e padrões (leia-se "building blocks") trazidos pelo DDD (tais quais Repository, Entities, Command e Event) enquanto outros não se fizeram necessários (como Domain Service, Static Factory, Policy/Strategy etc.)

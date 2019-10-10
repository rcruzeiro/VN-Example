# VN User Behavior Manager
Microsserviço para Gerenciamento de Comportamento do Usuário visando atender cenário de teste prático VN.

### Before you start
Este é um projeto de demonstração de um microsserviço desenhado seguindo princípios DDD, SOLID e RESTFUL.

Este projeto foi pensado visando atender o cenário proposto no teste de admissão da VN. Para tal fim, expõe 4 (quatro) endpoints (detalhados na seção **Docs** abaixo), sendo:


 | Verbo | Endpoint       | Descrição                         |
 | ----- | -------------- | --------------------------------- |
 | GET   | /api/v1.0/Behavior/ip/{ip} | Utilize para consultar um comportamento pelo IP (retorna uma lista)  |
 | GET   | /api/v1.0/Behavior/page/{pageName}      | Utilize para consultar um comportamento pelo nome de uma página (retorna uma lista) |
 | GET   | /api/v1.0/Behavior/{ip}/{pageName}/{userAgent} | Utilize para consultar um comportamento exclusivo por sua chave composta (retorna um único objeto) |
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

### Docs
A documentação da API (Swagger) poderá ser acessada em:

[http://localhost:8081/docs](http://localhost:8081/docs)

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

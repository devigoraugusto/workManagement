# Work Management System

---
Projeto de gerenciamento de tarefas e projetos com um CRUD de tarefas, e integra��o com banco de dados.

## Tecnologias Utilizadas
- ASP.NET Core
- Entity Framework Core
- SQL Server
- Swagger
- Dependency Injection
- Conteineriza��o com Docker
- Testes Unit�rios
- Design Patterns (Repository, Unit of Work)

## Funcionalidades
- Cadastro de Tarefas
- Listagem de Tarefas
- Atualiza��o de Tarefas
- Exclus�o de Tarefas
- Filtros de Tarefas

## Instala��o e Configura��o
1. Clone o reposit�rio:
   ```bash
   git clone
   ```
2. Navegue at� o diret�rio do projeto:
   ```bash
   cd workManagement
   ```
3. Restaure as depend�ncias:
   ```bash
   dotnet restore
   ```
4. Configure a string de conex�o no arquivo `appsettings.json`:
   ```json
   "ConnectionStrings": {
	   "DefaultConnection": "Server=localhost;Database=WorkManagement;User Id=sa;Password=your_password;"
   }
   ```
5. Execute as migra��es do Entity Framework:
   ```bash
   dotnet ef database update
   ```
6. Inicie o projeto:
   ```bash
   dotnet run
   ```
7. Acesse a API via Swagger em `http://localhost:5000/swagger`.

## Testes
Para executar os testes unit�rios, utilize o seguinte comando:
	```bash
	dotnet test
	```

## Rodando com Docker
Para rodar o projeto com Docker, siga os passos abaixo:
1. Certifique-se de que o Docker est� instalado e em execu��o.
2. Navegue at� o diret�rio do projeto.
3. Construa a imagem Docker:
   ```bash
   docker build -t workmanagement .
   ```
4. Execute o cont�iner Docker:
   ```bash
   docker run -d -p 5000:80 --name workmanagement_container workmanagement
   ```
5. Acesse a API via Swagger em `http://localhost:5000/swagger`.


# Work Management System

---
Projeto de gerenciamento de tarefas e projetos com um CRUD de tarefas, e integração com banco de dados.

## Tecnologias Utilizadas
- ASP.NET Core
- Entity Framework Core
- SQL Server
- Swagger
- Dependency Injection
- Conteinerização com Docker
- Testes Unitários
- Design Patterns (Repository, Unit of Work)

## Funcionalidades
- Cadastro de Tarefas
- Listagem de Tarefas
- Atualização de Tarefas
- Exclusão de Tarefas
- Filtros de Tarefas

## Instalação e Configuração
1. Clone o repositório:
   ```bash
   git clone
   ```
2. Navegue até o diretório do projeto:
   ```bash
   cd workManagement
   ```
3. Restaure as dependências:
   ```bash
   dotnet restore
   ```
4. Configure a string de conexão no arquivo `appsettings.json`:
   ```json
   "ConnectionStrings": {
	   "DefaultConnection": "Server=localhost;Database=WorkManagement;User Id=sa;Password=your_password;"
   }
   ```
5. Execute as migrações do Entity Framework:
   ```bash
   dotnet ef database update
   ```
6. Inicie o projeto:
   ```bash
   dotnet run
   ```
7. Acesse a API via Swagger em `http://localhost:5000/swagger`.

## Testes
Para executar os testes unitários, utilize o seguinte comando:
	```bash
	dotnet test
	```

## Rodando com Docker
Para rodar o projeto com Docker, siga os passos abaixo:
1. Certifique-se de que o Docker está instalado e em execução.
2. Navegue até o diretório do projeto.
3. Construa a imagem Docker:
   ```bash
   docker build -t workmanagement .
   ```
4. Execute o contêiner Docker:
   ```bash
   docker run -d -p 5000:80 --name workmanagement_container workmanagement
   ```
5. Acesse a API via Swagger em `http://localhost:5000/swagger`.


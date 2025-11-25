# README – Projeto Gerenciador de Tarefas (GT)

Este arquivo contém todos os comandos e estrutura do projeto.

---

# Sobre o Projeto

Gerenciador de Tarefas desenvolvido em C# (.NET) com PostgreSQL.

---

# Estrutura do Projeto

```
/GT.Api          → Controllers, Program.cs, Swagger
/GT.Application  → Services, DTOs, UseCases
/GT.Domain       → Entidades e Interfaces
/GT.Infra        → DbContext, Repositories, Migrations
```

---

# Comandos Usados Até Agora

## Criar solução

```bash
dotnet new sln -n GT
```

## Criar projetos

```bash
dotnet new webapi -n GT.Api
dotnet new classlib -n GT.Application
dotnet new classlib -n GT.Domain
dotnet new classlib -n GT.Infra
```

## Adicionar projetos à solução

```bash
dotnet sln add GT.Api/GT.Api.csproj
dotnet sln add GT.Application/GT.Application.csproj
dotnet sln add GT.Domain/GT.Domain.csproj
dotnet sln add GT.Infra/GT.Infra.csproj
```

## Referenciar camadas

```bash
dotnet add GT.Api reference GT.Application
dotnet add GT.Application reference GT.Domain
dotnet add GT.Api reference GT.Infra
dotnet add GT.Infra reference GT.Domain
```

## Instalar Entity Framework Core + PostgreSQL

### No GT.Infra

```bash
dotnet add GT.Infra package Microsoft.EntityFrameworkCore
dotnet add GT.Infra package Microsoft.EntityFrameworkCore.Design
dotnet add GT.Infra package Npgsql.EntityFrameworkCore.PostgreSQL
```

### Na API

```bash
dotnet add GT.Api package Microsoft.EntityFrameworkCore
dotnet add GT.Api package Npgsql.EntityFrameworkCore.PostgreSQL
```

## Migrations

Criar:

```bash
dotnet ef migrations add InitialMigration --project GT.Infra --startup-project GT.Api
```

Atualizar banco:

```bash
dotnet ef database update --project GT.Infra --startup-project GT.Api
```

---

# Como Rodar o Projeto

1. Restaurar dependências:

```bash
dotnet restore
```

2. Rodar API:

```bash
dotnet run --project GT.Api
```

3. Acessar Swagger:
   [http://localhost:5000/swagger](http://localhost:5000/swagger)

---

# Configuração do Banco (PostgreSQL)

Criar banco:

```sql
CREATE DATABASE GT;
```

Connection string no `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=GT;Username=postgres;Password=SUASENHA"
}
```

Aplicar migrations:

```bash
dotnet ef database update --project GT.Infra --startup-project GT.Api
```

---

# Entidades

## Usuario

```csharp
public class Usuario {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string SenhaHash { get; set; }
    public List<Tarefa> Tarefas { get; set; }
}
```

## Tarefa

```csharp
public class Tarefa {
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descricao { get; set; }
    public bool Concluida { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
}
```

---

# Endpoints

## Auth

* POST /auth/login

## Tarefas

* POST /tarefas
* GET /tarefas
* PUT /tarefas/{id}/concluir
* DELETE /tarefas/{id}

---

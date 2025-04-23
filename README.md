# Ambev Developer Evaluation - Backend API

## 📦 Sobre o Projeto

Esta aplicação foi desenvolvida como parte do processo de avaliação técnica da Ambev. Ela simula uma API RESTful para gerenciamento de usuários, produtos, carrinhos e vendas, implementada em .NET com arquitetura moderna, testes unitários completos e suporte a Docker.

---

## 🚀 Como Executar o Projeto

### Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker + Docker Compose](https://docs.docker.com/get-docker/)
- [PostgreSQL CLI ou cliente DB externo opcional]

### 1. Clonar o repositório

```bash
git clone https://github.com/SeuUsuario/Ambev.DeveloperEvaluation.git
cd Ambev.DeveloperEvaluation
```

### 2. Executar via Docker

```bash
docker-compose up --build
```

A aplicação estará disponível em: [https://localhost:8081/swagger](https://localhost:8081/swagger)

### 3. Executar Localmente (sem Docker)

1. Configure a connection string no `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=developer_evaluation;Username=developer;Password=ev@luAt10n"
}
```

2. Aplique as migrations:

```bash
dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM
```

3. Execute o projeto:

```bash
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi
```

---

## 🧪 Executar os Testes

```bash
dotnet test
```

- Todos os testes estão organizados por área (Users, Products, Carts, Sales).
- Total de testes implementados: **134**
- Cobertura de testes: 100% dos cenários documentados.

---

## 🔍 Swagger

A documentação da API está disponível em tempo de execução via Swagger UI:

```
https://localhost:8081/swagger/index.html
```

---

## 📦 Exemplos de Payload

### Criar Produto (POST /api/Products)
```json
{
  "title": "Breja Ambev",
  "description": "Cerveja Leve para Churrasco",
  "price": 9.99,
  "category": "Birita",
  "image": "https://example.com/mouse.png",
  "rate": 4.9,
  "count": 120
}
```

### Criar Venda (POST /api/Sales)
```json
{
  "customerId": "eea3ebf9-2ff6-4670-81ae-fcc22477ef96",
  "branchId": "1c4d6cfa-1fd6-40cf-b20d-4b4600d84653",
  "items": [
    {
      "productId": 2,
      "quantity": 12
    }
  ]
}
```

---

## 🧱 Tecnologias

- .NET 8 + C# 12
- PostgreSQL
- Redis (Docker)
- MongoDB (Docker)
- xUnit + Moq + FluentAssertions
- MediatR + CQRS
- FluentValidation
- AutoMapper
- Serilog (pronto para logging estruturado)
- Swagger/OpenAPI

---

## 📂 Organização

```
├── Application
│   ├── Users
│   ├── Products
│   ├── Carts
│   └── Sales
├── Domain
│   ├── Entities
│   ├── Interfaces
│   └── Enums
├── ORM
├── WebApi
├── Tests
└── docker-compose.yml
```

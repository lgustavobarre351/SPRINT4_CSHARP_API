# 🚀 Investimentos API

## 👥 **INTEGRANTES**
- **André Lambert** - RM: 99148
- **Felipe Cortez** - RM: 99750
- **Julia Lins** - RM: 98690
- **Luis Barreto** - RM: 99210
- **Victor Aranda** - RM: 99667

---

## 🎯 **SOBRE O PROJETO**

API RESTful para gestão de investimentos desenvolvida com **ASP.NET Core 9.0** e **PostgreSQL**.

### **📈 Funcionalidades:**
- **Gestão de usuários** com validação de CPF
- **CRUD de investimentos** (ações, fundos, etc.)
- **Validação B3** para códigos de ações brasileiras
- **Consulta de CEP** para validação de endereços
- **Relatórios** com LINQ

### **🛠️ Tecnologias:**
- ASP.NET Core 9.0
- Entity Framework Core
- PostgreSQL + Supabase
- Swagger/OpenAPI

## 🚀 **COMO EXECUTAR**

### **Pré-requisitos:**
- .NET 9.0 SDK instalado
- Git para clonar o repositório

### **Passos:**
```bash
# Clonar e executar
git clone https://github.com/lgustavobarre351/SPRINT4_CSHARP_API.git
cd SPRINT4_CSHARP_API/Investimentos
dotnet restore
dotnet run
```

**Acesse:** http://localhost:5171/swagger

## 📋 **ENDPOINTS PRINCIPAIS**

### **👥 Usuários** (`/api/Usuarios`)
- `GET /api/Usuarios` - Listar usuários
- `POST /api/Usuarios` - Criar usuário
- `GET /api/Usuarios/{cpf}` - Buscar por CPF

### **💰 Investimentos** (`/api/Investimentos`)
- `GET /api/Investimentos` - Listar investimentos
- `POST /api/Investimentos` - Criar investimento
- `GET /api/Investimentos/usuario/{cpf}` - Por usuário
- `GET /api/Investimentos/tipo/{tipo}` - Por tipo

### **🌐 APIs Externas** (`/api/ApisExternas`)
- `GET /api/ApisExternas/codigos-b3` - Códigos B3
- `GET /api/ApisExternas/validar-codigo/{symbol}` - Validar ação
- `GET /api/ApisExternas/cotacao/{symbol}` - Cotação

## 🗄️ **BANCO DE DADOS**

PostgreSQL hospedado no Supabase:
- **user_profiles** - Dados dos usuários
- **investimentos** - Operações de investimento

## 🎯 **EXEMPLOS DE USO**

### Criar usuário:
```json
POST /api/Usuarios
{
  "cpf": "12345678901",
  "nome": "João Silva",
  "email": "joao@email.com"
}
```

### Criar investimento:
```json
POST /api/Investimentos
{
  "userCpf": "12345678901",
  "tipo": "Ação",
  "codigo": "PETR4",
  "valor": 1000.00,
  "operacao": "compra"
}
```

## 📜 **LICENÇA**

MIT License - Projeto desenvolvido para Challenge FIAP 2024.



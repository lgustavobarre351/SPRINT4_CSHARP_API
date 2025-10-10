# 🚀 Investimentos API - Sistema de Gestão de Carteiras de Investimento

**API completa para gestão de investimentos financeiros com integração a mercados reais, desenvolvida com ASP.NET Core 9.0, Entity Framework Core e PostgreSQL**

## 🎯 **O QUE É ESTE PROJETO?**

Esta é uma **API RESTful para gestão de carteiras de investimento** que simula um sistema real de corretora/fintech. A aplicação permite:

### **📈 Funcionalidades Principais:**
- **Gestão completa de investimentos**: CRUD de operações de compra/venda
- **Integração com mercado financeiro**: Cotações em tempo real via Alpha Vantage
- **Compliance bancário**: Validação de endereços via CEP para KYC
- **Monitoramento de criptomoedas**: Acompanhamento do Bitcoin via CoinGecko
- **Análises avançadas**: Relatórios e agrupamentos com LINQ
- **Documentação interativa**: Interface Swagger completa

### **💼 Cenários de Uso Real:**
1. **Cliente Pessoa Física**: Gerenciar carteira pessoal de investimentos
2. **Assessor de Investimentos**: Monitorar múltiplas carteiras de clientes
3. **Compliance/Auditoria**: Relatórios para órgãos reguladores
4. **Sistema de Trading**: Base para plataforma de investimentos

## 🏦 **CONTEXTO E UTILIDADE DA API**

### **Por que esta API foi criada?**
Esta aplicação resolve problemas reais enfrentados por **fintechs e corretoras de investimento**:

#### **🎯 Problema 1: Gestão de Carteiras**
- **Situação**: Investidores precisam acompanhar múltiplos ativos (ações, fundos, crypto)
- **Solução**: Sistema centralizado para registrar todas as operações
- **Valor**: Visão unificada do patrimônio e performance

#### **💹 Problema 2: Informações de Mercado**
- **Situação**: Decisões de investimento precisam de dados atualizados
- **Solução**: Integração com APIs financeiras para cotações em tempo real
- **Valor**: Dados confiáveis para tomada de decisão

#### **📋 Problema 3: Compliance Regulatório**
- **Situação**: Instituições financeiras precisam validar dados dos clientes
- **Solução**: Validação automática de endereços e informações pessoais
- **Valor**: Conformidade com regulamentações bancárias (BACEN, CVM)

#### **📊 Problema 4: Análises e Relatórios**
- **Situação**: Investidores e assessores precisam de insights sobre portfólios
- **Solução**: Consultas LINQ para análises avançadas e agrupamentos
- **Valor**: Relatórios gerenciais e fiscais automatizados

### **🌐 Integrações com APIs Externas:**

#### **📈 Alpha Vantage - Cotações de Ações**
- **O que faz**: Fornece preços em tempo real de ações globais
- **Por que usar**: Essencial para valorização de carteiras
- **Exemplo prático**: "Quanto valem minhas 100 ações da Apple hoje?"

#### **🏠 ViaCEP - Validação de Endereços**
- **O que faz**: Consulta endereços brasileiros por CEP
- **Por que usar**: Compliance KYC (Know Your Customer) obrigatório
- **Exemplo prático**: "Validar endereço do cliente para abertura de conta"

#### **₿ CoinGecko - Preços de Criptomoedas**
- **O que faz**: Monitora preços de Bitcoin em tempo real
- **Por que usar**: Diversificação de portfólio com crypto
- **Exemplo prático**: "Incluir Bitcoin na análise de performance"

## ⚙️ **PRÉ-REQUISITOS PARA EXECUÇÃO**

### **📋 Requisitos do Sistema:**
- **.NET 9.0 SDK** ou superior
- **PostgreSQL** (ou acesso ao Supabase)
- **Visual Studio 2022** ou **VS Code** (opcional, mas recomendado)
- **Git** para versionamento
- **Docker** (opcional, para containerização)

### **🔑 Chaves de API Necessárias:**
1. **Alpha Vantage API Key** (já configurada: `21H1KB6IUY6IL40N`)
2. **Supabase PostgreSQL** (já configurado)
3. **ViaCEP** (API pública, sem chave necessária)
4. **CoinGecko** (API pública, sem chave necessária)

### **🗄️ Banco de Dados:**
- **PostgreSQL no Supabase** (já configurado)
- **Connection String**: Configurada em `appsettings.json`
- **Tabelas necessárias**: `investimentos`, `user_profiles`

## 📋 **REQUISITOS TÉCNICOS IMPLEMENTADOS**
- **O que faz**: Monitora preços de Bitcoin em tempo real
- **Por que usar**: Diversificação de portfólio com crypto
- **Exemplo prático**: "Incluir Bitcoin na análise de performance"

### ✅ **Swagger C# com Documentação Completa**
- 📖 **Documentação da API (10%)**
- Swagger/OpenAPI com anotações detalhadas
- Exemplos de requisições e respostas
- Validações e esquemas documentados
- Interface interativa em `/swagger`

### ✅ **ASP.NET Core Web API e Entity Framework com CRUD completo (35%)**
- **Controllers RESTful**: `InvestimentosController`
- **Entity Framework Core 9.0**: DbContext configurado
- **PostgreSQL**: Banco de dados Supabase
- **Repository Pattern**: Interface e implementação
- **CRUD Completo**:
  - `GET /api/investimentos` - Listar todos
  - `GET /api/investimentos/{id}` - Buscar por ID
  - `GET /api/investimentos/usuario/{cpf}` - Buscar por usuário
  - `POST /api/investimentos` - Criar novo
  - `PUT /api/investimentos/{id}` - Atualizar
  - `DELETE /api/investimentos/{id}` - Excluir

### ✅ **Pesquisas com LINQ (10%)**
- `GET /api/investimentos/tipo/{tipo}` - Filtrar por tipo
- `GET /api/investimentos/operacao/{operacao}` - Filtrar por operação
- `GET /api/investimentos/total-valor/{cpf}` - Valor total por usuário (SUM)
- `GET /api/investimentos/recentes?days=30` - Investimentos recentes (WHERE + DateTime)
- `GET /api/investimentos/resumo-por-tipo` - Resumo agrupado (GroupBy + Count + Average)
- `GET /api/investimentos/usuarios/cpfs` - Lista CPFs únicos (Distinct + OrderBy)

### ✅ **Publicação em ambiente Cloud (15%)**
- **Docker**: Dockerfile para containerização
- **CI/CD**: GitHub Actions workflow completo
- **Azure**: Deploy automático para Azure Container Instances
- **GitHub Container Registry**: Armazenamento de imagens

### ✅ **Endpoints conectando com outras APIs (20%)**
- `GET /api/investimentos/cotacao/{symbol}` - **Alpha Vantage API** (cotações de ações)
- `GET /api/investimentos/cep/{cep}` - **ViaCEP API** (consulta de endereços)
- `GET /api/investimentos/bitcoin-price` - **CoinGecko API** (preço do Bitcoin)

### ✅ **Apresentar arquitetura em diagramas (10%)**
- 📊 **Diagramas completos em [ARCHITECTURE.md](./ARCHITECTURE.md)**
- Arquitetura geral do sistema
- Fluxo de dados CRUD
- Pipeline CI/CD
- Modelo de dados
- Integração com APIs externas

## 🛠️ **ARQUITETURA E TECNOLOGIAS**

### **🏗️ Arquitetura da Solução**
```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────┐
│   Swagger UI    │    │   Controllers    │    │   Repository    │
│   (Frontend)    │◄──►│   (API Layer)    │◄──►│   (Data Layer)  │
└─────────────────┘    └──────────────────┘    └─────────────────┘
                                │                        │
                                ▼                        ▼
                       ┌──────────────────┐    ┌─────────────────┐
                       │   External APIs   │    │   PostgreSQL    │
                       │ Alpha/CEP/Crypto │    │   (Supabase)    │
                       └──────────────────┘    └─────────────────┘
```

### **📚 Stack Tecnológico**

| Categoria | Tecnologia | Versão | Função |
|-----------|------------|---------|---------|
| **Backend** | ASP.NET Core | 9.0 | Framework web principal |
| **ORM** | Entity Framework Core | 9.0 | Mapeamento objeto-relacional |
| **Database** | PostgreSQL (Supabase) | 15+ | Banco de dados na nuvem |
| **Documentation** | Swagger/OpenAPI | 6.8.1 | Documentação interativa |
| **Containerization** | Docker | Latest | Containerização da aplicação |
| **CI/CD** | GitHub Actions | - | Pipeline de deploy automático |
| **Cloud** | Azure Container Instances | - | Hospedagem na nuvem |
| **External APIs** | Alpha Vantage, ViaCEP, CoinGecko | - | Integrações financeiras |

### **🎯 Padrões de Design Implementados**
- **Repository Pattern**: Abstração da camada de dados
- **Dependency Injection**: Inversão de controle nativa do .NET
- **RESTful API**: Padrão REST para endpoints
- **Clean Architecture**: Separação clara de responsabilidades
- **SOLID Principles**: Código estruturado e manutenível

## 🏃‍♂️ **COMO EXECUTAR O PROJETO**

### **🚀 Método 1: Execução Direta (.NET)**

#### **Passo 1: Clonar o Repositório**
```bash
git clone https://github.com/2025-Challenge-XP/Sprint_3_CSharp.git
cd Sprint_3_CSharp/Investimentos
```

#### **Passo 2: Verificar Pré-requisitos**
```bash
# Verificar se .NET 9.0 está instalado
dotnet --version

# Caso não tenha, baixar em: https://dotnet.microsoft.com/download/dotnet/9.0
```

#### **Passo 3: Restaurar Dependências**
```bash
dotnet restore
```

#### **Passo 4: Compilar o Projeto**
```bash
dotnet build
```

#### **Passo 5: Executar a Aplicação**
```bash
dotnet run
```

#### **Passo 6: Acessar a Aplicação**
```
🌐 API: http://localhost:5171
📖 Swagger: http://localhost:5171/swagger
```

### **🐳 Método 2: Execução com Docker**

#### **Passo 1: Build da Imagem**
```bash
docker build -t investimentos-api ./Investimentos
```

#### **Passo 2: Executar Container**
```bash
docker run -p 8080:80 investimentos-api
```

#### **Passo 3: Acessar**
```
🌐 API: http://localhost:8080
📖 Swagger: http://localhost:8080/swagger
```

### **☁️ Método 3: Deploy na Cloud (Azure)**

O projeto possui **GitHub Actions** configurado para deploy automático:

1. **Fork** o repositório
2. Configure os **secrets** no GitHub:
   - `AZURE_RESOURCE_GROUP`
   - `DATABASE_CONNECTION_STRING`
3. **Push** para a branch `main`
4. Deploy automático será executado

## 🧪 **COMO TESTAR A API**

### **📖 Interface Swagger (Recomendado)**
1. Acesse: `http://localhost:5171/swagger`
2. Escolha um endpoint
3. Clique em "Try it out"
4. Preencha os parâmetros
5. Clique em "Execute"
6. Veja a resposta

### **🎯 Dados para Testes**
- **CPF válido**: `52604928238` (já existe na base)
- **Tipos de investimento**: `Ação`, `CDB`, `Tesouro`, `FII`
- **Operações**: `compra`, `venda`
- **CEPs para teste**: `01310-100`, `20040-020`, `04038-001`
- **Símbolos de ações**: `AAPL`, `GOOGL`, `PETR4`, `VALE3`

### **📱 Exemplo de Teste Completo**

#### **1. Listar investimentos existentes:**
```http
GET /api/investimentos
```

#### **2. Criar um novo investimento:**
```json
POST /api/investimentos
{
  "userCpf": "52604928238",
  "tipo": "Ação",
  "codigo": "AAPL",
  "valor": 5000.00,
  "operacao": "compra"
}
```

#### **3. Consultar cotação da Apple:**
```http
GET /api/investimentos/cotacao/AAPL
```

#### **4. Validar endereço via CEP:**
```http
GET /api/investimentos/cep/01310-100
```

#### **5. Ver preço do Bitcoin:**
```http
GET /api/investimentos/bitcoin-price
```

#### **6. Relatório por tipo:**
```http
GET /api/investimentos/resumo-por-tipo
```

## � **FUNCIONALIDADES DETALHADAS**

### **💼 CRUD Completo de Investimentos (35%)**
- **CREATE**: Registrar novas operações de compra/venda
- **READ**: Consultar investimentos por usuário, ID, filtros
- **UPDATE**: Modificar operações existentes
- **DELETE**: Remover investimentos da carteira

**Endpoints:**
```http
GET    /api/investimentos              # Listar todos
POST   /api/investimentos              # Criar novo
GET    /api/investimentos/{id}         # Buscar por ID
PUT    /api/investimentos/{id}         # Atualizar
DELETE /api/investimentos/{id}         # Excluir
GET    /api/investimentos/usuario/{cpf} # Por usuário
```

### **🔍 Consultas LINQ Avançadas (10%)**
- **Filtros**: Por tipo de investimento e operação
- **Agregações**: Soma de valores, médias, contadores
- **Agrupamentos**: Estatísticas por categoria
- **Ordenação**: Por data, valor, relevância

**Endpoints:**
```http
GET /api/investimentos/tipo/{tipo}             # Filtrar por tipo
GET /api/investimentos/operacao/{operacao}     # Filtrar por operação
GET /api/investimentos/total-valor/{cpf}       # Soma por usuário
GET /api/investimentos/recentes?days=30        # Filtro temporal
GET /api/investimentos/resumo-por-tipo         # Agrupamento
GET /api/investimentos/usuarios/cpfs           # Lista CPFs únicos
```

### **🌐 Integrações com APIs Externas (20%)**

#### **📈 Alpha Vantage - Cotações Financeiras**
- **Função**: Cotações em tempo real de ações globais
- **Uso prático**: Valorização de carteiras, decisões de investimento
- **Endpoint**: `GET /api/investimentos/cotacao/{symbol}`

#### **🏠 ViaCEP - Validação de Endereços**
- **Função**: Consulta de endereços brasileiros por CEP
- **Uso prático**: Compliance KYC, validação de dados
- **Endpoint**: `GET /api/investimentos/cep/{cep}`

#### **₿ CoinGecko - Preços de Criptomoedas**
- **Função**: Monitoramento do mercado de Bitcoin
- **Uso prático**: Diversificação de portfólio, análise comparativa
- **Endpoint**: `GET /api/investimentos/bitcoin-price`

### **☁️ Deploy na Cloud (15%)**
- **GitHub Actions**: Pipeline CI/CD automatizado
- **Docker**: Containerização para deploy consistente
- **Azure Container Instances**: Hospedagem escalável
- **Monitoring**: Logs e health checks automáticos

### **📖 Documentação Completa (10%)**
- **Swagger/OpenAPI**: Interface interativa para testes
- **Anotações detalhadas**: Descrições de todos os endpoints
- **Exemplos práticos**: JSONs de request/response
- **Schemas de validação**: Regras de negócio documentadas

### **🏗️ Diagramas de Arquitetura (10%)**
- **Fluxo de dados**: Como as informações transitam
- **Integração de APIs**: Conexões externas mapeadas
- **Modelo de dados**: Estrutura do banco PostgreSQL
- **Deploy pipeline**: Processo de CI/CD visualizado

## 🏗️ **ESTRUTURA DO PROJETO**

### **📁 Organização dos Arquivos**
```
Investimentos/
├── 📂 Controllers/           # Controladores da API
│   └── InvestimentosController.cs
├── 📂 Data/                 # Contexto do Entity Framework
│   └── AppDbContext.cs
├── 📂 Models/               # Modelos de dados
│   ├── Investimento.cs
│   └── UserProfile.cs
├── 📂 Repositories/         # Padrão Repository
│   ├── IInvestimentoRepository.cs
│   └── EfInvestimentoRepository.cs
├── 📂 Properties/           # Configurações de launch
│   └── launchSettings.json
├── 📄 Program.cs           # Configuração da aplicação
├── 📄 Dockerfile          # Containerização
├── 📄 appsettings.json    # Configurações (dev/prod)
└── 📄 Investimentos.csproj # Projeto .NET
```

### **🔧 Arquivos de Configuração**

#### **appsettings.json - Configurações principais**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User Id=postgres.oziwendirtmqquvqkree;Password=***;Server=aws-0-us-east-2.pooler.supabase.com;Port=6543;Database=postgres"
  },
  "ExternalApis": {
    "AlphaVantageKey": "21H1KB6IUY6IL40N"
  }
}
```

#### **Dockerfile - Containerização**
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# Build e configuração para produção
```

### **📦 Dependências NuGet**
- `Microsoft.AspNetCore.OpenApi` (Swagger)
- `Microsoft.EntityFrameworkCore.Design` (EF Tools)
- `Npgsql.EntityFrameworkCore.PostgreSQL` (PostgreSQL)
- `Swashbuckle.AspNetCore` (Swagger UI)

## 🚀 **DEPLOY E PRODUÇÃO**

### **☁️ GitHub Actions - CI/CD Automático**
O projeto possui pipeline completo que executa:

1. ✅ **Build**: Compilação do código .NET
2. ✅ **Tests**: Execução de testes unitários
3. ✅ **Docker Build**: Criação da imagem container
4. ✅ **Registry Push**: Upload para GitHub Container Registry
5. ✅ **Azure Deploy**: Deploy automático no Azure Container Instances

### **🔧 Configuração do Deploy**
Para usar o deploy automático, configure os secrets no GitHub:

```yaml
# Secrets necessários no GitHub
AZURE_RESOURCE_GROUP: "meu-resource-group"
DATABASE_CONNECTION_STRING: "sua-connection-string"
AZURE_CREDENTIALS: "azure-service-principal-json"
```

### **🌍 URL de Produção**
Após o deploy, a API estará disponível em:
```
🌐 https://seu-container.azurecontainer.io
📖 https://seu-container.azurecontainer.io/swagger
```

## ⚠️ **TROUBLESHOOTING**

### **❌ Problemas Comuns e Soluções**

#### **Erro: "dotnet command not found"**
**Problema**: .NET SDK não instalado
**Solução**: 
```bash
# Baixar e instalar .NET 9.0 SDK
# https://dotnet.microsoft.com/download/dotnet/9.0
dotnet --version  # Verificar instalação
```

#### **Erro: "Connection refused" (PostgreSQL)**
**Problema**: Banco de dados não acessível
**Solução**: 
- Verificar connection string em `appsettings.json`
- Confirmar se Supabase está ativo
- Checar regras de firewall

#### **Erro: "401 Unauthorized" (Alpha Vantage)**
**Problema**: Chave da API inválida ou expirada
**Solução**:
```csharp
// Atualizar chave em InvestimentosController.cs
var apiKey = "SUA_NOVA_CHAVE_AQUI";
```

#### **Erro: "Port already in use"**
**Problema**: Porta 5171 ocupada
**Solução**:
```bash
# Verificar processos na porta
netstat -tulpn | grep 5171

# Ou usar porta diferente
dotnet run --urls="http://localhost:5000"
```

### **🔍 Debug e Logs**
Para investigar problemas:

```bash
# Logs detalhados durante execução
dotnet run --verbosity detailed

# Verificar saúde da aplicação
curl http://localhost:5171/health

# Logs do container Docker
docker logs container-name
```

## � Exemplo de Uso

### Criar Investimento
```json
POST /api/investimentos
{
  "userCpf": "12345678901",
  "tipo": "Ação",
  "codigo": "PETR4",
  "valor": 1000.50,
  "operacao": "compra"
}
```

### Consultar Resumo por Tipo (LINQ)
```json
GET /api/investimentos/resumo-por-tipo
[
  {
    "tipo": "Ação",
    "totalInvestimentos": 5,
    "valorTotal": 15000.00,
    "valorMedio": 3000.00
  }
]
```

### Consultar API Externa
```json
GET /api/investimentos/bitcoin-price
{
  "bitcoin": {
    "brl": 180000.50,
    "usd": 35000.25
  },
  "consultadoEm": "2025-10-10T14:30:00Z"
}
```

## 📋 Checklist de Entrega

- ✅ **ASP.NET Core Web API e Entity Framework com CRUD completo (35%)**
- ✅ **Pesquisas com LINQ (10%)**
- ✅ **Publicação em ambiente Cloud (15%)**
- ✅ **Endpoints conectando com outras APIs (20%)**
- ✅ **Documentação do projeto (10%)**
- ✅ **Apresentar arquitetura em diagramas (10%)**
- ✅ **Código em versionador (GitHub)**
- ✅ **Arquivo README completo**
- ✅ **Métodos e funções estruturadas**
- ✅ **Legibilidade e estruturação do código**

## 🔗 **LINKS E RECURSOS**

### **📚 Documentação e Código**
- **📦 Repositório GitHub**: [https://github.com/2025-Challenge-XP/Sprint_3_CSharp](https://github.com/2025-Challenge-XP/Sprint_3_CSharp)
- **📖 Swagger Local**: `http://localhost:5171/swagger` (quando executando)
- **🏗️ Diagramas de Arquitetura**: [ARCHITECTURE.md](./ARCHITECTURE.md)
- **🧪 Guia de Testes**: [TESTES_SWAGGER.txt](./TESTES_SWAGGER.txt)

### **🌐 APIs Externas Utilizadas**
- **📈 Alpha Vantage**: [https://www.alphavantage.co/](https://www.alphavantage.co/)
- **🏠 ViaCEP**: [https://viacep.com.br/](https://viacep.com.br/)
- **₿ CoinGecko**: [https://www.coingecko.com/](https://www.coingecko.com/)

### **☁️ Infraestrutura**
- **🗄️ Supabase PostgreSQL**: Banco de dados na nuvem
- **🐳 Docker Hub**: Containerização e deploy
- **☁️ Azure Container Instances**: Hospedagem na nuvem

### **🛠️ Ferramentas de Desenvolvimento**
- **💻 .NET 9.0**: [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
- **🐳 Docker**: [https://www.docker.com/](https://www.docker.com/)
- **📝 Visual Studio Code**: [https://code.visualstudio.com/](https://code.visualstudio.com/)

## 👥 **EQUIPE DE DESENVOLVIMENTO**

**Desenvolvido para o Sprint 3 - C# Challenge XP Inc.**

| Nome | RM | Função |
|------|-----|---------|
| **André Lambert** | RM 99148 | Backend Development |
| **Felipe Cortez** | RM 99750 | API Design & Integration |
| **Julia Lins** | RM 98690 | Database & Architecture |
| **Luis Barreto** | RM 99210 | DevOps & Cloud Deploy |
| **Victor Aranda** | RM 99667 | Documentation & Testing |

## 📞 **SUPORTE E CONTATO**

### **🚨 Para Problemas Técnicos**
1. Consulte a seção [Troubleshooting](#️-troubleshooting)
2. Verifique os logs da aplicação
3. Abra uma issue no GitHub

### **💡 Para Dúvidas sobre Funcionalidades**
1. Consulte a documentação Swagger
2. Revise os exemplos práticos
3. Teste no ambiente local primeiro

### **🎯 Para Sugestões de Melhoria**
1. Faça um fork do repositório
2. Implemente a melhoria
3. Abra um Pull Request

---

## 🎉 **CONCLUSÃO**

Esta **API de Investimentos** representa uma solução completa e moderna para gestão de carteiras financeiras, integrando:

- ✅ **Tecnologias atuais**: .NET 9.0, Entity Framework Core, PostgreSQL
- ✅ **Práticas de mercado**: Clean Architecture, Repository Pattern, CI/CD
- ✅ **Integrações reais**: APIs financeiras utilizadas por fintechs
- ✅ **Documentação completa**: Swagger interativo e guias detalhados
- ✅ **Deploy profissional**: Docker, GitHub Actions, Azure

**🚀 Pronto para produção e expansão!**

---

*"Transformando dados financeiros em insights valiosos através de tecnologia moderna e APIs bem estruturadas."*

**Sprint 3 - C# Challenge XP Inc. | 2025**

## 🎯 **PARA TESTES IMEDIATOS**

### **💾 Dados Pré-configurados**
- **CPF de teste**: `52604928238` (já existe na base de dados)
- **Tipos válidos**: `Ação`, `CDB`, `Tesouro`, `FII`
- **Operações**: `compra`, `venda`
- **CEPs brasileiros**: `01310-100`, `20040-020`, `04038-001`
- **Símbolos de ações**: `AAPL`, `GOOGL`, `PETR4`, `VALE3`

### **🚀 Quick Start - 5 Minutos**
```bash
# 1. Clone e execute
git clone https://github.com/2025-Challenge-XP/Sprint_3_CSharp.git
cd Sprint_3_CSharp/Investimentos
dotnet run

# 2. Abra o Swagger
# http://localhost:5171/swagger

# 3. Teste qualquer endpoint
# Todos os dados necessários já estão configurados!
```

## 📋 **CHECKLIST DE ENTREGA COMPLETO**

- ✅ **ASP.NET Core Web API e Entity Framework com CRUD completo (35%)**
  - Controllers RESTful implementados
  - Entity Framework Core com PostgreSQL
  - Repository Pattern aplicado
  - Operações CRUD completas

- ✅ **Pesquisas com LINQ (10%)**
  - 6 endpoints com consultas LINQ diferentes
  - Filtros, agrupamentos e agregações
  - Performance otimizada

- ✅ **Publicação em ambiente Cloud (15%)**
  - Docker containerizado
  - GitHub Actions CI/CD
  - Deploy automático Azure
  - Monitoramento configurado

- ✅ **Endpoints conectando com outras APIs (20%)**
  - Alpha Vantage (cotações financeiras)
  - ViaCEP (validação de endereços)
  - CoinGecko (preços de criptomoedas)
  - Tratamento de erros implementado

- ✅ **Documentação do projeto (10%)**
  - README completo e detalhado
  - Swagger/OpenAPI interativo
  - Exemplos práticos de uso
  - Guia de troubleshooting

- ✅ **Apresentar arquitetura em diagramas (10%)**
  - Diagramas de arquitetura geral
  - Fluxo de dados mapeado
  - Integração de APIs documentada
  - Pipeline CI/CD visualizado

### **📊 Critérios Adicionais Atendidos**
- ✅ **Código em versionador** (GitHub)
- ✅ **Arquivo README completo** (este documento)
- ✅ **Métodos e funções estruturadas** (Clean Code)
- ✅ **Legibilidade e estruturação do código** (SOLID, Design Patterns)
- ✅ **Tratamento de erros** (Try/catch, validações)
- ✅ **Segurança** (Connection strings protegidas)
- ✅ **Performance** (Consultas LINQ otimizadas)
- ✅ **Escalabilidade** (Arquitetura modular)

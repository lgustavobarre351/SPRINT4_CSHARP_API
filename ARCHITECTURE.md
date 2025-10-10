# 📊 Diagramas de Arquitetura - Investimentos API

## 🏗️ Arquitetura Geral do Sistema

```mermaid
graph TB
    Client[Cliente/Browser] --> API[ASP.NET Core Web API]
    API --> Controller[InvestimentosController]
    Controller --> Repository[IInvestimentoRepository]
    Repository --> EF[Entity Framework Core]
    EF --> DB[(PostgreSQL Database)]
    
    Controller --> HTTP[HttpClient]
    HTTP --> ExtAPI1[Alpha Vantage API]
    HTTP --> ExtAPI2[ViaCEP API]
    HTTP --> ExtAPI3[CoinGecko API]
    
    API --> Swagger[Swagger/OpenAPI]
    API --> CORS[CORS Middleware]
    API --> Static[Static Files]
    
    subgraph Cloud[" Azure Cloud "]
        Container[Docker Container]
        ACI[Azure Container Instances]
        Container --> ACI
    end
    
    API --> Container
```

## 🔄 Fluxo de Dados CRUD

```mermaid
sequenceDiagram
    participant C as Cliente
    participant API as Web API
    participant Ctrl as Controller
    participant Repo as Repository
    participant EF as Entity Framework
    participant DB as PostgreSQL
    
    Note over C,DB: CREATE - Criar Investimento
    C->>API: POST /api/investimentos
    API->>Ctrl: InvestimentosController
    Ctrl->>Repo: CreateAsync(investimento)
    Repo->>EF: Add(investimento)
    EF->>DB: INSERT INTO investimentos
    DB-->>EF: Row Created
    EF-->>Repo: Investimento
    Repo-->>Ctrl: Investimento
    Ctrl-->>API: 201 Created
    API-->>C: Investimento criado
    
    Note over C,DB: READ - Buscar Investimentos
    C->>API: GET /api/investimentos
    API->>Ctrl: InvestimentosController
    Ctrl->>Repo: GetAllAsync()
    Repo->>EF: ToListAsync()
    EF->>DB: SELECT * FROM investimentos
    DB-->>EF: ResultSet
    EF-->>Repo: List<Investimento>
    Repo-->>Ctrl: List<Investimento>
    Ctrl-->>API: 200 OK
    API-->>C: Lista de investimentos
```

## 🔍 Arquitetura de Consultas LINQ

```mermaid
graph LR
    Client[Cliente] --> Controller[Controller]
    Controller --> Repository[Repository]
    
    subgraph LINQ[" Consultas LINQ "]
        Repository --> Q1[GetByTipoAsync]
        Repository --> Q2[GetByOperacaoAsync]
        Repository --> Q3[GetTotalValueByUserAsync]
        Repository --> Q4[GetRecentInvestmentsAsync]
        Repository --> Q5[GetInvestmentSummaryByTypeAsync]
    end
    
    Q1 --> EF[Entity Framework]
    Q2 --> EF
    Q3 --> EF
    Q4 --> EF
    Q5 --> EF
    
    EF --> DB[(PostgreSQL)]
    
    subgraph Operações[" Operações SQL Geradas "]
        EF --> WHERE[WHERE Clauses]
        EF --> GROUP[GROUP BY]
        EF --> SUM[SUM/COUNT]
        EF --> ORDER[ORDER BY]
    end
```

## 🌐 Integração com APIs Externas

```mermaid
graph TB
    Controller[InvestimentosController] --> HttpClient[HttpClient Service]
    
    subgraph External[" APIs Externas "]
        HttpClient --> API1[Alpha Vantage<br/>Cotações de Ações]
        HttpClient --> API2[ViaCEP<br/>Consulta CEP]
        HttpClient --> API3[CoinGecko<br/>Preço Bitcoin]
    end
    
    API1 --> JSON1[JSON Response<br/>Stock Prices]
    API2 --> JSON2[JSON Response<br/>Address Data]
    API3 --> JSON3[JSON Response<br/>Bitcoin Price]
    
    JSON1 --> Controller
    JSON2 --> Controller
    JSON3 --> Controller
    
    Controller --> Client[Cliente Final]
    
    Note[Timeout & Error Handling<br/>Configurados para todas as APIs]
```

## 🚀 Pipeline de Deploy (CI/CD)

```mermaid
graph LR
    Dev[Developer] --> Git[GitHub Repository]
    Git --> Trigger{Push to main?}
    
    Trigger -->|Yes| Build[GitHub Actions<br/>Build & Test]
    Build --> Docker[Docker Build]
    Docker --> Registry[GitHub Container Registry]
    Registry --> Deploy[Azure Container Instances]
    
    Deploy --> Live[Aplicação Live na Cloud]
    
    Trigger -->|No| End[Fim]
    
    subgraph Pipeline[" CI/CD Pipeline "]
        Build
        Docker
        Registry
        Deploy
    end
    
    style Live fill:#90EE90
    style Pipeline fill:#E6F3FF
```

## 🗄️ Modelo de Dados

```mermaid
erDiagram
    INVESTIMENTO {
        guid Id PK
        string UserCpf FK
        guid UserId
        string Tipo
        string Codigo
        decimal Valor
        string Operacao
        datetime CriadoEm
        datetime AlteradoEm
    }
    
    USER_PROFILES {
        guid id PK
        string cpf UK
        string nome
        datetime created_at
    }
    
    USER_PROFILES ||--o{ INVESTIMENTO : "possui"
    
    INVESTIMENTO {
        string Tipo "Ação, CDB, Tesouro, etc"
        string Operacao "compra, venda"
        decimal Valor "Valor em Reais"
    }
```

## 🛡️ Camadas da Aplicação

```mermaid
graph TB
    subgraph Presentation[" 🎨 Camada de Apresentação "]
        Controller[Controllers]
        Swagger[Swagger/OpenAPI]
        Middleware[Middleware Pipeline]
    end
    
    subgraph Business[" 💼 Camada de Negócio "]
        Repository[Repository Pattern]
        Services[Services]
        Validation[Data Validation]
    end
    
    subgraph Data[" 🗄️ Camada de Dados "]
        EF[Entity Framework Core]
        DbContext[AppDbContext]
        Migrations[Database Migrations]
    end
    
    subgraph External[" 🌐 Serviços Externos "]
        HttpClient[HTTP Client]
        APIs[External APIs]
    end
    
    Controller --> Repository
    Controller --> Services
    Repository --> EF
    EF --> DbContext
    Services --> HttpClient
    HttpClient --> APIs
    
    style Presentation fill:#FFE6E6
    style Business fill:#E6F3FF
    style Data fill:#E6FFE6
    style External fill:#FFF0E6
```

## 📊 Métricas e Monitoramento

```mermaid
graph LR
    App[Aplicação] --> Logs[Application Logs]
    App --> Metrics[Performance Metrics]
    App --> Health[Health Checks]
    
    Logs --> Console[Console Output]
    Metrics --> Telemetry[Application Insights]
    Health --> Status[Status Endpoint]
    
    subgraph Observability[" 📈 Observabilidade "]
        Console
        Telemetry
        Status
    end
    
    Observability --> Monitor[Azure Monitor]
    Monitor --> Alerts[Alertas Automáticos]
```

---

### 🔍 Legenda dos Componentes

| Componente | Descrição | Tecnologia |
|------------|-----------|------------|
| **Web API** | Interface REST da aplicação | ASP.NET Core 9.0 |
| **Entity Framework** | ORM para acesso a dados | EF Core 9.0 |
| **PostgreSQL** | Banco de dados relacional | PostgreSQL (Supabase) |
| **Repository Pattern** | Abstração da camada de dados | Interface + Implementação |
| **LINQ** | Consultas integradas à linguagem | C# LINQ to Entities |
| **HttpClient** | Cliente HTTP para APIs externas | .NET HttpClient |
| **Docker** | Containerização da aplicação | Docker |
| **GitHub Actions** | Pipeline CI/CD | YAML Workflow |
| **Azure** | Plataforma de cloud | Azure Container Instances |
| **Swagger** | Documentação da API | Swashbuckle.AspNetCore |
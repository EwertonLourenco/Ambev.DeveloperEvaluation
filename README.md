# Ambev Developer Evaluation

Uma aplicação full-stack para gestão simples de **Pedidos** e **Usuários** com autenticação **JWT**.
Back-end em **ASP.NET Core (.NET 8)**, front-end em **Angular**, e infraestrutura com **Docker Compose** (PostgreSQL, Redis e MongoDB).

> **Status**: funcionando com Docker (UI servida por Nginx com `env.js` em runtime) ou modo híbrido (API/DB no Docker e UI local).

---

## Funcionalidades

* **Autenticação (JWT)** – login e proteção de rotas.
* **Home** – atalhos e **ping** da API.
* **Pedidos** – listar, paginação, criar, editar (cálculos de subtotal, desconto, total).
* **Usuários** – criação e consulta (conforme endpoints disponíveis).
* **Swagger** – documentação e teste da API.

---

## Stack

* **API**: ASP.NET Core (.NET 8) + MediatR + FluentValidation + AutoMapper
* **Banco**: PostgreSQL (dados), Redis (cache), MongoDB (no-sql opcional)
* **UI**: Angular (standalone components) + Nginx (produção)
* **Infra**: Docker Compose
* **Extras**: Serilog, HealthChecks, CORS configurável

---

## Estrutura (resumo)

```
template/
├── backend/
│   └── src/
│       ├── Ambev.DeveloperEvaluation.WebApi/   # API (Swagger, CORS, JWT)
│       ├── Ambev.DeveloperEvaluation.ORM/      # EF Core, Contexto, Migrations
│       └── ...
└── backend/src/Ambev.DeveloperEvaluation.UI/   # Angular app
    ├── ops/nginx/default.conf                  # Nginx (SPA + /api proxy opcional)
    ├── ops/nginx/docker-entrypoint.sh          # Gera env.js em runtime
    └── ...
```

---

## ▶Como rodar (Docker – **recomendado**)

> Pré-requisitos: **Docker Desktop**.

1. **Suba tudo** (API, UI, DBs, cache):

```bash
docker compose up -d --build
```

2. **Acesse**:

* UI: [http://localhost](http://localhost)
* API (Swagger): [http://localhost:8080/swagger](http://localhost:8080/swagger)

> A UI injeta `env.js` em runtime via `docker-entrypoint.sh` e usa `API_BASE_URL` (default: `http://ambev.developerevaluation.webapi:8080` em rede Docker).

3. **Parar**:

```bash
docker compose down
```

---

## Modo híbrido (DB + API no Docker, UI local)

> Útil para desenvolver o front com hot reload.

1. Suba **somente** banco/redis/mongo e a API:

```bash
docker compose up -d ambev.developerevaluation.database \
                      ambev.developerevaluation.cache \
                      ambev.developerevaluation.nosql \
                      ambev.developerevaluation.webapi
```

2. No diretório da UI:

```bash
npm install
npm start
# ou: ng serve
```

3. Acesse a UI: `http://localhost:4200`

> **CORS**: a API está liberando origens `http://localhost` e `http://localhost:4200`.
> Se precisar, ajuste a policy no `Program.cs`.

---

## ⚙️ Variáveis/Portas principais

* **API**

  * Porta: `8080` (HTTP)
  * `ConnectionStrings__DefaultConnection=Host=ambev_developer_evaluation_database;Port=5432;...`
  * CORS liberado para: `http://localhost`, `http://localhost:4200` (ajustável)
* **UI (Nginx)**

  * Porta: `80`
  * `API_BASE_URL` (injeta `window.__env.API_BASE_URL` em `/env.js`)
* **PostgreSQL**: `15432:5432`
* **Redis**: `6379:6379`
* **MongoDB**: `27017:27017`

---

## Autenticação

* **JWT Bearer**.
* Faça **login** via UI ou pelo Swagger nos endpoints de `Auth`.
* Crie usuários pelos endpoints de **Users** (quando habilitados) ou conforme estratégia da sua API.

---

## Endpoints (amostra)

* `GET /api/Orders` – lista (paginação, busca)
* `GET /api/Orders/{id}` – detalhe
* `POST /api/Orders` – cria pedido
* `PUT /api/Orders/{id}` – atualiza itens (cálculo de desconto automático no domínio)
* `DELETE /api/Orders/{id}` – remove
* `POST /api/Auth` – autenticação (JWT)

> Documentação completa em **Swagger**: `http://localhost:8080/swagger`

---

## Desenvolvimento

### Requisitos

* .NET 8 SDK
* Node 20+
* Angular CLI (recomendado)
* Docker Desktop

### Rodando a API local (sem Docker)

```bash
cd backend/src/Ambev.DeveloperEvaluation.WebApi
dotnet restore
dotnet run
# Swagger em: http://localhost:8080/swagger (se ASPNETCORE_URLS=...)
```

### Rodando a UI local

```bash
cd backend/src/Ambev.DeveloperEvaluation.UI
npm install
npm start  # ng serve
```

> **Base URL da API** na UI local: use o `env.js` (pasta `public/`) ou configure um proxy.
> Em produção (Docker/Nginx), o `env.js` é gerado em runtime pelo `docker-entrypoint.sh`.

---

## Observações de arquitetura

* **Regras de desconto** de pedido no domínio:

  * Mínimo de itens para aplicar desconto;
  * ≥ 5 unidades → 10%, ≥ 10 unidades → 20%;
  * Totais recalculados sempre que itens mudam.
* **Camadas**: WebApi → Application (MediatR) → Domain → Infra (ORM/EF Core).
* **UI**: Angular standalone + roteamento, componentes para Home, Login, Orders (lista/detalhe) e Users.

---

## Troubleshooting

* **UI mostra “Offline” ao verificar API**
  → Verifique CORS (origem `http://localhost` / `:4200`) e `API_BASE_URL` no `env.js`.

* **Aparece “Welcome to nginx!”**
  → O `default.conf` deve apontar `root /usr/share/nginx/html;` e usar
  `try_files $uri $uri/ /index.html;` para SPA.
  → Confirme que a cópia do `dist/` foi feita para `/usr/share/nginx/html`.

* **`env.js` não gerado** (container da UI sai com erro 127)
  → O script `ops/nginx/docker-entrypoint.sh` precisa de permissão de execução e estar em `/docker-entrypoint.d/50-env.sh`.

* **CORS 403/401**
  → Garanta o cabeçalho `Authorization: Bearer <token>` nas chamadas protegidas e que a origem esteja liberada na policy.

---

## Licença

Projeto para avaliação técnica e estudos. Ajuste conforme a política da sua organização.

---

# EstoqueIntranet

Sistema de controle de estoque para uso em intranet. O objetivo é permitir o cadastro de produtos e unidades de medida, acompanhar movimentações e, futuramente, identificar itens por QR Code em computadores, celulares e coletores Zebra.

## Tecnologias atuais

- .NET 9 / ASP.NET Core
- Blazor Server para a interface web
- Entity Framework Core com SQLite

O projeto existente foi mantido em .NET, em vez de migrado para a arquitetura React, FastAPI e PostgreSQL que apareceu como sugestão nos requisitos iniciais.

## Estrutura

- `EstoqueIntranet.Api`: API ASP.NET Core, contexto Entity Framework, modelos de domínio e inicialização de dados de exemplo.
- `EstoqueIntranet.Web`: aplicação web Blazor Server.
- `EstoqueIntranet.sln`: solução Visual Studio com os dois projetos.

## O que já está implementado

- Modelos para produtos, unidades de medida, localizações, usuários, movimentações e QR Codes.
- Configuração das relações e índices principais do banco em `AppDbContext`.
- Inicialização do SQLite e carga de unidades, localização, usuários, produtos e QR Codes de exemplo em `DbInitializer`.
- Endpoint `GET /api/health`.
- Endpoint `GET /api/produtos` para listar produtos com unidade e localização.
- Endpoint `GET /api/produtos/{codigo}` para consultar um produto por código.
- Página e navegação iniciais do template Blazor; ainda não constituem o dashboard de estoque.

## O que falta

- Operações de cadastro e edição de produtos e unidades.
- Movimentações funcionais de entrada, saída e ajuste, com validação de saldo e atualização atômica.
- Autenticação, autorização e identificação do usuário em cada movimentação.
- Cadastro e controle de pallets, geração/impressão de QR Codes e fluxo de leitura.
- Interface de estoque responsiva, dashboard e relatórios.
- Importação e validação dos dados da planilha de inventário.
- Configuração e testes de implantação na intranet, backup e acesso seguro.

## Executar localmente

Requer o .NET 9 SDK. Execute cada projeto em um terminal separado, a partir da raiz da solução:

```powershell
dotnet run --project .\EstoqueIntranet.Api
```

```powershell
dotnet run --project .\EstoqueIntranet.Web
```

As URLs locais dos perfis de desenvolvimento estão definidas em `Properties/launchSettings.json` de cada projeto. A API cria o banco SQLite e carrega dados de demonstração na primeira execução.

## Dados locais

O banco SQLite e seus arquivos auxiliares (`estoque.db`, `estoque.db-shm` e `estoque.db-wal`), além de `bin/`, `obj/` e arquivos de usuário do Visual Studio, são ignorados pelo Git. O banco contém dados locais/de demonstração e não deve ser publicado.

## Próximo passo recomendado

Implementar na API o fluxo de movimentação de estoque (entrada, saída e ajuste), validando quantidade e saldo e registrando cada operação junto com a atualização do produto. Depois, conectar a interface web e acrescentar o fluxo de QR Code.

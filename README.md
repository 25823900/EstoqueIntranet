# EstoqueIntranet

Sistema de controle de estoque para uso em intranet. O sistema deverá permitir o cadastro de produtos e unidades de medida, acompanhar movimentações e identificar itens por QR Code em computadores, celulares e coletores Zebra. A leitura de QR Code e a baixa de estoque fazem parte dos requisitos funcionais do sistema.

## Tecnologias atuais

- .NET 10 LTS / ASP.NET Core
- Blazor Server para a interface web
- Entity Framework Core com SQL Server

O projeto existente foi mantido em .NET, em vez de migrado para a arquitetura React, FastAPI e PostgreSQL que apareceu como sugestão nos requisitos iniciais.

## Requisito de QR Code e dispositivos

A leitura de QR Code faz parte do fluxo principal do sistema e deve ser considerada em todas as etapas do projeto. O desenho prevê:

- QR Codes associados a identificadores estáveis de produtos e, quando aplicável, de pallets; o saldo não será gravado no QR Code.
- Uma tela web responsiva para localizar o item, escolher a movimentação e informar a quantidade.
- Suporte a leitores Zebra que enviem o código como entrada de teclado e a leitura pela câmera do celular, conforme compatibilidade do navegador e do dispositivo.
- Consulta do item pela API e registro da movimentação no servidor, com validação e atualização transacional do saldo.
- Testes reais em computador, celular e no modelo de coletor Zebra disponível antes da implantação.

Assim, QR Code não é um módulo opcional para depois: cadastro, API, interface e movimentações devem ser implementados de forma compatível com esse fluxo.

## Estrutura

- `EstoqueIntranet.Api`: API ASP.NET Core, contexto Entity Framework, modelos de domínio e inicialização de dados de exemplo.
- `EstoqueIntranet.Web`: aplicação web Blazor Server.
- `EstoqueIntranet.sln`: solução Visual Studio com os dois projetos.

## O que já está implementado

- Modelos para produtos, unidades de medida, localizações, usuários, movimentações e QR Codes.
- Configuração das relações e índices principais do banco em `AppDbContext`.
- Migração do esquema do banco com Entity Framework Core e carga de unidades, localização, usuários, produtos e QR Codes de exemplo em `DbInitializer`.
- Endpoint `GET /api/health`.
- Endpoint `GET /api/produtos` para listar produtos com unidade e localização.
- Endpoint `GET /api/produtos/{codigo}` para consultar um produto por código.
- Página e navegação iniciais do template Blazor; ainda não constituem o dashboard de estoque.

## O que falta

- Operações de cadastro e edição de produtos e unidades.
- Movimentações funcionais de entrada, saída e ajuste, com validação de saldo e atualização atômica.
- Autenticação, autorização e identificação do usuário em cada movimentação.
- Cadastro e controle de pallets, geração/impressão de QR Codes e leitura integrada ao fluxo de movimentação.
- Integração e testes da leitura por câmera em celulares e por coletor Zebra; o comportamento final deve ser validado no modelo e navegador utilizados pela empresa.
- Interface de estoque responsiva, dashboard e relatórios.
- Importação e validação dos dados da planilha de inventário.
- Configuração e testes de implantação na intranet, backup e acesso seguro.

## Executar localmente

Requer o .NET 10 SDK e uma instância do SQL Server instalada, em execução e acessível. A connection string padrão aponta para a instância padrão em `localhost`, usando autenticação integrada do Windows. Se o SQL Server estiver em outro host ou usar uma instância nomeada (por exemplo, `localhost\SQLEXPRESS`), configure a connection string apropriada.

No desenvolvimento local, `appsettings.Development.json` confia no certificado TLS do SQL Server local. Em outros ambientes, a configuração padrão valida o certificado. Para apontar para outra instância, defina `ConnectionStrings__DefaultConnection` como variável de ambiente. Exemplo para a sessão atual do PowerShell:

```powershell
$env:ConnectionStrings__DefaultConnection = "Server=localhost\SQLEXPRESS;Database=EstoqueIntranet;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

Não armazene senhas ou outros segredos em arquivos versionados. A identidade usada pela aplicação precisa ter permissão para criar o banco na primeira execução e para aplicar alterações de esquema.

Execute cada projeto em um terminal separado, a partir da raiz da solução:

```powershell
dotnet run --project .\EstoqueIntranet.Api
```

```powershell
dotnet run --project .\EstoqueIntranet.Web
```

As URLs locais dos perfis de desenvolvimento estão definidas em `Properties/launchSettings.json` de cada projeto. Na inicialização, a API aplica as migrations pendentes e carrega os dados de demonstração se as unidades de medida ainda não existirem.

## Migrations do banco

As migrations do Entity Framework Core ficam em `EstoqueIntranet.Api/Data/Migrations`. A API aplica migrations pendentes ao iniciar. Para criar uma migration após alterar o modelo, instale a ferramenta EF Core 10 se necessário e execute na raiz da solução:

```powershell
dotnet tool install --global dotnet-ef --version 10.0.14
dotnet ef migrations add NomeDaMigration --project .\EstoqueIntranet.Api --startup-project .\EstoqueIntranet.Api --output-dir Data\Migrations
```

O comando de instalação é necessário apenas uma vez por ambiente. Revise e versione os arquivos de migration gerados.

## Banco de dados

O sistema usa SQL Server e não mantém suporte a SQLite. Os arquivos locais do banco SQLite anterior foram removidos; os dados daquele arquivo não foram migrados. As migrations criam o esquema no SQL Server, mas não importam dados de outros bancos.

Os diretórios `bin/`, `obj/` e arquivos de usuário do Visual Studio são ignorados pelo Git.

## Próximo passo recomendado

Implementar o fluxo integrado de movimentação: API para entrada, saída e ajuste com validação e atualização transacional do saldo, mais uma tela web responsiva que aceite leitura do identificador QR e funcione com câmera de celular e leitor Zebra. A geração/impressão dos códigos e a validação nos dispositivos fazem parte dessa entrega.

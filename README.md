# Erick - Trabalho Pratico 1: Locadora de Veiculos - Etapa 1

Projeto individual da Etapa 1, desenvolvido em ASP.NET Core 7 com Entity Framework Core 7 e SQL Server Express.

## Conteudo desta etapa

- Cinco entidades: `Fabricante`, `Veiculo`, `Cliente`, `Aluguel` e `Reserva`.
- Chaves primarias e estrangeiras explicitas.
- Relacionamentos configurados no `ApplicationContext`.
- Connection string para SQL Server Express em `appsettings.json`.
- Modelo conceitual em `MODELAGEM_CONCEITUAL.md`.

Controllers, CRUD, filtros, Swagger e testes nao foram antecipados, pois pertencem as proximas etapas do trabalho.

## Como abrir

1. Abra `LocadoraVeiculosErick.sln` no Visual Studio.
2. Aguarde a restauracao dos pacotes NuGet.
3. Confirme que o SQL Server Express esta instalado com a instancia `SQLEXPRESS`.
4. Use **Compilar > Compilar Solucao** para conferir o projeto.

O banco sera criado por migration quando essa operacao for feita durante a evolucao do projeto.

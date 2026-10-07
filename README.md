# Trabalho Prático 1 - Etapa 3 - Erick

Projeto ASP.NET Core 7 com Entity Framework Core 7, SQL Server e Swagger. A API possui CRUD completo de Fabricantes, Clientes, Veículos, Reservas e Aluguéis, além de sete filtros com relacionamentos.

## Executar no Visual Studio

1. Extraia todo o ZIP em uma pasta nova.
2. Abra `LocadoraVeiculosErick.sln`.
3. Aguarde a restauração dos pacotes.
4. Escolha o perfil **SQL Server local** ao lado do botão verde.
5. Pressione F5.
6. O Swagger abrirá em `http://localhost:5187/swagger`.

## Resultado da Etapa 3

- 5 entidades com CRUD completo: 25 endpoints.
- 7 consultas especiais no `FiltrosController`.
- 32 endpoints testados manualmente no Swagger.
- GETs e filtros retornaram HTTP 200.
- POSTs retornaram HTTP 201.
- PUTs e DELETEs retornaram HTTP 204.
- Relatório PDF com uma evidência para cada operação.

## Ordem para testar no Swagger

Cadastre primeiro Fabricante, Cliente, Veículo, Reserva e Aluguel. Execute os filtros antes de excluir os registros. Para excluir, use a ordem inversa: Aluguel, Reserva, Veículo, Cliente e Fabricante.

## Sete filtros

- `/api/Filtros/veiculos-por-fabricante/1`
- `/api/Filtros/reservas-por-cliente/1`
- `/api/Filtros/reservas-por-status?status=Confirmada`
- `/api/Filtros/reservas-por-periodo?inicio=2026-10-08&fim=2026-10-10`
- `/api/Filtros/alugueis-em-aberto`
- `/api/Filtros/alugueis-por-cliente/1`
- `/api/Filtros/veiculos-com-reservas`

## Entrega

O ZIP final contém o projeto completo, este README, o relatório PDF e um passo a passo simples. O repositório pessoal informado é:

`https://github.com/Erickry/LocadoraVeiculosErick`

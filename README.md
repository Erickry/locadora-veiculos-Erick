# Trabalho Prático 1 — Etapa 2 — Erick

Projeto ASP.NET Core 7 com Entity Framework Core 7, SQL Server Express e a quinta entidade individual `Reserva`.

## Executar no Visual Studio

1. Extraia todo o ZIP em uma pasta nova.
2. Abra `LocadoraVeiculosErick.sln` e aguarde a restauração dos pacotes.
3. Escolha o perfil **SQL Express** ao lado do botão verde e pressione F5.
4. Se este computador não possuir a instância SQLEXPRESS, escolha **SQL Server local**.
5. O banco `LocadoraVeiculosErickDb` será criado pelas migrations e o Swagger abrirá em `http://localhost:5187/swagger`.

## Critérios da Etapa 2

| Critério | Implementação |
|---|---|
| CRUD de todas as entidades | Fabricantes, Veículos, Clientes, Aluguéis e Reservas possuem GET, GET por ID, POST, PUT e DELETE. |
| Entity Framework com SQL Express | EF Core 7.0.20, UseSqlServer, cinco DbSets, migration e conexão `.\SQLEXPRESS`. |
| Validação e erros | DataAnnotations, validação de FKs, CPF/e-mail/placa únicos, datas, quilometragens, status e conflitos de reserva; respostas 400, 404 e 409. |
| 5 filtros e 2 tipos de JOIN | Sete filtros próprios, com INNER JOIN e LEFT JOIN explícitos em LINQ. |

## Ordem rápida de cadastro no Swagger

Cadastre primeiro Fabricante, depois Cliente, Veículo, Reserva e Aluguel. Anote os IDs retornados em cada POST.

`POST /api/Fabricantes`

```json
{ "nome": "Volkswagen", "paisOrigem": "Alemanha" }
```

`POST /api/Clientes`

```json
{ "nome": "Cliente de Teste", "cpf": "12345678901", "email": "cliente@example.com", "telefone": "31999999999" }
```

`POST /api/Veiculos`

```json
{ "modelo": "Polo", "placa": "ABC1D23", "anoFabricacao": 2024, "quilometragem": 5000, "disponivel": true, "fabricanteId": 1 }
```

`POST /api/Reservas`

```json
{ "clienteId": 1, "veiculoId": 1, "dataReserva": "2026-09-26T12:00:00", "dataInicio": "2026-10-01T10:00:00", "dataFim": "2026-10-03T10:00:00", "status": "Pendente" }
```

Status permitidos: `Pendente`, `Confirmada` e `Cancelada`. Reservas não canceladas do mesmo veículo não podem possuir períodos sobrepostos.

`POST /api/Alugueis`

```json
{ "clienteId": 1, "veiculoId": 1, "dataInicio": "2026-09-26T10:00:00", "dataFimPrevista": "2026-09-28T10:00:00", "dataDevolucao": null, "quilometragemInicial": 5000, "quilometragemFinal": null, "valorDiaria": 120, "valorTotal": 240 }
```

## Sete filtros — todos GET

- `/api/Filtros/veiculos-por-fabricante/1` — INNER JOIN.
- `/api/Filtros/reservas-por-cliente/1` — INNER JOIN entre Reserva, Cliente e Veículo.
- `/api/Filtros/reservas-por-status?status=pendente` — INNER JOIN.
- `/api/Filtros/reservas-por-periodo?inicio=2026-10-01&fim=2026-10-03` — INNER JOIN.
- `/api/Filtros/alugueis-em-aberto` — INNER JOIN.
- `/api/Filtros/alugueis-por-cliente/1` — INNER JOIN.
- `/api/Filtros/veiculos-com-reservas` — LEFT JOIN; inclui veículos sem reserva.

## Verificação realizada

- Compilação Release concluída com zero erros.
- Migration conferida com as cinco tabelas, chaves estrangeiras e índices únicos.
- 89 verificações HTTP e de dados aprovadas em banco de teste: CRUD, validações, conflitos, status HTTP e os sete filtros.
- A execução final com SQL Express deve ser confirmada no Visual Studio do aluno.

Após validar, atualize o repositório pessoal do Erick e envie ao Canvas o ZIP baixado desse repositório.

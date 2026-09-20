# Modelo conceitual - Locadora de Veiculos Erick

O sistema possui cinco entidades. `Reserva` e a entidade individual escolhida para este projeto.

```mermaid
erDiagram
    FABRICANTE ||--o{ VEICULO : fabrica
    CLIENTE ||--o{ ALUGUEL : realiza
    VEICULO ||--o{ ALUGUEL : participa
    CLIENTE ||--o{ RESERVA : solicita
    VEICULO ||--o{ RESERVA : recebe

    FABRICANTE {
        int FabricanteId PK
        string Nome
        string PaisOrigem
    }

    VEICULO {
        int VeiculoId PK
        string Modelo
        string Placa UK
        int AnoFabricacao
        int Quilometragem
        bool Disponivel
        int FabricanteId FK
    }

    CLIENTE {
        int ClienteId PK
        string Nome
        string CPF UK
        string Email UK
        string Telefone
    }

    ALUGUEL {
        int AluguelId PK
        int ClienteId FK
        int VeiculoId FK
        datetime DataInicio
        datetime DataFimPrevista
        datetime DataDevolucao
        int QuilometragemInicial
        int QuilometragemFinal
        decimal ValorDiaria
        decimal ValorTotal
    }

    RESERVA {
        int ReservaId PK
        int ClienteId FK
        int VeiculoId FK
        datetime DataReserva
        datetime DataInicio
        datetime DataFim
        string Status
    }
```

## Relacionamentos

- Um fabricante pode possuir varios veiculos; todo veiculo pertence a um fabricante.
- Um cliente pode realizar varios alugueis; todo aluguel pertence a um cliente.
- Um veiculo pode participar de varios alugueis em periodos diferentes; todo aluguel pertence a um veiculo.
- Um cliente pode fazer varias reservas; toda reserva pertence a um cliente.
- Um veiculo pode receber varias reservas em periodos diferentes; toda reserva pertence a um veiculo.

## Restricoes de integridade

- As cinco entidades possuem chave primaria explicita.
- `FabricanteId`, `ClienteId` e `VeiculoId` sao chaves estrangeiras explicitas.
- Placa, CPF e e-mail sao unicos.
- Campos essenciais utilizam `Required` e limites de tamanho.
- Exclusoes relacionadas usam comportamento restrito para preservar os registros de alugueis e reservas.

# ⚙️ Diretrizes do TheKingInYellow.Core

O projeto foi construído separando estritamente as regras de negócio (Backend) da camada de apresentação (Godot). Toda a inteligência reside no assembly `TheKingInYellow.Core` e é coberta por testes no `TheKingInYellow.Core.Tests`[cite: 1].

## 🛡️ Fail Soft e Zero Exceptions no Game Loop
Para evitar travamentos abruptos do Garbage Collector e *crashes* durante o gameplay, adotamos as seguintes regras:
- **Proibido lançar exceções de fluxo:** Regras de negócio não usam `throw new Exception()`.
- **Result Pattern:** Operações como leitura de I/O (`SaveEngine`), alocação de ocupações e checagem de perícias retornam um `Result` ou `Result<T>`.
- **Prevenção vs Punição:** Usamos `Debug.Assert` para alertar o desenvolvedor sobre estados inválidos (Fail Hard em Debug), mas adotamos valores de *fallback* e retornos graciosos para manter o jogo rodando em Release (Fail Soft).

## 🎲 Injeção de Determinismo (Testes)
A aleatoriedade dos dados de d100 e d6 é controlada por uma interface central.
- **Produção:** Usa o `DiceRoller` padrão do Core para gerar a entropia real[cite: 1].
- **Testes Unitários:** Usa uma implementação mockada (`FixedDiceRoller`) baseada na interface `IDiceRoller` para garantir que testes de *Skill Checks* não falhem por aleatoriedade[cite: 1].

## 💾 Persistência e Dados Estáticos
Não utilizamos banco de dados SQL. Toda a configuração estática é consumida de arquivos JSON mapeados e validados, localizados no diretório `/data` (ex: `occupations.schema.json`, `occupations.json` e `skills.json`)[cite: 1].
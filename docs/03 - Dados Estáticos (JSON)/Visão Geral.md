# 📂 Visão Geral: Arquitetura Data-Driven

No *The King in Yellow*, nós não engessamos conteúdo dentro do código C# (Hardcode) e nem utilizamos bancos de dados relacionais (SQL). Toda a configuração de balanceamento, regras de ficha e itens do jogo vive em arquivos `.json` estáticos.

## Onde vivem os dados?
Todos os arquivos estáticos de definição ficam localizados no diretório `/data` do repositório base[cite: 1].

## Como o Core consome isso?
O Godot não lê esses arquivos diretamente. O carregamento é responsabilidade do `SaveEngine` (no `TheKingInYellow.Core`), que carrega e valida o JSON, garantindo que o Game Loop nunca quebre caso um arquivo seja modificado incorretamente (diretriz de *Fail Soft*).

## Index de Arquivos
- [[Perícias e Ocupações]] (`skills.json`, `occupations.json`)
- [[Tomos e Arsenal]] (`tomes.json`, `weapons.json`, `items.json`)
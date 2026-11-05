# 🧠 Perícias e Ocupações

Estes são os arquivos centrais para a criação da ficha do Investigador, localizados na raiz da pasta `/data`[cite: 1].

## skills.json
Contém a lista de todas as habilidades que um investigador pode aprender no jogo[cite: 1].
- **BaseValue:** O valor mínimo que a perícia possui por padrão (ex: *Brawl* começa com 25%).
- **Group:** Utilizado para agrupar perícias semelhantes (ex: `interpersonal`, `science`). Útil para resolver *slots* de escolha curinga nas ocupações.

## occupations.json
Mapeia as profissões disponíveis para o jogador no início da campanha[cite: 1].
- **SkillPointsMultiplier:** Define como os pontos de ocupação são calculados (geralmente multiplicando a EDU ou EDU + algum atributo-chave).
- **AvailableSkills:** Lista os identificadores (IDs) das perícias que a ocupação fornece.
- **Tokens de Escolha Dinâmica:** Suporta a mecânica de curingas. Por exemplo, `CHOICE_INTERPERSONAL` ou `ANY` permite ao jogador abrir um *Dropdown* e escolher uma perícia correspondente, processado pelo `OccupationSlotResolver`.

## Validação (Schema)
Para evitar erros de digitação e quebras de estrutura, utilizamos o `occupations.schema.json`[cite: 1]. Ele garante que o arquivo de ocupações sempre siga o contrato exigido pelo `TheKingInYellow.Core`.
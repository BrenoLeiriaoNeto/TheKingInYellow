# 📖 Tomos e 🔫 Arsenal (Em Desenvolvimento)

Arquivos planejados para mapear o impacto físico e psicológico no investigador. 

## tomes.json (Conhecimento Esotérico)
Mapeia os livros proibidos que o investigador pode encontrar e estudar. Estudar estes artefatos interage diretamente com o método `ReadForbiddenBook` na entidade `Investigator`.
- **LanguageSkillRequired:** Perícia de idioma necessária para traduzir o livro (ex: `language_latin`).
- **StudyWeeksRequired:** Tempo consumido no jogo para absorver o conteúdo.
- **LucidityLoss:** Quantidade de Sanidade (Lucidez) drenada da mente do investigador após a leitura.
- **LoreGain:** Pontos adicionados ao *Forbidden Lore* (Conhecimento Esotérico).

## weapons.json (Armas e Dano)
Alimenta o sistema de combate do jogo, cruzando informações com o `SkillCheckEngine`.
- **DamageDice:** A notação de dano da arma (ex: `1d10`, `1d4+DB`).
- **RequiredSkillId:** A perícia testada no momento do ataque (ex: `firearm_handgun`).
- **MalfunctionRating:** O valor no `d100` a partir do qual a arma falha ou trava.

## items.json (Inventário)
Guarda artefatos mundanos (chaves, lanternas, diários) que vão alimentar a lista `InventoryItemIds` da ficha do personagem.
Restaurante Galáctico

Restaurante Galáctico é um jogo 2D de puzzle e ação em turnos, desenvolvido na Unity com C# para o Projeto Integrador do curso de Jogos Digitais da Faculdade Méliès.

O jogador precisa preparar pizzas e completar entregas enquanto atravessa fases com inimigos e obstáculos. A movimentação acontece em uma grade, e cada ação faz o turno avançar. Por isso, escolher o próximo movimento é parte central do desafio.

O projeto está em desenvolvimento.

Como funciona

O jogador se move uma célula por vez. Também pode usar um dash com cargas limitadas para atravessar várias células em um único turno. Depois de uma ação do jogador, os inimigos agem em sequência.

Os inimigos têm comportamentos diferentes. Alguns perseguem o jogador, outros se movem de maneira aleatória ou avançam em uma direção. O cenário também interfere nas decisões, com elementos como gelo, que provoca deslizamento, e minas, que são ativadas ao serem pisadas.

Para concluir uma fase, o jogador precisa preparar as pizzas e entregá-las na sacola de entrega sem ser derrotado.

Desenvolvimento

A programação foi dividida em sistemas de movimentação, gerenciamento de turnos, comportamento dos inimigos e controle da partida.

Uma decisão importante foi separar a posição lógica das entidades na grade da animação do movimento. A célula ocupada é atualizada antes do deslocamento visual, o que ajuda a manter o funcionamento dos turnos previsível.

O EnemyTurnManager coordena as ações dos inimigos por meio da interface IEnemyTurn. Assim, inimigos com comportamentos diferentes podem participar do mesmo sistema de turnos. Um dos inimigos utiliza busca em largura (BFS) para encontrar um caminho até o jogador.

O projeto também usa eventos para comunicar ações do jogador a outros sistemas sem concentrar toda a lógica em um único script.

Entre os principais scripts estão GridManager, GridEntity, TurnManager, EnemyTurnManager, NewPlayerController e GameManager.

Tecnologias

Unity 2D, C#, Unity Input System, Grid/Tilemap e Git/GitHub.

Como executar

Abra o projeto pelo Unity Hub com uma versão compatível da Unity. Depois, abra a cena principal em Assets/Scenes e execute pelo Editor.

Sobre o projeto

Restaurante Galáctico é um trabalho acadêmico desenvolvido em equipe na Faculdade Méliès

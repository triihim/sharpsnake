using System;
using Snake.Common;
using Snake.Core;

namespace Snake
{
    internal class ConsoleSnake
    {
        private Game game;
        private Renderer renderer;

        public ConsoleSnake(GameConfiguration configuration)
        {
            game = new Game(configuration);
            renderer = new Renderer(game);
            Initialize();
        }

        public void Start()
        {
            game.StartGameLoop();
        }

        private void Initialize()
        {
            game.OnUpdate += Update;
            game.OnGameOver += GameOver;
            game.Initialize();
        }

        private void Update()
        {
            if(game.IsRunning())
            {
                renderer.RenderGameScreen();
            }
        }
        
        private void GameOver()
        {
            renderer.RenderGameOverScreen();
            Restart();
        }

        private void Restart()
        {
            renderer.ClearScreen();
            game.Reset();
            game.StartGameLoop();
        }

    }
}

using System;

namespace MyGame
{
    public class GameManager
    {
        private static GameManager instance = null;

        // Estado del juego
        public enum GameState
        {
            Menu,
            Playing,
            Victory,
            Defeat
        }

        private GameState currentState = GameState.Menu;

        // Atributos globales
        private int lives = 3;
        private int score = 0;

        // Propiedades públicas (si se necesitan acceder desde afuera)
        public int Lives { get => lives; set => lives = value; }
        public int Score { get => score; set => score = value; }
        public GameState CurrentState { get => currentState; set => currentState = value; }

        // Constructor privado para que no se pueda instanciar desde fuera
        private GameManager()
        {
        }

        // Método para acceder a la única instancia
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameManager();
                return instance;
            }
        }

        // Reiniciar el juego (opcional)
        public void ResetGame()
        {
            lives = 3;
            score = 0;
            currentState = GameState.Menu;
        }

    }
}

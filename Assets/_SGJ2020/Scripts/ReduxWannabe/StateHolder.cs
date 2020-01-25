namespace _SGJ2020.Scripts
{
    public class StateHolder
    {
        private static GameState _gameState = new GameState();

        public static GameState State => _gameState;
    }
}
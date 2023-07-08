public interface IGameStateManager {
    void StartState();
    void FinishState(GameStates nextGameState);
}

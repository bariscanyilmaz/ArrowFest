using UnityEngine;
using UnityEngine.Events;


public enum GameState { Play, FinishLine, Win, Lose, Wait }
public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Level _level;

    private GameState _gameState;

    public GameState GameState => _gameState;

    private Level _currentLevel;

    public Level GetLevel() => _currentLevel;

    [SerializeField] FollowPath _follower;

    void Start()
    {
        _gameState = GameState.Wait;
        _currentLevel = Instantiate(_level);
        _follower.SetPathCreator(_currentLevel.GetPathCreator());

        //_currentLevel.GetPathCreator().TriggerPathUpdate();
    }

    public void SetState(GameState state) => _gameState = state;

    public UnityEvent StartGame, GameOver, Restart;

    public void OnGameReStart()
    {
        Destroy(_currentLevel.gameObject);
        _currentLevel = Instantiate(_level);
    }

}

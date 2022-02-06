using UnityEngine;
using UnityEngine.Events;


public enum GameState { Play, FinishLine, Win, Lose, Wait }
public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Level _level;
    [SerializeField]
    private FollowPath _follower;

    private int _cointCount;
    private int _collectedCoin;

    private Level _currentLevel;
    private GameState _gameState;
    
    public GameState GameState => _gameState;

    public Level GetLevel() => _currentLevel;

    public int CoinCount => _cointCount;
    public int CollectedCoinCount => _collectedCoin;


    void Start()
    {
        _gameState = GameState.Wait;
        _currentLevel = Instantiate(_level);
        _follower.SetPathCreator(_currentLevel.GetPathCreator());
        //_currentLevel.GetPathCreator().TriggerPathUpdate();
    }

    public void SetState(GameState state) => _gameState = state;

    public UnityEvent StartGame, GameOver, Restart, Win, FinishLine;

    public void OnGameReStart()
    {
        Destroy(_currentLevel.gameObject);
        _currentLevel = Instantiate(_level);
    }


    public void IncreaseCoin() => _cointCount++;
    public void IncreaseCollectedCoin() => _collectedCoin++;

    public void AddCoins() => _cointCount += _collectedCoin;


}

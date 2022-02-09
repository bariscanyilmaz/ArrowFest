using UnityEngine;
using UnityEngine.Events;


public enum GameState { Play, FinishLine, Win, Lose, Wait }
public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private Level[] _levels;

    [SerializeField]
    private FollowPath _follower;

    public Level CurrentLevel { get; private set; }

    public int CurrentLevelIndex { get; private set; }

    public GameState GameState { get; private set; }

    public int CoinCount { get; private set; }
    public int CollectedCoinCount { get; private set; }

    public UnityEvent StartGame, GameOver, Restart, Win, FinishLine;

    void Start()
    {
        CurrentLevelIndex = PlayerPrefs.GetInt("Level", 0);
        CoinCount = PlayerPrefs.GetInt("Coin", 0);
        CollectedCoinCount = 0;
        GameState = GameState.Wait;
        LoadLevel();
        UIManager.Instance.UpdateLevelText();
        UIManager.Instance.UpdateCoinText();
        _follower.SetPathCreator(CurrentLevel.GetPathCreator());

    }

    public void SetState(GameState state) => GameState = state;



    public void OnGameReStart()
    {
        LoadLevel();
        CollectedCoinCount = 0;
        PlayerPrefs.SetInt("Coin", CoinCount);
        UIManager.Instance.UpdateLevelText();
        UIManager.Instance.UpdateCoinText();
        _follower.SetPathCreator(CurrentLevel.GetPathCreator());

    }


    public void IncreaseCoin() => CoinCount++;
    public void IncreaseCollectedCoin() => CollectedCoinCount++;

    public void AddCoins() => CoinCount += CollectedCoinCount;

    public void LoadNextLevel()
    {
        CollectedCoinCount = 0;
        ++CurrentLevelIndex;
        PlayerPrefs.SetInt("Level", CurrentLevelIndex);
        PlayerPrefs.SetInt("Coin", CoinCount);
        LoadLevel();
        UIManager.Instance.UpdateLevelText();
        UIManager.Instance.UpdateCoinText();
        _follower.SetPathCreator(CurrentLevel.GetPathCreator());
    }


    void LoadLevel()
    {
        if (CurrentLevel != null)
        {
            Destroy(CurrentLevel.gameObject);
        }
        CurrentLevel = Instantiate(_levels[CurrentLevelIndex % _levels.Length]);
    }
}

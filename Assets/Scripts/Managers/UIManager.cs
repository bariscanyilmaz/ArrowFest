using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    GameObject _startPanel, _gameOverPanel, _winPanel;

    [SerializeField]
    TextMeshProUGUI _collectedCoinText, _coinText, _levelText;

    [SerializeField]
    Button _button;

    [SerializeField]
    TextMeshProUGUI _buttonText;

    void Start()
    {
        _startPanel.SetActive(true);
        _button.onClick.AddListener(() =>
        {
            if (GameManager.Instance.GameState == GameState.Lose)
            {
                _gameOverPanel.SetActive(false);
                GameManager.Instance.Restart.Invoke();
                GameManager.Instance.SetState(GameState.Play);
            }

            if (GameManager.Instance.GameState == GameState.Win)
            {

                GameManager.Instance.AddCoins();
                UpdateCoinText();
                GameManager.Instance.LoadNextLevel();
                UpdateLevelText();
                _winPanel.SetActive(false);
                GameManager.Instance.StartGame.Invoke();

                IEnumerator Do()
                {
                    float elapsedTime = 0;
                    float waitForOneSecond = 1f;
                    while (elapsedTime < waitForOneSecond)
                    {
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }

                    GameManager.Instance.SetState(GameState.Play);
                }
                StartCoroutine(Do());

            }
            _button.gameObject.SetActive(false);
        });
    }
    public void OnWin()
    {
        _buttonText.text = "Claim";
        _collectedCoinText.text = GameManager.Instance.CollectedCoinCount.ToString();
        _button.gameObject.SetActive(true);
        _winPanel.SetActive(true);

    }
    public void OnGameOver()
    {
        _buttonText.text = "Restart";
        GameManager.Instance.SetState(GameState.Lose);
        _button.gameObject.SetActive(true);
        _gameOverPanel.SetActive(true);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.Instance.GameState == GameState.Wait)
            {
                _startPanel.SetActive(false);
                GameManager.Instance.StartGame.Invoke();
                GameManager.Instance.SetState(GameState.Play);
            }


        }
    }

    public void UpdateCoinText() => _coinText.text = GameManager.Instance.CoinCount.ToString();

    public void UpdateLevelText() => _levelText.text = $"Level {GameManager.Instance.CurrentLevelIndex + 1}";

    void StartGame()
    {
        GameManager.Instance.SetState(GameState.Play);
    }

}

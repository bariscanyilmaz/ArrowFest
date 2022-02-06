using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    GameObject _startPanel, _gameOverPanel,_winPanel;

    [SerializeField]
    TextMeshProUGUI _collectedCoinText,_coinText;


    public void StartButtonOnClick()
    {

    }
    public void RestartButtonOnClick()
    {

        //GameManager.Instance.Restart.Invoke();
    }

    public void OnWin()
    {
        _collectedCoinText.text=GameManager.Instance.CollectedCoinCount.ToString();
        _winPanel.SetActive(true);

    }
    public void OnGameOver()
    {
        GameManager.Instance.SetState(GameState.Lose);
        _gameOverPanel.SetActive(true);
    }

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.Wait)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPanel.SetActive(false);
                GameManager.Instance.StartGame.Invoke();
                GameManager.Instance.SetState(GameState.Play);
            }
        }

        if (GameManager.Instance.GameState == GameState.Lose)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _gameOverPanel.SetActive(false);
                GameManager.Instance.Restart.Invoke();
                GameManager.Instance.SetState(GameState.Play);
            }
        }

        if (GameManager.Instance.GameState == GameState.Win)
        {
            if (Input.GetMouseButtonDown(0))
            {

                //increase coin count
                GameManager.Instance.AddCoins();
                UpdateCoinText();
                //load next level
                //start game;
                _winPanel.SetActive(false);
                //
                //
            }
        }
    }

    public void UpdateCoinText()=>_coinText.text=GameManager.Instance.CoinCount.ToString();

}

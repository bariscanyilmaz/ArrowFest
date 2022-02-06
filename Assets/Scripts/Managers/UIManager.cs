using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    GameObject _startPanel, _gameOverPanel,_winPanel;


    public void StartButtonOnClick()
    {

    }
    public void RestartButtonOnClick()
    {

        //GameManager.Instance.Restart.Invoke();
    }

    public void OnWin()
    {
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
                _winPanel.SetActive(false);
                //load new level
                //GameManager.Instance.Restart.Invoke();
                //GameManager.Instance.SetState(GameState.Play);
            }
        }
    }

}

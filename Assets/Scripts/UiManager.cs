using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public static UiManager UiManagerInstance { get; private set; }

    [SerializeField] private TextMeshProUGUI moveCountTXT;
    [SerializeField] private TextMeshProUGUI timerTXT;
    [SerializeField] private TextMeshProUGUI rightTileTXT;
    [SerializeField] private TextMeshProUGUI lastTimeTXT;
    [SerializeField] private TextMeshProUGUI lastMoveTXT;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject finishPanel;
    [SerializeField] private Database dataB;

    private float elapsedTime;
    private int moveCount = 0;
    private int rightTileCount = 0;
    private int totalTileCount = 15;
    private int minutes;
    private int seconds;

    private void Awake()
    {
        if (UiManagerInstance != null && UiManagerInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            UiManagerInstance = this;
        }
    }

    private void Update()
    {
        UpdateTimer();
    }

    // Update moves and set on Ui
    public void UpdateMoveCount()
    {
        moveCount++;
        moveCountTXT.text = moveCount.ToString();
    }

    // Update moves and set on Ui
    public void ValidateTilePosition(bool isRight)
    {
        if (isRight)
        {
            rightTileCount++;
        }
        else
        {
            rightTileCount--;
        }

        rightTileTXT.text = rightTileCount.ToString();

        if (rightTileCount == totalTileCount)
        {
            FinishGame();
        }
    }
    void FinishGame()
    {
        gamePanel.SetActive(false);
        dataB.ClearTiles();
        finishPanel.SetActive(true);
        GetLastInfos();
    }
    public void NewGame()
    {
        dataB.Clear();
        SceneManager.LoadScene(0);
    }

    public void GetLastInfos()
    {
        lastMoveTXT.text = moveCount.ToString();
        lastTimeTXT.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Update time and set on Ui
    private void UpdateTimer()
    {
        elapsedTime += Time.deltaTime;

        minutes = Mathf.FloorToInt(elapsedTime / 60F);
        seconds = Mathf.FloorToInt(elapsedTime % 60F);

        timerTXT.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

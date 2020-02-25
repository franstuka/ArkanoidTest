using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MySceneManager))]
public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        sceneManager = GetComponent<MySceneManager>();
        scoreManager = new ScoreManager();
    }

    #endregion

    public const int MAX_HIGHSCORES = 10;
    public const int MAX_LEVELS = 3;
    [SerializeField] private int actualLevel; //I left this variable modificable to set the level that we want to test in unity interface
    private int playerHP;
    private LinkedList<HighScore> highScoreList = new LinkedList<HighScore>();
    private MySceneManager sceneManager;
    private LevelManager levelManager;
    private ScoreManager scoreManager;

    private void Start()
    {
        LoadData();
        RestartGame();
    }

    /// <summary>
    /// Restart all game variables and goes to the main menu.
    /// </summary>
    public void RestartGame()
    {
        playerHP = 3;
        actualLevel = 0;
        scoreManager.ResetScore();
        sceneManager.ChangeScene(actualLevel);
    }

    /// <summary>
    /// Save the highscores.
    /// </summary>
    public void SaveData()
    {
        SaveDataManager.SaveData(highScoreList);
    }

    /// <summary>
    /// Load the highScores.
    /// </summary>
    private void LoadData()
    {
        highScoreList = SaveDataManager.LoadData();
    }

    /// <summary>
    /// Go to the next level.
    /// </summary>
    public void NextLevel()
    {
        actualLevel++;
        sceneManager.ChangeScene(actualLevel);
    }

    /// <summary>
    /// Get player's health.
    /// </summary>
    /// <returns></returns>
    public int GetPlayerHP()
    {
        return playerHP;
    }

    /// <summary>
    /// Reduce the playerHP by one.
    /// </summary>
    public void OnLoseHP()
    {
        playerHP--;
    }

    /// <summary>
    /// Get the reference to the score manager.
    /// </summary>
    /// <returns></returns>
    public ScoreManager GetScoreManager()
    {
        return scoreManager;
    }

    /// <summary>
    /// Get the reference to the level manager.
    /// </summary>
    /// <returns></returns>
    public LevelManager GetLevelManager()
    {
        return levelManager;
    }

    /// <summary>
    /// Sets the reference to the level manager.
    /// </summary>
    /// <returns></returns>
    public void SetLevelManager(LevelManager levelManager)
    {
        this.levelManager = levelManager;
    }

    /// <summary>
    /// Returns if is the last level.
    /// </summary>
    /// <returns></returns>
    public bool IsTheLastLevel()
    {
        if(actualLevel < MAX_LEVELS)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Test if player archieves a highscore.
    /// </summary>
    /// <returns></returns>
    public bool TestNewHighscore()
    {
        if(highScoreList.Count < MAX_HIGHSCORES || scoreManager.GetScore() > highScoreList.Last.Value.GetScore())
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Add a new highscore in the highscores list.
    /// </summary>
    /// <param name="name"></param>
    public void AddNewHighScore(string name)
    {
        LinkedListNode<HighScore> searchNode = highScoreList.First;
        HighScore newHighScore = new HighScore(name, scoreManager.GetScore());

        bool end = false;

        //insert the highScore in order.
        while(!end)
        {
            if(searchNode == null)
            {
                highScoreList.AddLast(newHighScore);
                end = true;
            }
            else
            {
                if(newHighScore.GetScore() > searchNode.Value.GetScore())
                {
                    highScoreList.AddBefore(searchNode, newHighScore);
                    end = true;
                }
                else
                {
                    searchNode = searchNode.Next;
                }
            }
        }

        //doing this we can save more highscores than admited, so if we inserted a new one
        //when we have the max number, we remove the last
        if(highScoreList.Count > MAX_HIGHSCORES)
        {
            highScoreList.RemoveLast();
        }

        SaveData();
    }

    /// <summary>
    /// Gets all the highscores.
    /// </summary>
    /// <returns></returns>
    public LinkedList<HighScore> GetHighScores()
    {
        return highScoreList;
    }

    /// <summary>
    /// Orders exit game.
    /// </summary>
    public void ExitGame()
    {
        sceneManager.ExitGame();
    }
}

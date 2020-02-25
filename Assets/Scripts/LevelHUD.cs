using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHUD : MonoBehaviour
{
    public enum Screens { GAME_SCREEN, END_LEVEL, NEW_HIGHSCORE, SHOW_HIGHSCORES, ENTER_LEVEL, MAIN_MENU };

    public const int MAX_NAME_CHARACTERS = 10;
    private const string DEFAULT_NAME = "Unknown";

    //active screen
    [SerializeField] private Screens activeScreen = Screens.ENTER_LEVEL;

    //countdown text
    [SerializeField] private TMPro.TextMeshProUGUI countDownText;

    //Game information on screen
    [SerializeField] private List<GameObject> playerHP;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private ActiveCapsuleHUD[] activeCapsuleHUDs;

    //enter new HighScores screen
    [SerializeField] private GameObject enterNewHighScoreScreen;
    [SerializeField] private TMPro.TextMeshProUGUI enterNewHighScorePlayerText;

    //highScores Screen
    [SerializeField] private GameObject highScoresScreen;
    [SerializeField] private TMPro.TextMeshProUGUI[] highScoresTexts;

    //nextLevel screen
    [SerializeField] private GameObject nextLevelScreen;

    //main menu stuff to hide when show highscores
    [SerializeField] private GameObject menuScreen;

    private void Start()
    {

    }

    void Update()
    {
        UpdateActiveCapsulesHUD();

        if(activeScreen == Screens.NEW_HIGHSCORE)
        {
            OnEnterPlayerNameChar();
        }
    }

    /// <summary>
    /// Gets the player key pulsation on enter new highscore name.
    /// </summary>
    private void OnEnterPlayerNameChar()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //filter text
                if (enterNewHighScorePlayerText.text == "")
                {
                    GameManager.instance.AddNewHighScore(DEFAULT_NAME);
                }
                else if (enterNewHighScorePlayerText.text.Length > MAX_NAME_CHARACTERS)
                {
                    GameManager.instance.AddNewHighScore(enterNewHighScorePlayerText.text.Substring(0, 10));
                }
                else
                {
                    GameManager.instance.AddNewHighScore(enterNewHighScorePlayerText.text);
                }
                OnShowHighScores();

            }
            else if (enterNewHighScorePlayerText.text.Length > 0 && (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Delete)))
            {
                enterNewHighScorePlayerText.text = enterNewHighScorePlayerText.text.Substring(0, enterNewHighScorePlayerText.text.Length - 1);
            }
            else if (enterNewHighScorePlayerText.text.Length < 10)
                enterNewHighScorePlayerText.text += Input.inputString;
        }
    }

    /// <summary>
    /// Refresh the actives capsules HUD timers.
    /// </summary>
    private void UpdateActiveCapsulesHUD()
    {
        for (int i = 0; i < activeCapsuleHUDs.Length; i++)
        {
            if(activeCapsuleHUDs[i].gameObject.activeInHierarchy)
            {
                activeCapsuleHUDs[i].UpdateTimers();
            }
        }
    }

    /// <summary>
    /// Rest player HP on screen.
    /// </summary>
    /// <param name="hp"></param>
    public void SetHUDHP(int hp)
    {
        //we get GetPlayerHP() instead GetPlayerHP() -1 because the GameManager is updated before
        if (hp > 3)
        {
            Debug.LogError("HP is greater than 3");
        }
        else
        {
            for (int i = hp; i < 3; i++)
            {
                playerHP[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Actions to do when the level starts.
    /// </summary>
    public void OnStartLevel()
    {
        ChangeScreens(Screens.GAME_SCREEN);
    }

    /// <summary>
    /// Actions to do when level loads.
    /// </summary>
    public void OnLoadLevel()
    {
        HideAllActivesCapsules();
        SetHUDHP(GameManager.instance.GetPlayerHP());
        UpdateScoreHUD(GameManager.instance.GetScoreManager().GetScore());
        ChangeScreens(Screens.ENTER_LEVEL);
    }

    /// <summary>
    /// When ends a level and it isn't the last.
    /// </summary>
    public void OnWinLevel()
    {
        ChangeScreens(Screens.END_LEVEL);
    }

    /// <summary>
    /// When player gets a new highscore.
    /// </summary>
    public void OnNewHighScore()
    {
        ChangeScreens(Screens.NEW_HIGHSCORE);
    }

    /// <summary>
    /// Show the highscores
    /// </summary>
    public void OnShowHighScores()
    {
        SetHighscoreText();
        ChangeScreens(Screens.SHOW_HIGHSCORES);
    }

    /// <summary>
    /// Setup the fields to see the highscores correctly.
    /// </summary>
    private void SetHighscoreText()
    {
        LinkedList<HighScore> highScores = GameManager.instance.GetHighScores();
        LinkedListNode<HighScore> searchNode = highScores.First;
        int i = 0;

        for (; searchNode != null; i++, searchNode = searchNode.Next)
        {
            highScoresTexts[i].text = searchNode.Value.GetName() + "\t" + searchNode.Value.GetScore().ToString();
        }
        for (; i < GameManager.MAX_HIGHSCORES; i++)
        {
            highScoresTexts[i].text = "";
        }
    }

    /// <summary>
    /// Go to the main main, ending the game.
    /// </summary>
    public void GoBackOnMainMenu()
    {
        GameManager.instance.RestartGame();
    }

    /// <summary>
    /// Return to main menu when show the highscores in the main menu.
    /// </summary>
    public void MainMenuReturnToMainMenu()
    {
        ChangeScreens(Screens.MAIN_MENU);
    }

    /// <summary>
    /// Updates the countdown text.
    /// </summary>
    /// <param name="text"></param>
    public void UpdateStartCountdownText(string text)
    {
        countDownText.text = text;
    }

    /// <summary>
    /// Updates the score HUD
    /// </summary>
    /// <param name="score"></param>
    public void UpdateScoreHUD(int score)
    {
        scoreText.text = score.ToString();
    }

    /// <summary>
    /// The HUD loses the reference to the gamemanager, so with this funcion we garantee that.
    /// </summary>
    public void OnNextLevelClick()
    {
        GameManager.instance.NextLevel();
    }

    /// <summary>
    /// The HUD loses the reference to the gamemanager, so with this funcion we garantee that.
    /// </summary>
    public void OnExitGameClick()
    {
        GameManager.instance.ExitGame();
    }

    /// <summary>
    /// Add new active capsule to the HUD.
    /// </summary>
    /// <param name="capsule"></param>
    public void AddNewActiveCapsule(Capsule capsule)
    {
        bool end = false;
        for (int i = 0; i < activeCapsuleHUDs.Length && !end; i++)
        {
            if(!activeCapsuleHUDs[i].gameObject.activeInHierarchy)
            {
                activeCapsuleHUDs[i].SetCapsuleRefference(capsule);
                activeCapsuleHUDs[i].gameObject.SetActive(true);
                end = true;
            }
        }  
    }

    /// <summary>
    /// Remove an active capsule from the HUD.
    /// </summary>
    /// <param name="capsule"></param>
    public void RemoveActiveCapsule(Capsule capsule)
    {
        bool end = false;
        for (int i = 0; i < activeCapsuleHUDs.Length && !end; i++)
        {
            if (activeCapsuleHUDs[i].gameObject.activeInHierarchy)
            {
                if(activeCapsuleHUDs[i].GetCapsuleReff() == capsule)
                {
                    activeCapsuleHUDs[i].gameObject.SetActive(false);
                    end = true;
                }
            }
        }
    }

    /// <summary>
    /// Hide all active capsules from the HUD.
    /// </summary>
    public void HideAllActivesCapsules()
    {
        for (int i = 0; i < activeCapsuleHUDs.Length; i++)
        {
            activeCapsuleHUDs[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Do all the stuff needed when changing screens.
    /// </summary>
    /// <param name="screen"></param>
    private void ChangeScreens(Screens screen)
    {
        switch(screen)
        {
            case Screens.ENTER_LEVEL:
                {
                    break;
                }
            case Screens.GAME_SCREEN:
                {
                    break;
                }
            case Screens.END_LEVEL:
                {
                    nextLevelScreen.SetActive(true);
                    break;
                }
            case Screens.NEW_HIGHSCORE:
                {
                    enterNewHighScoreScreen.SetActive(true);
                    break;
                }
            case Screens.SHOW_HIGHSCORES:
                {
                    if(activeScreen == Screens.NEW_HIGHSCORE)
                    {
                        enterNewHighScoreScreen.SetActive(false);
                    }
                    if (activeScreen == Screens.MAIN_MENU)
                    {
                        menuScreen.SetActive(false);
                    } 
                    highScoresScreen.SetActive(true);
                    break;
                }
            case Screens.MAIN_MENU:
                {
                    highScoresScreen.SetActive(false);
                    menuScreen.SetActive(true);
                    break;
                }
        }
        activeScreen = screen;
    }
}

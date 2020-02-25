using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int LevelBallBaseDamage = 1;
    public float LevelBallBaseSpeed = 2f;
    public float LevelSliderSpeed = 2f;
    public int winLevelScore = 500;
    
    [SerializeField] private bool spawnCapsulesEnabled = true;
    [SerializeField] private List<Brick> levelBricks = new List<Brick>();
    [SerializeField] private Slider slider;
    [SerializeField] private Ball ball;
    [SerializeField] private DropManager dropManager;
    [SerializeField] private ActiveCapsulesManager activeCapsules;
    [SerializeField] private LevelHUD levelHUD;   
    
    private List<Capsule> capsulesFalling = new List<Capsule>();
    private int bricksToDestroy = 0;

    private void Awake()
    {
        GameManager.instance.SetLevelManager(this);
    }

    private void Start()
    {
        
        OnLoadLevel();
    }

    /// <summary>
    /// Add bricks to the manager.
    /// </summary>
    /// <param name="brick"></param>
    public void AddBrick(Brick brick)
    {
        levelBricks.Add(brick);
        if(!brick.GetIfBrickIsUnbreakable())
        {
            bricksToDestroy++;
        }
    }

    /// <summary>
    /// Do the operations related with the elimination of one brick from level.
    /// </summary>
    /// <param name="brick">The brick to remove.</param>
    public void RemoveBrick(Brick brick)
    {
        //change the score
        ChangeScore(brick.GetScoreOnDestroy());

        //removes the brick
        bricksToDestroy--;
        levelBricks.Remove(brick);

        //roll spawn new capsule prob
        if(spawnCapsulesEnabled && bricksToDestroy > 0)
            dropManager.RollSpawnProb(new Vector2(brick.transform.position.x, brick.transform.position.y));

        //must test the end game condition
        if(TestWinGame())
        {
            OnWinLevel();
        }
    }

    /// <summary>
    /// Returns true if the game is winned.
    /// </summary>
    /// <returns></returns>
    private bool TestWinGame()
    {
        if(bricksToDestroy == 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Returns true if the game is lost.
    /// </summary>
    /// <returns></returns>
    private bool TestLoseGame()
    {
        if (GameManager.instance.GetPlayerHP() <= 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Reduce the player HP and order all the actions needed.
    /// </summary>
    public void OnLoseHP()
    {
        GameManager.instance.OnLoseHP();
        
        //must test the end game condition
        if(TestLoseGame())
        {
            OnLoseGame();
        }
        else
        {
            levelHUD.HideAllActivesCapsules();
            ClearAllCapsulesFalling();
            activeCapsules.EndAllEffects();
            ball.gameObject.SetActive(false);
            levelHUD.SetHUDHP(GameManager.instance.GetPlayerHP());
            StartCoroutine(StartGameCountDown());
        }
    }

    /// <summary>
    /// Orders the actions to do when a level is loaded.
    /// </summary>
    public void OnLoadLevel()
    {
        levelHUD.OnLoadLevel();
        StartCoroutine(StartGameCountDown());
    }

    /// <summary>
    /// Orders the actions to do when the level start and it's playable.
    /// </summary>
    public void OnStartLevel()
    {
        slider.SetPlayerHasControl(true);
        ball.ResetBall();
    }

    /// <summary>
    /// Orders the actions to do when the level is ended.
    /// </summary>
    public void OnWinLevel()
    {
        OnEndLevel();
        ChangeScore(winLevelScore);
        if(GameManager.instance.IsTheLastLevel())
        {
            TestHighScore();
        }   
        else
        {
            levelHUD.OnWinLevel();
        }
    }

    /// <summary>
    /// Actions to do when the game is lost.
    /// </summary>
    public void OnLoseGame()
    {
        OnEndLevel();
        TestHighScore();
    }

    /// <summary>
    /// Stops the game components.
    /// </summary>
    public void OnEndLevel()
    {
        slider.SetPlayerHasControl(false);
        ClearAllCapsulesFalling();
        activeCapsules.EndAllEffects();
        ball.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
    }

    /// <summary>
    /// Test if there is a new highscore and change the HUD.
    /// </summary>
    private void TestHighScore()
    {
        if (GameManager.instance.TestNewHighscore())
        {
            levelHUD.OnNewHighScore();
        }
        else
        {
            levelHUD.OnShowHighScores();
        }
    }

    /// <summary>
    /// Activates one new capsule.
    /// </summary>
    /// <param name="capsuleGameObject"></param>
    public void AddNewActiveCapsule(GameObject capsuleGameObject)
    {
        Capsule capsule = capsuleGameObject.GetComponent<Capsule>();
        bool inserted;

        if(capsule == null)
        {
            Debug.LogError("Capsule component don't found.");
        }
        else
        {
            //remove the capsule in capsuleFallingList
            RemoveCapsuleFalling(capsule);

            //try to insert in the active capsules list
            inserted = activeCapsules.AddCapsule(capsule);
          
            if (inserted)
            {
                //if it has been inserted, activate the effect
                capsule.AddEffect();
                //show it in the hud
                levelHUD.AddNewActiveCapsule(capsule);
            }
            else
            {
                //otherelse the effect was active, the duration has been updated
                //and the actual capsule must be destroyed
                Destroy(capsuleGameObject);
            }
        }
    }

    /// <summary>
    /// Remove one activeCapsule.
    /// </summary>
    /// <param name="capsule"></param>
    public void RemoveActiveCapsule(Capsule capsule)
    {
        levelHUD.RemoveActiveCapsule(capsule);
        capsule.RemoveEffect();
        Destroy(capsule.gameObject);
    }

    /// <summary>
    /// Add a new capsule falling to the list.
    /// </summary>
    /// <param name="capsule"></param>
    public void AddCapsuleFalling(Capsule capsule)
    {
        capsulesFalling.Add(capsule);
    }

    /// <summary>
    /// Removes a capsule falling from list.
    /// </summary>
    /// <param name="capsule"></param>
    public void RemoveCapsuleFalling(Capsule capsule)
    {
        capsulesFalling.Remove(capsule);
    }

    /// <summary>
    /// Clear and destroy all capsules falling.
    /// </summary>
    private void ClearAllCapsulesFalling()
    {
        GameObject removeObject = null;

        while (capsulesFalling.Count != 0)
        {
            removeObject = capsulesFalling[0].gameObject;
            capsulesFalling.RemoveAt(0);
            Destroy(removeObject);
        }
    }   

    /// <summary>
    /// Changes the score and updates the HUD.
    /// </summary>
    /// <param name="score"></param>
    public void ChangeScore(int score)
    {
        //add the score from brick
        GameManager.instance.GetScoreManager().AddScore(score);
        //updates the score in the HUD
        levelHUD.UpdateScoreHUD(GameManager.instance.GetScoreManager().GetScore());
    }

    /// <summary>
    /// Start game countdown.
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartGameCountDown()
    {
        levelHUD.UpdateStartCountdownText("3");
        yield return new  WaitForSeconds(1f);
        levelHUD.UpdateStartCountdownText("2");
        yield return new WaitForSeconds(1f);
        levelHUD.UpdateStartCountdownText("1");
        yield return new WaitForSeconds(1f);
        levelHUD.UpdateStartCountdownText("GO!");
        yield return new WaitForSeconds(1f);
        levelHUD.UpdateStartCountdownText("");
        OnStartLevel();
        levelHUD.OnStartLevel();
    }

    /// <summary>
    /// Gets a ball reference.
    /// </summary>
    /// <returns></returns>
    public Ball GetBall()
    {
        return ball;
    }

    /// <summary>
    /// Gets a slider reference.
    /// </summary>
    /// <returns></returns>
    public Slider GetSlider()
    {
        return slider;
    }

    /// <summary>
    /// Gets all the bricks in scene.
    /// </summary>
    /// <returns></returns>
    public List<Brick> GetBrickList()
    {
        return levelBricks;
    }
}

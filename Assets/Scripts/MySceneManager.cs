using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    /// <summary>
    /// Change to indicaded scenes. Scenes must be named "Level" + the number of the level.
    /// </summary>
    /// <param name="level"></param>
    public void ChangeScene(int level)
    {
        switch (level)
        {
            case 0:
                {
                    SceneManager.LoadScene("Level0");
                    break;
                }
            case 1:
                {
                    SceneManager.LoadScene("Level1");
                    break;
                }
            case 2:
                {
                    SceneManager.LoadScene("Level2");
                    break;
                }
            case 3:
                {
                    SceneManager.LoadScene("Level3");
                    break;
                }
            default:
                {
                    Debug.LogWarning("Level not incluyed in MySceneManager");
                    break;
                }
        }
    }

    /// <summary>
    /// Close the game.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}

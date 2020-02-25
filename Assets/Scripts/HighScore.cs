using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore
{
    private string name;
    private int score;

    public HighScore(string name, int score)
    {
        this.name = name;
        this.score = score;
    }

    /// <summary>
    /// Gets the highscore stored.
    /// </summary>
    /// <returns></returns>
    public int GetScore()
    {
        return score;
    }

    /// <summary>
    /// Gets the name introduced by the player in the highscore.
    /// </summary>
    /// <returns></returns>
    public string GetName()
    {
        return name;
    }
}

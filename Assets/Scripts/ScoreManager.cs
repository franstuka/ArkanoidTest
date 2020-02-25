using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{
    private int score;

    /// <summary>
    /// Restarts the game score to 0.
    /// </summary>
    public void ResetScore()
    {
        score = 0;
    }

    /// <summary>
    /// Add score.
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        this.score += score;
    }

    /// <summary>
    /// Get the score.
    /// </summary>
    /// <returns></returns>
    public int GetScore()
    {
        return score;
    }
}

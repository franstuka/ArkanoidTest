using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseScoreCapsule : Capsule
{
    [SerializeField] private int score;

    /// <summary>
    /// Increase score capsule effect.
    /// </summary>
    public override void AddEffect()
    {
        base.AddEffect();
        GameManager.instance.GetLevelManager().ChangeScore(score);
    }
}

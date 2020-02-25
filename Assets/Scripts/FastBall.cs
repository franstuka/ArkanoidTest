using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBall : Capsule
{
    /// <summary>
    /// Activates the fastball effect.
    /// </summary>
    public override void AddEffect()
    {
        base.AddEffect();
        GameManager.instance.GetLevelManager().GetBall().AddAppearance(Ball.BallAppearance.FASTBALL);
        GameManager.instance.GetLevelManager().GetBall().SetBallSpeed(GameManager.instance.GetLevelManager().LevelBallBaseSpeed * 1.5f);
    }

    /// <summary>
    /// Stops the fastball effect.
    /// </summary>
    public override void RemoveEffect()
    {
        base.RemoveEffect();
        GameManager.instance.GetLevelManager().GetBall().RemoveAppearance(Ball.BallAppearance.FASTBALL);
        GameManager.instance.GetLevelManager().GetBall().SetBallSpeed(GameManager.instance.GetLevelManager().LevelBallBaseSpeed);
    }
}

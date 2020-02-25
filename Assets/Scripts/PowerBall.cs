using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBall : Capsule
{
    /// <summary>
    /// Activates the powerball effect.
    /// </summary>
    public override void AddEffect()
    {
        base.AddEffect();
        GameManager.instance.GetLevelManager().GetBall().AddAppearance(Ball.BallAppearance.POWERBALL);
        GameManager.instance.GetLevelManager().GetBall().SetBallDamage(GameManager.instance.GetLevelManager().LevelBallBaseDamage * 2);
    }

    /// <summary>
    /// Stops the powerball effect.
    /// </summary>
    public override void RemoveEffect()
    {
        base.RemoveEffect();
        GameManager.instance.GetLevelManager().GetBall().RemoveAppearance(Ball.BallAppearance.POWERBALL);
        GameManager.instance.GetLevelManager().GetBall().SetBallDamage(GameManager.instance.GetLevelManager().LevelBallBaseDamage);
    }
}

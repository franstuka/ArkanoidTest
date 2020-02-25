using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickResistence : Capsule
{
    private const float INCREASE_LIFE_PROB = 50; //over 100

    /// <summary>
    /// Activates the BrickResistence effect.
    /// </summary>
    public override void AddEffect()
    {
        base.AddEffect();
        List<Brick> brickList= GameManager.instance.GetLevelManager().GetBrickList();
        float randomNumber;

        foreach(Brick brick in brickList)
        {
            randomNumber = Random.Range(0f, 100f);

            if(randomNumber < INCREASE_LIFE_PROB)
            {
                brick.AddHP();
            }
        }
    }
}

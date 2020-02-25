using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private const int MAX_HP = 4;
    [SerializeField] private int HP = 1;
    [SerializeField] private bool unbreakable;
    [SerializeField] private int scoreOnDestroy = 10;
    [SerializeField] private Sprite[] avaibleSprites;

    private void Start()
    {
        AddBrickToManager();
    }

    /// <summary>
    /// Add brick to the level manager.
    /// </summary>
    private void AddBrickToManager()
    {
        GameManager.instance.GetLevelManager().AddBrick(this);
    }

    /// <summary>
    /// Remove the brick from level manager.
    /// </summary>
    private void RemoveBrickOnManager()
    {
        GameManager.instance.GetLevelManager().RemoveBrick(this);
    }

    /// <summary>
    /// Do the operations needed when a brick is hitted.
    /// This funcion ignores the damage to unbreakable bricks.
    /// </summary>
    /// <param name="damage"></param>
    public void OnHit(int damage)
    {
        if(!unbreakable)
        {
            GetDamage(damage);

            if(HP > 0)
                ChangeApearance();
            else
            {
                RemoveBrickOnManager();
                Destroy(gameObject);
            }
        }  
    }

    /// <summary>
    /// Change the brick appearance depending on the brick hp and the sprites stored.
    /// </summary>
    public void ChangeApearance()
    {
        if (!unbreakable)
        {
            switch(HP)
            {
                case 1:
                    {
                        GetComponent<SpriteRenderer>().sprite = avaibleSprites[0];
                        break;
                    }
                case 2:
                    {
                        GetComponent<SpriteRenderer>().sprite = avaibleSprites[1];
                        break;
                    }
                case 3:
                    {
                        GetComponent<SpriteRenderer>().sprite = avaibleSprites[2];
                        break;
                    }
                case 4:
                    {
                        GetComponent<SpriteRenderer>().sprite = avaibleSprites[3];
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }

    /// <summary>
    /// Brick gets damage.
    /// </summary>
    /// <param name="damage"></param>
    private void GetDamage(int damage)
    {
        HP -= damage;
    }

    /// <summary>
    /// Get the score on destroy.
    /// </summary>
    /// <returns></returns>
    public int GetScoreOnDestroy()
    {
        return scoreOnDestroy;
    }

    /// <summary>
    /// Get if the brick is unbreakable.
    /// </summary>
    /// <returns></returns>
    public bool GetIfBrickIsUnbreakable()
    {
        return unbreakable;
    }

    /// <summary>
    /// Add hp to the brick.
    /// </summary>
    public void AddHP()
    {
        if(!unbreakable && HP < MAX_HP)
        {
            HP++;
            ChangeApearance();
        }
    }
}

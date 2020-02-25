using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public enum BallAppearance { NORMAL, POWERBALL, FASTBALL};

    [SerializeField] private float minSpeedYPercentage = 25f; // over 100, actues practically as a constant.
    [SerializeField] private float minSpeedXPercentage = 2f; // over 100, actues practically as a constant.
    [SerializeField] private Transform defaultStartPosition;
    [SerializeField] private GameObject[] particleSystems;

    private float fixedSpeed = 5f;
    private Vector2 ballDirection; //normalizated direction
    private int ballDamage = 1;
    private int activeBallEffectsCounter = 0;
    

    private void Awake()
    {
        if(defaultStartPosition == null)
            Debug.LogError("Ball hasn't starting point.");
    }

    private void Update()
    {
        Move();
    }

    /// <summary>
    /// Moves the ball.
    /// </summary>
    private void Move()
    {
        transform.position = new Vector3(
            transform.position.x + ballDirection.x * fixedSpeed * Time.deltaTime,
            transform.position.y + ballDirection.y * fixedSpeed * Time.deltaTime,
            transform.position.z);
    }

    /// <summary>
    /// Actions to do when the ball hits something.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollideWithObject(Collision2D collision)
    {
        Bounce(collision);

        if (!TestMinSpeedY())
        {
            SolveMinSpeedY();
        }
        if (!TestMinSpeedX())
        {
            SolveMinSpeedX();
        }
    }

    /// <summary>
    /// Bounce the ball depending of the plane of collision and gets the final direction.
    /// </summary>
    /// <param name="collision"></param>
    private void Bounce(Collision2D collision)
    {
        Vector2 u = Vector2.Dot(ballDirection, collision.GetContact(0).normal) * collision.GetContact(0).normal;
        Vector2 w = ballDirection - u;

        ballDirection = w - u;
    }

    /// <summary>
    /// Resets the ball at his default position and state.
    /// </summary>
    public void ResetBall()
    {
        transform.position = defaultStartPosition.position;
        SetRandomDirection();
        SetBallSpeed(GameManager.instance.GetLevelManager().LevelBallBaseSpeed);
        SetBallDamage(GameManager.instance.GetLevelManager().LevelBallBaseDamage);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Returns if ball speed Y is greater than the minimun allowed.
    /// </summary>
    /// <returns></returns>
    private bool TestMinSpeedY()
    {
        if(Mathf.Abs(ballDirection.y) > minSpeedYPercentage / 100)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Returns if ball speed X is greater than the minimun allowed.
    /// </summary>
    /// <returns></returns>
    private bool TestMinSpeedX()
    {
        if (Mathf.Abs(ballDirection.x) > minSpeedXPercentage / 100)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// This solves some problems that can cause ball not fall or do it soo slowly.
    /// </summary>
    private void SolveMinSpeedY()
    {
        float xComponentValue = CalculatesTheOtherComponentValueNormalizated(minSpeedYPercentage/100);

        if (ballDirection.y >= 0)
        {
            if(ballDirection.x >=0)
            {
                ballDirection = new Vector2(xComponentValue , minSpeedYPercentage / 100);
            }
            else
            {
                ballDirection = new Vector2(-xComponentValue, minSpeedYPercentage / 100);
            }
        }
        else
        {
            if (ballDirection.x >= 0)
            {
                ballDirection = new Vector2(xComponentValue, -minSpeedYPercentage / 100);
            }
            else
            {
                ballDirection = new Vector2(-xComponentValue, -minSpeedYPercentage / 100);
            }
        }
    }

    /// <summary>
    /// Solves the problem that the ball don't move in a horizonal way.
    /// </summary>
    private void SolveMinSpeedX()
    {
        float yComponentValue = CalculatesTheOtherComponentValueNormalizated(minSpeedXPercentage / 100);

        if (ballDirection.x >= 0)
        {
            if (ballDirection.y >= 0)
            {
                ballDirection = new Vector2(minSpeedXPercentage / 100, yComponentValue);
            }
            else
            {
                ballDirection = new Vector2(minSpeedXPercentage / 100, -yComponentValue);
            }
        }
        else
        {
            if (ballDirection.y >= 0)
            {
                ballDirection = new Vector2(-minSpeedXPercentage / 100, yComponentValue);
            }
            else
            {
                ballDirection = new Vector2(-minSpeedXPercentage / 100, -yComponentValue);
            }
        }
    }

    /// <summary>
    /// Sets the ball damage.
    /// </summary>
    /// <param name="ballDamage"></param>
    public void SetBallDamage(int ballDamage)
    {
        this.ballDamage = ballDamage;
    }
  

    /// <summary>
    /// Set the ball speed.
    /// </summary>
    /// <param name="speed"></param>
    public void SetBallSpeed(float speed)
    {
        fixedSpeed = speed;
    }

    /// <summary>
    /// Set one random direction to the ball.
    /// </summary>
    private void SetRandomDirection()
    {
        float randomY = Random.Range(minSpeedYPercentage / 100f, 1f);
        int randomSignX = Random.Range(0, 2);
        int randomSignY = Random.Range(0, 2);
        float xValue = CalculatesTheOtherComponentValueNormalizated(randomY);

        if (randomSignY == 0)
        {
            randomY = -randomY;
        }
        if (randomSignX == 0)
        {
            xValue = -xValue;
        }

        ballDirection = new Vector2(xValue, randomY);
    }

    /// <summary>
    /// Calculates the other value of normalizated direction if we set one of them to a determinated value.
    /// </summary>
    /// <param name="componentValue"></param>
    /// <returns></returns>
    private float CalculatesTheOtherComponentValueNormalizated(float componentValue)
    {
        return Mathf.Sqrt(1f - Mathf.Pow(componentValue, 2));
    }

    /// <summary>
    /// What happens if the ball is out of scenario.
    /// </summary>
    private void OutOffScenario()
    {
        GameManager.instance.GetLevelManager().OnLoseHP();
    }

    /// <summary>
    /// Remove one determinated ball appearance.
    /// </summary>
    /// <param name="appearance"></param>
    public void RemoveAppearance(BallAppearance appearance)
    {
        switch (appearance)
        {
            case BallAppearance.NORMAL:
                {
                    break;
                }
            case BallAppearance.POWERBALL:
                {
                    particleSystems[0].SetActive(false);
                    activeBallEffectsCounter--;
                    if (activeBallEffectsCounter == 0)
                    {
                        AddAppearance(BallAppearance.NORMAL);
                    }
                    break;
                }
            case BallAppearance.FASTBALL:
                {
                    particleSystems[1].SetActive(false);
                    activeBallEffectsCounter--;
                    if(activeBallEffectsCounter == 0)
                    {
                        AddAppearance(BallAppearance.NORMAL);
                    }
                    break;  
                }
        }
    }

    /// <summary>
    /// Add one determinated ball appearance.
    /// </summary>
    /// <param name="appearance"></param>
    public void AddAppearance(BallAppearance appearance)
    {
        switch (appearance)
        {
            case BallAppearance.NORMAL:
                {                  
                    break;
                }
            case BallAppearance.POWERBALL:
                {
                    activeBallEffectsCounter++;
                    particleSystems[0].SetActive(true);
                    break;
                }
            case BallAppearance.FASTBALL:
                {
                    activeBallEffectsCounter++;
                    particleSystems[1].SetActive(true);
                    break;
                }
        }
    }

    /// <summary>
    /// On Unity default colision, it happens after on fixedUpdate.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Brick"))
        {
            OnCollideWithObject(collision);
            collision.gameObject.GetComponent<Brick>().OnHit(ballDamage);
        }
        else if (collision.gameObject.CompareTag("Slider"))
        {
            OnCollideWithObject(collision);
        }
        else if (collision.gameObject.CompareTag("EndScenario"))
        {
            OutOffScenario();
        }
        else
        {
            OnCollideWithObject(collision);
        }     
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    [SerializeField] protected string capsuleName = "capsuleName";
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float remainingTime = 10f;
    private bool isActive = false;

    private void Start()
    {
        OnSpawn();
    }

    private void Update()
    {
        if(isActive)
        {
            ReduceTime();
        }
        else
        {
            Move();
        }
    }

    /// <summary>
    /// Overridable funcion by the capsule effects.
    /// </summary>
    public virtual void AddEffect()
    {

    }

    /// <summary>
    /// Overridable funcion by the capsule effects.
    /// </summary>
    public virtual void RemoveEffect()
    {

    }

    /// <summary>
    /// Get the capsule effect name.
    /// </summary>
    /// <returns></returns>
    public string GetName()
    {
        return capsuleName;
    }    

    /// <summary>
    /// Move the capsule.
    /// </summary>
    private void Move()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
    }

    /// <summary>
    /// Stop the movement of the capsule and set it active.
    /// </summary>
    private void SetActive()
    {
        isActive = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    /// <summary>
    /// When the capsule is hitted by the slider.
    /// </summary>
    public void OnHit()
    {
        SetActive();
        //active the capsule in the manager
        GameManager.instance.GetLevelManager().AddNewActiveCapsule(gameObject);
    }

    /// <summary>
    /// Reduce the remaining time of the effect of the capsule.
    /// </summary>
    public void ReduceTime()
    {
        remainingTime -= Time.deltaTime;
    }

    /// <summary>
    /// Set the remaining time of the effect of the capsule.
    /// </summary>
    /// <param name="time"></param>
    public void SetRemainingTime(float time)
    {
        remainingTime = time;
    }


    /// <summary>
    /// Get the remaining time of the effect of the capsule.
    /// </summary>
    public float GetRemainingTime()
    {
        return remainingTime;
    }

    /// <summary>
    /// When capsule spawn actions.
    /// </summary>
    private void OnSpawn()
    {
        GameManager.instance.GetLevelManager().AddCapsuleFalling(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Slider"))
        {
            OnHit();
        }
        else if (collision.gameObject.CompareTag("EndScenario"))
        {
            GameManager.instance.GetLevelManager().RemoveCapsuleFalling(this);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Gets the capsule sprite.
    /// </summary>
    /// <returns></returns>
    public Sprite GetSprite()
    {
        return gameObject.GetComponent<SpriteRenderer>().sprite;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    public enum Appearance { NORMAL};

    private float speed = 2f;
    private bool playerHasControl = false;

    private void Start()
    {
        SetSpeed(GameManager.instance.GetLevelManager().LevelSliderSpeed);
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// Moves the slider with the player input.
    /// </summary>
    private void Move()
    {     
        if(playerHasControl)
        {
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime * Input.GetAxisRaw("Horizontal"), transform.position.y, transform.position.z);
        }
    }

    /// <summary>
    /// Change the appearence, if there is more than one.
    /// </summary>
    public void ChangeAppearance()
    {

    }

    /// <summary>
    /// Sets slider speed.
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    /// <summary>
    /// Set if player has control.
    /// </summary>
    /// <param name="value"></param>
    public void SetPlayerHasControl(bool value)
    {
        playerHasControl = value;
    }
}

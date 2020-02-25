using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLateralScroll : MonoBehaviour
{
    //We can use a shader to made this, but this is more fast to do.

    [SerializeField] private float screenJump = 0f;
    [SerializeField] private GameObject[] backGroundGameObject;
    [SerializeField] private float backGroundSpeed = 0.25f;

    private void Update()
    {
        MoveBackGround();
    }

    /// <summary>
    /// Moves the background.
    /// </summary>
    private void MoveBackGround()
    {
        for (int i = 0; i < backGroundGameObject.Length; i++)
        {
            backGroundGameObject[i].transform.position = new Vector3(backGroundGameObject[i].transform.position.x - backGroundSpeed * Time.deltaTime,
                backGroundGameObject[i].transform.position.y,
                backGroundGameObject[i].transform.position.z);
            if (backGroundGameObject[i].transform.position.x <= -screenJump)
            {
                backGroundGameObject[i].transform.position = new Vector3(backGroundGameObject[i].transform.position.x + screenJump*2,
                    backGroundGameObject[i].transform.position.y,
                    backGroundGameObject[i].transform.position.z);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveCapsuleHUD : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMPro.TextMeshProUGUI timer;
    private Capsule capsuleReff;

    /// <summary>
    /// Sets the timer text, showing 3 numbers.
    /// </summary>
    /// <param name="time"></param>
    private void SetTimer(float time)
    {
        timer.text = ((int)(time*100)/100f).ToString();
    }

    /// <summary>
    /// Sets all the data needed from the capsule of reference.
    /// </summary>
    /// <param name="capsule"></param>
    public void SetCapsuleRefference(Capsule capsule)
    {
        capsuleReff = capsule;
        image.sprite = capsule.GetSprite();
        UpdateTimers();
    }

    /// <summary>
    /// Update the on screen timers.
    /// </summary>
    public void UpdateTimers()
    {
        SetTimer(capsuleReff.GetRemainingTime());
    }

    /// <summary>
    /// Gets the capsule reference.
    /// </summary>
    /// <returns></returns>
    public Capsule GetCapsuleReff()
    {
        return capsuleReff;
    }

}

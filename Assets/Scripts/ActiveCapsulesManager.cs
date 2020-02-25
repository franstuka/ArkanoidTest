using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCapsulesManager : MonoBehaviour
{
    private List<Capsule> activeCapsules = new List<Capsule>();

    void Update()
    {
        UpdateCapsulesStatus();
    }

    /// <summary>
    /// Add active capsule to the list.
    /// </summary>
    /// <param name="capsule"></param>
    /// <returns></returns>
    public bool AddCapsule(Capsule capsule)
    {
        bool insert = true;

        //search if capsule is active in the list
        for (int i = 0; i < activeCapsules.Count && insert; i++)
        {
            if(activeCapsules[i].GetName() == capsule.GetName())
            {
                //reset the time duration
                activeCapsules[i].SetRemainingTime(capsule.GetRemainingTime());
                insert = false;

            }
        }
        //insert the capsule in list if needed
        if(insert)
        {
            activeCapsules.Add(capsule);
        }

        return insert;
    }

    /// <summary>
    /// Removes an expired capsule from the list.
    /// </summary>
    /// <param name="capsule"></param>
    private void RemoveCapsule(Capsule capsule)
    {
        GameManager.instance.GetLevelManager().RemoveActiveCapsule(capsule);
        activeCapsules.Remove(capsule);
    }

    /// <summary>
    /// Update all the timers from the capsules.
    /// </summary>
    private void UpdateCapsulesStatus()
    {
        int i = 0;

        while(i < activeCapsules.Count)
        {
            if (activeCapsules[i].GetRemainingTime() < 0f)
            {          
                RemoveCapsule(activeCapsules[i]);
            }
            else
                i++;
        }
    }

    /// <summary>
    /// Ends all capsule effects.
    /// </summary>
    public void EndAllEffects()
    {
        while(activeCapsules.Count != 0)
        {
            RemoveCapsule(activeCapsules[0]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    [SerializeField] private GameObject[] availableCapsulesPrefab;

    /// <summary>
    /// Don't need to sum 100.
    /// </summary>
    [SerializeField] private float[] capsulesSpawnProb; 

    [SerializeField] private float globalSpawnProb = 5f;
    private float sumProbabilitie;

    private void Start()
    {
        if(capsulesSpawnProb.Length != availableCapsulesPrefab.Length)
        {
            Debug.LogError("Lenght of posible spawned capsules don't match with the Lenght of spawn probabilities");
        }

        SetProbabilitySum();
    }

    /// <summary>
    /// Roll the probability to spawn a new capsule.
    /// </summary>
    /// <param name="position">Posible position to spawn.</param>
    public void RollSpawnProb(Vector2 position)
    {
        float randomValue = Random.Range(0f, 100f);

        if(randomValue <= globalSpawnProb)
        {
            SpawnRandomCapsule(position);
        }
    }

    /// <summary>
    /// Spaws one random capsule at selected position.
    /// </summary>
    /// <param name="position"></param>
    private void SpawnRandomCapsule(Vector2 position)
    {
        GameObject newGameObject;
        float sumValue = 0;
        float randomValue = Random.Range(0f, sumProbabilitie);

        for (int i = 0; i < capsulesSpawnProb.Length; i++)
        {
            sumValue += capsulesSpawnProb[i];
            if(randomValue <= sumValue)
            {
                newGameObject = Instantiate(availableCapsulesPrefab[i]);
                newGameObject.transform.position = new Vector3(position.x, position.y, newGameObject.transform.position.z);
                break;
            }
        }
    }

    /// <summary>
    /// Calculates the sum probabilitie. With this form, pertentages in manager don't need to sum 100. 
    /// </summary>
    private void SetProbabilitySum()
    {
        sumProbabilitie = 0;

        for (int i = 0; i < capsulesSpawnProb.Length; i++)
        {
            sumProbabilitie += capsulesSpawnProb[i];
        }
    }
}

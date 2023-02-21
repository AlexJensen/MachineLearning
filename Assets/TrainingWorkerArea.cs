using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingWorkerArea : MonoBehaviour
{ 

    public GameObject goldPrefab;
    public int totalGold, minBatch, maxBatch, boardSize;

    GameObject[] goldPool;

    // Start is called before the first frame update
    void Start()
    {
        CreateGoldBatches();
    }

    public void CreateGoldBatches()
    {
        if (goldPool == null)
        {
            goldPool = new GameObject[totalGold];
            for (int i = 0; i < totalGold; i++)
            {
                goldPool[i] = Instantiate(goldPrefab);
            }
        }

        int goldLeft = totalGold;
        while (goldLeft > 0)
        {
            int batchSize = Random.Range(minBatch, maxBatch);

        }
    }
}

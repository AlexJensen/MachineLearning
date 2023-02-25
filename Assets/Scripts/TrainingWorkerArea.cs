using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrainingWorkerArea : MonoBehaviour
{ 

    public GameObject goldPrefab;
    public int totalGold, minBatch, maxBatch;
    public float boardSize;

    GameObject[] goldPool;

    public void CreateGoldBatches()
    {
        if (goldPool == null)
        {
            goldPool = new GameObject[totalGold];
            for (int i = 0; i < totalGold; i++)
            {
                goldPool[i] = Instantiate(goldPrefab);
                goldPool[i].transform.SetParent(transform);
                goldPool[i].SetActive(false);
            }
        }
        for (int i = 0; i < totalGold; i++)
        {
            if (!goldPool[i].activeSelf)
            {
                goldPool[i].transform.localPosition = new Vector3(Random.Range(-boardSize, boardSize), .5f, Random.Range(-boardSize, boardSize));
                goldPool[i].SetActive(true);
            }
        }
    }

    public GameObject GetClosestGold(WorkerAgent worker)
    {
        return goldPool.OrderBy(i => Vector3.Magnitude(i.transform.position - worker.transform.position)).ElementAt(0);
    }

    private void Update()
    {
        if (goldPool.Where(s=>s.activeSelf).Count() < totalGold / 2)
        {
            CreateGoldBatches();
        }
    }
}

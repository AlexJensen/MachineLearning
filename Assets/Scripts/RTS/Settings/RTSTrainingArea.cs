using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using UnityEngine;

public class RTSTrainingArea : MonoBehaviour
{ 
    public GameObject goldPrefab;
    public int totalGold;
    public float boardSize;

    GameObject[] goldPool;
    public WorkerAgent[] workerPool;
    public GuardAgent[] guardPool;
    public EnemyAgent[] enemyPool;

    public void Start()
    {
        workerPool = GetComponentsInChildren<WorkerAgent>();
        guardPool = GetComponentsInChildren<GuardAgent>();
        enemyPool = GetComponentsInChildren<EnemyAgent>();
    }

    public void InitializeTrial()
    {
        foreach (RTSAgent agent in workerPool)
        {
            agent.gameObject.SetActive(true);
            agent.transform.SetPositionAndRotation(agent.m_startPos, Quaternion.Euler(new Vector3(0f, Random.Range(0, 360))));
        }
        foreach (RTSAgent agent in guardPool)
        {
            agent.gameObject.SetActive(true);
            agent.transform.SetPositionAndRotation(agent.m_startPos, Quaternion.Euler(new Vector3(0f, Random.Range(0, 360))));
        }
        foreach (RTSAgent agent in enemyPool)
        {
            agent.gameObject.SetActive(true);
            agent.transform.SetPositionAndRotation(agent.m_startPos, Quaternion.Euler(new Vector3(0f, Random.Range(0, 360))));
        }
        InitializeGold();
        RefreshGold(true);
    }

    private void InitializeGold()
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
    }

    private void RefreshGold(bool reset = false)
    {
        for (int i = 0; i < totalGold; i++)
        {
            if (!goldPool[i].activeSelf || reset)
            {
                goldPool[i].transform.localPosition = new Vector3(Random.Range(-boardSize, boardSize), .5f, Random.Range(-boardSize, boardSize));
                goldPool[i].SetActive(true);
            }
        }
    }

    

    public GameObject GetClosestGold(Agent agent)
    {
        return goldPool.OrderBy(i => Vector3.Magnitude(i.transform.position - agent.transform.position)).ElementAt(0);
    }

    public GameObject GetClosestWorker(Agent agent)
    {
        return workerPool.OrderBy(i => Vector3.Magnitude(i.transform.position - agent.transform.position)).ElementAt(0).gameObject;
    }
    public GameObject GetClosestGuard(Agent agent)
    {
        return guardPool.OrderBy(i => Vector3.Magnitude(i.transform.position - agent.transform.position)).ElementAt(0).gameObject;
    }
    public GameObject GetClosestEnemy(Agent agent)
    {
        return enemyPool.OrderBy(i => Vector3.Magnitude(i.transform.position - agent.transform.position)).ElementAt(0).gameObject;
    }

    private void Update()
    {
        if (goldPool.Where(s=>s.activeSelf).Count() < totalGold / 2)
        {
            RefreshGold();
        }
    }
}

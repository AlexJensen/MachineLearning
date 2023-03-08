using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using TMPro;
using UnityEngine;

public class HQAgent : MonoBehaviour
{
    public TextMeshProUGUI goldGui;

    public int gold = 0;

    internal void OnCollect(WorkerAgent agent)
    {
        if (agent.gold > 0)
        {
            agent.AddReward(Mathf.Pow(agent.gold, 2));
            gold += agent.gold;
            agent.gold = 0;
        }
        else
        {
            agent.AddReward(-0.1f);
        }
    }
    private void Update()
    {
        goldGui.text = gold.ToString();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Gold"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}

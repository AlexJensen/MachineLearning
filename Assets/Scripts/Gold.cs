
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public int minValue, maxValue;
    int value;
    Vector3 initialScale;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    private void OnEnable()
    {
        value = Random.Range(minValue, maxValue + 1);
        transform.localScale = initialScale * (value / 1.5f);
    }

    internal void OnObtained(WorkerAgent agent)
    {
        if (agent.gold + value <= agent.carryCapacity)
        {
            agent.AddReward(value);
            gameObject.SetActive(false);
            agent.gold += value;
        }
        else
        {
            int openSpace = agent.carryCapacity - agent.gold;
            agent.gold += openSpace;
            value -= openSpace;
            agent.AddReward(openSpace);
            transform.localScale = initialScale * (value / 1.5f);
        }
    }
}

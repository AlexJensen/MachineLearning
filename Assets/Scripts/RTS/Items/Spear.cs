using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public Agent agent;

    public string[] EnemyTags;
    public string[] AllyTags;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != agent.gameObject)
        {
            foreach (string tag in EnemyTags)
            {
                if (collision.gameObject.CompareTag(tag))
                {
                    collision.gameObject.GetComponent<RTSAgent>().AddReward(-10f);
                    collision.gameObject.SetActive(false);
                    agent.AddReward(100f);
                }
            }
            foreach (string tag in AllyTags)
            {
                if (collision.gameObject.CompareTag(tag))
                {
                    collision.gameObject.GetComponent<RTSAgent>().AddReward(-10f);
                    collision.gameObject.SetActive(false);
                    agent.AddReward(-100f);
                }
            }
        }
    }
}

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using TMPro;
using UnityEditor;

public class GuardAgent : RTSAgent
{
    public RTSTrainingArea area;
    public HQAgent HQ;

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);

        GameObject closestWorker = area.GetClosestWorker(this);
        GameObject closestEnemy = area.GetClosestEnemy(this);
        sensor.AddObservation(closestWorker.transform.position.x);
        sensor.AddObservation(closestWorker.transform.position.z);
        sensor.AddObservation(closestEnemy.transform.position.x);
        sensor.AddObservation(closestEnemy.transform.position.z);
        sensor.AddObservation(HQ.gold);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-1f);
        }
    }

    private void Update()
    {
        // Encourage mutual cooperation
        AddReward(Time.fixedDeltaTime * (HQ.gold / 100f));
    }
}

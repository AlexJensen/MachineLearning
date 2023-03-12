using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using TMPro;
using UnityEditor;

public class WorkerAgent : RTSAgent
{
    public RTSTrainingArea area;
    public HQAgent HQ;
    
    public TextMeshProUGUI goldGui;
    public int gold = 0;
    public int carryCapacity;
    public bool detailedInfo = false;

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        gold = 0;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);

        sensor.AddObservation(HQ.transform.position.x);
        sensor.AddObservation(HQ.transform.position.z);
        sensor.AddObservation(HQ.gold);
        sensor.AddObservation(gold);
        sensor.AddObservation(carryCapacity);

        GameObject closestGold = area.GetClosestGold(this);
        GameObject closestGuard = area.GetClosestGuard(this);
        GameObject closestEnemy = area.GetClosestEnemy(this);
        sensor.AddObservation(closestGold.transform.position.x);
        sensor.AddObservation(closestGold.transform.position.z);
        sensor.AddObservation(closestGuard.transform.position.x);
        sensor.AddObservation(closestGuard.transform.position.z);
        sensor.AddObservation(closestEnemy.transform.position.x);
        sensor.AddObservation(closestEnemy.transform.position.z);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Gold"))
        {
            collision.gameObject.GetComponent<Gold>().OnObtained(this);
        }
        if (collision.gameObject.CompareTag("HQ"))
        {
            collision.gameObject.GetComponent<HQAgent>().OnCollect(this);
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-1f);
        }
    }

    private void Update()
    {
        goldGui.transform.rotation = Quaternion.Euler(new Vector3(45, 0, 0));

        // Encourage mutual cooperation
        AddReward(Time.fixedDeltaTime * (HQ.gold / 10f));
    }

    public override void MoveAgent(ActionBuffers actions)
    {
        base.MoveAgent(actions);
        goldGui.text = detailedInfo ? gold.ToString() + "\n" + actions.ContinuousActions[0] + ":" + actions.ContinuousActions[1] + "\n" + GetCumulativeReward() : gold.ToString();
    }
}

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using TMPro;
using UnityEditor;

public class WorkerAgent : Agent
{
    public TrainingWorkerArea area;
    public HQAgent HQ;

    // Speed of agent rotation.
    public float turnSpeed = 300;

    // Speed of agent movement.
    public float moveSpeed = 2;

    public TextMeshProUGUI goldGui;
    public int gold = 0;
    public int carryCapacity;
    public bool detailedInfo = false;

    Rigidbody m_rigidbody;
    Vector3 m_startPos;



    public override void Initialize()
    {
        m_startPos = transform.localPosition;
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        m_rigidbody.velocity = Vector3.zero;
        transform.localPosition = m_startPos;
        transform.localRotation = Quaternion.identity;
        HQ.gold = 0;
        area.CreateGoldBatches(true);
        gold = 0;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        var localVelocity = transform.InverseTransformDirection(m_rigidbody.velocity);
        sensor.AddObservation(transform.position.x);
        sensor.AddObservation(transform.position.z);
        sensor.AddObservation(HQ.transform.position.x);
        sensor.AddObservation(HQ.transform.position.z);
        sensor.AddObservation(HQ.gold);
        sensor.AddObservation(localVelocity.x);
        sensor.AddObservation(localVelocity.z);
        sensor.AddObservation(gold);
        sensor.AddObservation(carryCapacity);
        sensor.AddObservation(area.GetClosestGold(this).transform.position.x);
        sensor.AddObservation(area.GetClosestGold(this).transform.position.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        MoveAgent(actions);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        if (Input.GetKey(KeyCode.D))
        {
            continuousActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            continuousActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            continuousActionsOut[1] = -1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            continuousActionsOut[0] = -1;
        }
    }

    public void MoveAgent(ActionBuffers actions)
    {
        ActionSegment<float> continuousActions = actions.ContinuousActions;

        var forward = Mathf.Clamp(continuousActions[0], -1f, 1f);
        var right = Mathf.Clamp(continuousActions[1], -1f, 1f);
        var rotate = Mathf.Clamp(continuousActions[2], -1f, 1f);

        Vector3 dirToGo = transform.forward * forward;
        Vector3 rotateDir = -transform.up * rotate;

        dirToGo += transform.right * right;



        goldGui.text = detailedInfo? gold.ToString() + "\n" + continuousActions[0] + ":" + continuousActions[1] + "\n" + GetCumulativeReward(): gold.ToString();

        m_rigidbody.AddForce(dirToGo * moveSpeed, ForceMode.VelocityChange);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);

        if (m_rigidbody.velocity.magnitude > 10f) // slow it down
        {
            m_rigidbody.velocity = m_rigidbody.velocity.normalized * 10f;
        }
        if (Mathf.Abs(forward) < 0.1f && Mathf.Abs(right) < 0.1f)
        {
            m_rigidbody.velocity *= 0.1f;
        }
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
        //goldGui.text = gold.ToString();
        goldGui.transform.rotation = Quaternion.Euler(new Vector3(45, 0, 0));
        AddReward(Time.fixedDeltaTime * (HQ.gold / 10f));
    }
}

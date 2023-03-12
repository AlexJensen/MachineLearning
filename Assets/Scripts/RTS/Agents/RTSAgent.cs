using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class RTSAgent : Agent
{
    Rigidbody m_rigidbody;
    public Vector3 m_startPos;
    bool initialized = false;

    // Speed of agent rotation.
    public float turnSpeed = 300;

    // Speed of agent movement.
    public float moveSpeed = 2;
    public float maxSpeed = 10;

    public override void Initialize()
    {
        base.Initialize();
        if (!initialized)
        {
            initialized = true;
            m_startPos = transform.position;
        }

        m_rigidbody = GetComponent<Rigidbody>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (initialized)
        {
            transform.SetPositionAndRotation(m_startPos, Quaternion.Euler(new Vector3(0f, Random.Range(0, 360))));
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);

        sensor.AddObservation(transform.position.x);
        sensor.AddObservation(transform.position.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        MoveAgent(actions);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        if (Input.GetKey(KeyCode.W))
        {
            continuousActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            continuousActionsOut[0] = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            continuousActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            continuousActionsOut[1] = -1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            continuousActionsOut[2] = 1;
        }
        if (Input.GetKey(KeyCode.E))
        {
            continuousActionsOut[2] = -1;
        }
    }

    public virtual void MoveAgent(ActionBuffers actions)
    {
        ActionSegment<float> continuousActions = actions.ContinuousActions;

        var forward = Mathf.Clamp(continuousActions[0], -1f, 1f);
        var right = Mathf.Clamp(continuousActions[1], -1f, 1f);
        var rotate = Mathf.Clamp(continuousActions[2], -1f, 1f);

        Vector3 dirToGo = transform.forward * forward;
        Vector3 rotateDir = -transform.up * rotate;

        dirToGo += transform.right * right;

        m_rigidbody.AddForce(dirToGo * moveSpeed, ForceMode.VelocityChange);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);

        // Clamp to max speed
        if (m_rigidbody.velocity.magnitude > maxSpeed)
        {
            m_rigidbody.velocity = m_rigidbody.velocity.normalized * maxSpeed;
        }

        // Put on the brakes if input is within min bounds
        if (Mathf.Abs(forward) < 0.1f && Mathf.Abs(right) < 0.1f)
        {
            m_rigidbody.velocity *= 0.1f;
        }
    }
}
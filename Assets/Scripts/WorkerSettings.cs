using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;

public class WorkerSettings : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] agents;
    [HideInInspector]
    public TrainingWorkerArea[] listArea;

    public int totalScore;

    StatsRecorder m_Recorder;

    public void Awake()
    {
        Academy.Instance.OnEnvironmentReset += EnvironmentReset;
        m_Recorder = Academy.Instance.StatsRecorder;
    }

    void EnvironmentReset()
    {
        listArea = FindObjectsOfType<TrainingWorkerArea>();
        foreach (var fa in listArea)
        {
            fa.CreateGoldBatches();
        }

        totalScore = 0;
    }

    public void Update()
    {
        // Send stats via SideChannel so that they'll appear in TensorBoard.
        // These values get averaged every summary_frequency steps, so we don't
        // need to send every Update() call.
        if ((Time.frameCount % 100) == 0)
        {
            m_Recorder.Add("TotalScore", totalScore);
        }
    }
}

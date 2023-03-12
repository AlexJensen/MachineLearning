using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;

public class RTSSettings : MonoBehaviour
{
    [HideInInspector]
    public RTSTrainingArea[] listArea;

    public int totalScore;

    StatsRecorder m_Recorder;

    public void Awake()
    {
        Academy.Instance.OnEnvironmentReset += EnvironmentReset;
        m_Recorder = Academy.Instance.StatsRecorder;
        listArea = FindObjectsOfType<RTSTrainingArea>();
    }

    void EnvironmentReset()
    {
        foreach (RTSTrainingArea fa in listArea)
        {
            fa.InitializeTrial();
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

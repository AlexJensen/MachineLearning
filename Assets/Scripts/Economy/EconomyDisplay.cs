using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyDisplay : MonoBehaviour
{
    public TMP_Text money, supply, workers, fighters, time;
    public EconomyAgent agent;
    // Update is called once per frame
    void Update()
    {
        money.text = "Cash: " + agent.money.ToString();
        supply.text = "Supply: " + agent.Demand.ToString() + "/" + agent.supply.ToString();
        workers.text = "Workers: " + agent.workers.ToString();
        fighters.text = "Fighters: "+ agent.fighters.ToString();
        time.text = "Time: " + agent.time.ToString();
    }
}

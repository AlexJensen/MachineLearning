using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class EconomyAgent : Agent
{
    public int startMoney, startSupply, supplyCap, startTime, startWorkers, startFighters;
    public int raidFrequency, startRaid, raidDifficultyIncrease;
    public int workerCost, fighterCost, supplyCost, supplyIncrement;
    public float workerIncome;
    
    internal int money, time, workers, fighters, supply, raidIntensity;

    internal int Demand
    {
        get { return workers + fighters; }
    }

    public override void OnEpisodeBegin()
    {
        //Initialize starting values
        money = startMoney;
        supply = startSupply;
        time = 0;
        workers = startWorkers;
        fighters = startFighters;
        raidIntensity = startRaid;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(money);
        sensor.AddObservation(supply);
        sensor.AddObservation(Demand);
        sensor.AddObservation(time);
        sensor.AddObservation(workers);
        sensor.AddObservation(fighters);
        sensor.AddObservation(raidIntensity);
        sensor.AddObservation(workerCost);
        sensor.AddObservation(fighterCost);
        sensor.AddObservation(supplyCost);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        time++;
        if (time >= startTime)
        {
            AddReward(workers * 10);
            AddReward(money / 100);
            EndEpisode();
            return;
        }

        if (time % raidFrequency == 0)
        {
            fighters -= Mathf.Max(0,raidIntensity);
            raidIntensity += raidDifficultyIncrease;
            AddReward(fighters);
            if (fighters >= 0)
            {
                AddReward(workers);
            }
            else
            {
                AddReward(-(startTime - time) * 10);
                AddReward(-money / 10);
                EndEpisode();
                return;
            }
        }

            //Determine purchases
            var discreteActions = actionBuffers.DiscreteActions;
            var buyWorker = discreteActions[0] > 0;
            var buyFighter = discreteActions[1] > 0;
            var buySupply = discreteActions[2] > 0;

            if (buyWorker)
            {
                if (money >= workerCost && supply > Demand)
                {
                    money -= workerCost;
                    AddReward(10f);
                    workers++;
                }
        }
            if (buyFighter)
            {
                if (money >= fighterCost && supply > Demand)
                {
                    money -= fighterCost;
                    AddReward(10f);
                    fighters++;
                }
            }
            if (buySupply)
            {
                if (money >= supplyCost && supply < supplyCap)
                {
                    money -= supplyCost;
                    if (Mathf.Abs(supply - Demand) < supplyIncrement)
                    {
                        AddReward(100f);
                    }
                    supply += supplyIncrement;
                    if (supply > supplyCap)
                    {
                        supply = supplyCap;
                    }
                }
            }

        if (time % 10 == 0)
        {
            //Generate income
            money += Mathf.FloorToInt(workerIncome * workers);
            AddReward(-money / 1000);
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = Input.GetKey(KeyCode.Alpha1) ? 1 : 0;
        discreteActionsOut[1] = Input.GetKey(KeyCode.Alpha2) ? 1 : 0;
        discreteActionsOut[2] = Input.GetKey(KeyCode.Alpha3) ? 1 : 0;
    }
}

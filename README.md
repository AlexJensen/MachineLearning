# MachineLearning
Practice project for learning Unity ML-Agents

Feel free to use any part of this project for anything you want it is purely for learning purposes.

This repo is to explore the various use cases for Unity's ML-Agents package. I won't be documenting it very well and very little focus will be spent on legibility or
higher level proper architecture practices so apologies in advance if any of the code is confusing or difficult to understand. 

Currently the project primarly contains the training tools for an RTS scenario consisting of three different agent types. The Worker agent is tasked with gathering
gold from the environment and returning it to the central HQ. Bandit agents are tasked with hunting down Worker agents to prevent the HQ from gaining gold and Guard
agents are tasked with protecting the Worker agents and defeating all the Bandit agents.

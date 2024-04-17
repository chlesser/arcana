using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeEnabler : MonoBehaviour
{
    GameManager gameManager;
    List<int> nodeRewards = new List<int>();
    List<(int, int)> nodePaths = new List<(int, int)>();
    public int currNode;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        nodeRewards.Add(42);
        nodeRewards.Add(41);
        nodeRewards.Add(34);
        nodeRewards.Add(33);
        nodeRewards.Add(52);
        nodeRewards.Add(51);
        nodeRewards.Add(44);
        nodeRewards.Add(43);
        nodeRewards.Add(62);
        nodeRewards.Add(61);
        nodeRewards.Add(99);
        currNode = 0;
        nodePaths.Add((0, 1));
        nodePaths.Add((0, 2));
        nodePaths.Add((1, 3));
        nodePaths.Add((2, 3));
        nodePaths.Add((2, 4));
        nodePaths.Add((3, 5));
        nodePaths.Add((4, 5));
        nodePaths.Add((5, 6));
        nodePaths.Add((5, 7));
        nodePaths.Add((6, 8));
        nodePaths.Add((7, 8));
        nodePaths.Add((7, 9));
        nodePaths.Add((8, 10));
        nodePaths.Add((9, 10));
    }
    public void updateNodes(int nodeID) {
        /*Archaic
        if(nodeID == 0) {
            updateIfLocked(1);
            updateIfLocked(2);
        } else if(nodeID == 1) {
            updateIfLocked(3);
        } else if(nodeID == 2) {
            updateIfLocked(4);
            updateIfLocked(3);
        } else if(nodeID == 3) {
            updateIfLocked(5);
        } else if(nodeID == 4) {
            updateIfLocked(5);
        } else if(nodeID == 5) {
            updateIfLocked(6);
            updateIfLocked(7);
            updateIfLocked(3);
            updateIfLocked(4);
        } else if (nodeID == 6) {
            updateIfLocked(8);
        } else if (nodeID == 7) {
            updateIfLocked(8);
            updateIfLocked(9);
        } else if (nodeID == 8) {
            updateIfLocked(10);
            updateIfLocked(7);
            updateIfLocked(8);
        } else if (nodeID == 9) {
            updateIfLocked(10);
            updateIfLocked(7);
        } */
        foreach ((int, int) entry in nodePaths) {
            if(entry.Item1 == nodeID) {
                updateIfLocked(entry.Item2);
                Debug.Log("Updating node " + entry.Item2);
            } else if (entry.Item2 == nodeID) {
                updateIfLocked(entry.Item1);
                Debug.Log("Updating node " + entry.Item1);
            }
        }
        currNode = nodeID;

    }
    public void updateIfLocked(int nodeID) {
        if(gameManager.nodes[nodeID] == 0) {
            gameManager.nodes[nodeID] = 1;
        }
    }
    public int getNodeReward() {
        return nodeRewards[currNode];
    }
}
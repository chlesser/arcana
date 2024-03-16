using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeEnabler : MonoBehaviour
{
    GameManager gameManager;
    List<int> nodeRewards = new List<int>();
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
    }
    public void updateNodes(int nodeID) {
        if(nodeID == 0) {
            //temporary to speed things up
            updateIfLocked(1);
            updateIfLocked(2);
            updateIfLocked(10);
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
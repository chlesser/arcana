using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeEnabler : MonoBehaviour
{
    GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void updateNodes(int nodeID) {
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
        }
    }
    public void updateIfLocked(int nodeID) {
        if(gameManager.nodes[nodeID] == 0) {
            gameManager.nodes[nodeID] = 1;
        }
    }

}
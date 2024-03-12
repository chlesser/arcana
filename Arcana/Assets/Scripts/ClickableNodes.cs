using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableNodes : MonoBehaviour, IPointerDownHandler
{
    GameManager gameManager;
    public int nodeID;
    //state 0 = locked, state 1 = unlocked, state 2 = completed
    public int state;
    private void Awake()
    {
        AddPhysics2DRaycaster();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        nameParse();
        state = gameManager.addNode(nodeID);
        if(gameManager == null) {
            Debug.Log("GameManager is null");
        }
        if(state == 1) {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.55f);
        }
        if(state == 2) {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0f);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + this.gameObject.name);
        if(state == 0) {return;}
        gameManager.nextNode();
        gameManager.battle(nodeID);
    }
    private void AddPhysics2DRaycaster()
    {
        Physics2DRaycaster physicsRaycaster = FindObjectOfType<Physics2DRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }
    //temp solutions
    private Color startcolor;
    void OnMouseEnter()
    {
        if(state == 1) {
            startcolor = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }
    void OnMouseExit()
    {
        if(state == 1) {
        GetComponent<SpriteRenderer>().color = startcolor;
        }
    }
    // end of temp solutions
    void nameParse() {
        string[] s = this.gameObject.name.Split(' ');
        nodeID = int.Parse(s[1]);}
}

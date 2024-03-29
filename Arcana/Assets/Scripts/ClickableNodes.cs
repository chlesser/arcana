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
        if(nodeID != 10) {
            if(state == 1) {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.6392157f, 0.6235294f, 0.4784314f, 1f);
            }
            if(state == 2) {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + this.gameObject.name);
        if(!(state == 1 && gameManager.nodesClickable)) {return;}
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
        if(state == 1 && nodeID != 10) {
            startcolor = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(0.796f, 0.792f, 0.65f, 1f);
        }
        if(state == 1 && nodeID == 10) {
            startcolor = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.75f);
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

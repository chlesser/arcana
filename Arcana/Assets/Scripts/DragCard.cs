using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragCard : MonoBehaviour, IPointerDownHandler
{
    bool clicked = false;
    bool playable = false;
    Vector3 OriginalPosition;
    int OriginalOrder;

    Collider2D playAreaCollider;
    Collider2D thisCollider;

    private void Start()
    {
        AddPhysics2DRaycaster();
        thisCollider = this.gameObject.GetComponent<Collider2D>();
        playAreaCollider = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<Collider2D>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!clicked) {
            // Çard hasn't been clicked yet
            OriginalPosition = transform.position;
            OriginalOrder = this.gameObject.GetComponent<Renderer>().sortingOrder;
            clicked = true;
            this.gameObject.GetComponent<Renderer>().sortingOrder = this.gameObject.transform.parent.GetComponent<HandManager>().getCardNum() * 2 + 2;
            this.gameObject.transform.GetChild(0).GetComponent<Renderer>().sortingOrder = this.gameObject.transform.parent.GetComponent<HandManager>().getCardNum() * 2 + 3;
        } else if (clicked && playable && !Input.GetMouseButtonDown(1)) {
            // Card has been clicked and is playable
            this.gameObject.GetComponent<Renderer>().sortingOrder = OriginalOrder;
            this.gameObject.transform.GetChild(0).GetComponent<Renderer>().sortingOrder = OriginalOrder + 1;
            clicked = false;
            this.gameObject.transform.position = GameObject.FindGameObjectWithTag("PlayArea").transform.position;
        } else {
            // Card has been clicked and is not playable
            this.gameObject.transform.position = OriginalPosition;
            this.gameObject.GetComponent<Renderer>().sortingOrder = OriginalOrder;
            this.gameObject.transform.GetChild(0).GetComponent<Renderer>().sortingOrder = OriginalOrder + 1;
            clicked = false;
        }
        
    }

    private void AddPhysics2DRaycaster()
    {
        Physics2DRaycaster physicsRaycaster = FindObjectOfType<Physics2DRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }

    void Update()
    {
        if(clicked) {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            this.gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
    }
    void FixedUpdate()
    {
        if (thisCollider != null && playAreaCollider != null) {
            if (thisCollider.bounds.Intersects(playAreaCollider.bounds)) {
                Debug.Log("Card is playable");
                playable = true;
            } else {
                playable = false;
            }
        } else {
            Debug.LogError("One of the colliders is missing!");
        }
    }
    /*void OnTriggerEnter2D(Collision2D col)
    {
        Debug.Log("Collision detected");
        if(col.gameObject.tag == "PlayArea") {
            Debug.Log("Card is playable");
            playable = true;
        }
    }
    void OnTriggerExit2D(Collision2D col)
    {
        if(col.gameObject.tag == "PlayArea") {
            Debug.Log("Card is not playable");
            playable = false;
        }
    } */
}

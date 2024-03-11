using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragCard : MonoBehaviour, IPointerDownHandler
{
    //Indicates if it is currently following the cursor
    bool clicked = false;
    //Indicates if the card can enter the play area
    bool playable = false;
    //Indicates if the card is in the playarea
    bool played = false;
    Vector3 OriginalPosition;
    int OriginalOrder;

    //Dealing with hand continuation... c is the card that is in the list hand
    public DeckManager.Card c;
    DeckManager DeckManager;

    Collider2D playAreaCollider;
    Collider2D thisCollider;

    //accesses the only useful part of the card. use for rendering and collision
    GameObject image;
    GameObject number;

    private void Awake()
    {
        image = this.gameObject.transform.GetChild(0).gameObject;
        number = this.gameObject.transform.GetChild(0).GetChild(0).gameObject;
        AddPhysics2DRaycaster();
        thisCollider = image.GetComponent<Collider2D>();
        playAreaCollider = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<Collider2D>();
        DeckManager = GameObject.FindGameObjectWithTag("DeckManager").GetComponent<DeckManager>();
        OriginalPosition = transform.position;
        OriginalOrder = image.GetComponent<Renderer>().sortingOrder;

        /*foreach (GameObject go in GameObject.FindGameObjectsWithTag("PlayArea")) {
            if (go.GetComponent<Collider2D>() != null) {
                playAreaCollider = go.GetComponent<Collider2D>();
            }
        } */
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!this.transform.parent.GetComponent<HandManager>().cardsCanBeClicked) {
            return;
        }
        else if(!clicked && !played) {
            // Çard hasn't been clicked yet
            OriginalPosition = transform.position;
            OriginalOrder = image.GetComponent<Renderer>().sortingOrder;
            clicked = true;
            image.GetComponent<Renderer>().sortingOrder = this.gameObject.transform.parent.GetComponent<HandManager>().getCardNum() * 2 + 2;
            number.GetComponent<Renderer>().sortingOrder = this.gameObject.transform.parent.GetComponent<HandManager>().getCardNum() * 2 + 3;
            number.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Grabbed");
            image.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Grabbed");
        }  //nothing
        else if (clicked && playable && !Input.GetMouseButtonDown(1)) {
            // Card has been clicked and is playable
            image.GetComponent<Renderer>().sortingOrder = OriginalOrder;
            number.GetComponent<Renderer>().sortingOrder = OriginalOrder + 1;
            number.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Cards");
            image.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Cards");
            clicked = false;
            this.gameObject.transform.position = GameObject.FindGameObjectWithTag("PlayArea").transform.position;
            played = true;
        } else if (!clicked && played && !Input.GetMouseButtonDown(1)) {
            //its done
            endTurn();
        } else {
            // Card has been clicked and is not playable
            played = false;
            this.gameObject.transform.position = OriginalPosition;
            image.GetComponent<Renderer>().sortingOrder = OriginalOrder;
            number.GetComponent<Renderer>().sortingOrder = OriginalOrder + 1;
            number.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Cards");
            image.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Cards");
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
    // Everything Else
    void Update()
    {
        /*if(Input.GetMouseButtonDown(0)) {
            GameObject perishTheThought = eventData.rawPointerPress;
            if (this.gameObject != perishTheThought) {
                doofus = true;
            } else {
                doofus = false;
            }
    } */
 
        if(clicked) {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            this.gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
    }
    // Physics
    void FixedUpdate()
    {
        if (thisCollider != null && playAreaCollider != null) {
            if (thisCollider.bounds.Intersects(playAreaCollider.bounds)) {
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
    void endTurn() {
        DeckManager.d.playCard(c);
        Destroy(this.gameObject);
    }
}

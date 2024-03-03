using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragCard : MonoBehaviour, IPointerDownHandler
{
    bool clicked = false;
    Vector3 OriginalPosition;
    int OriginalOrder;

    private void Start()
    {
        AddPhysics2DRaycaster();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
        if(!clicked) {
            OriginalPosition = transform.position;
            OriginalOrder = this.gameObject.GetComponent<Renderer>().sortingOrder;
            clicked = true;
            this.gameObject.GetComponent<Renderer>().sortingOrder = this.gameObject.transform.parent.GetComponent<HandManager>().getCardNum() * 2 + 2;
            this.gameObject.transform.GetChild(0).GetComponent<Renderer>().sortingOrder = this.gameObject.transform.parent.GetComponent<HandManager>().getCardNum() * 2 + 3;
        } else if (clicked && Input.GetMouseButtonDown(1)) {
            this.gameObject.transform.position = OriginalPosition;
            this.gameObject.GetComponent<Renderer>().sortingOrder = OriginalOrder;
            this.gameObject.transform.GetChild(0).GetComponent<Renderer>().sortingOrder = OriginalOrder + 1;
            clicked = false;
        } else {
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
}

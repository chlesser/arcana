using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableNodes : MonoBehaviour, IPointerDownHandler
{
    GameManager gameManager;
    private void Awake()
    {
        AddPhysics2DRaycaster();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked: " + this.gameObject.name);
        gameManager.nextNode();
        gameManager.battle(this.gameObject);
    }
    private void AddPhysics2DRaycaster()
    {
        Physics2DRaycaster physicsRaycaster = FindObjectOfType<Physics2DRaycaster>();
        if (physicsRaycaster == null)
        {
            Camera.main.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }
}

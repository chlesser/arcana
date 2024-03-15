using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class helpButt : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject helpCover;
    public GameObject page1;
    public GameObject page2;
    bool panelIsUp = false;
    GameManager gameManager;
    public void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if(panelIsUp) {
            page1.SetActive(false);
            page2.SetActive(false);
            if(gameManager.gameObject.transform.GetComponentInChildren<HandManager>() != null) {
                gameManager.gameObject.transform.GetComponentInChildren<HandManager>().cardsCanBeClicked = true;
            }
            panelIsUp = false;
        } else {
            page1.SetActive(true);
            page2.SetActive(false);
            panelIsUp = true;
            if(gameManager.gameObject.transform.GetComponentInChildren<HandManager>() != null) {
                Debug.Log("Setting cards to clickable");
                gameManager.gameObject.transform.GetComponentInChildren<HandManager>().cardsCanBeClicked = false;
            }
        }
    }
    public void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
    {
        helpCover.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.3f, 0.5f);
        helpCover.SetActive(true);
    }
    public void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
    {
        helpCover.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        helpCover.SetActive(false);
    }
    public void off() {
        this.gameObject.SetActive(false);
    }
    public void on() {
        this.gameObject.SetActive(true);
    }
    public void fadeOut() {
        StartCoroutine(fade(false, 1));
    }
    public void fadeIn() {
        StartCoroutine(fade(true, 1));
    }
    
    IEnumerator fade(bool goingIn, float time) {
        if(goingIn) {
            for(float i = 0; i < 1; i += Time.deltaTime / time) {
                this.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, i);
                yield return null;
            }
        } else {
            for(float i = 1; i > 0; i -= Time.deltaTime / time) {
                this.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }
    void Awake() {
        off();
        helpCover = GameObject.Find("HelpCover");
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        helpCover.SetActive(false);
    }
}

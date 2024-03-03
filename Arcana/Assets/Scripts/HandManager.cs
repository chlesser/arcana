using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandManager : MonoBehaviour
{
    Vector3 pos;  
    int offset;
    int cardNum;
    public int gapSize;
    DeckManager DeckManager;
    void Awake() {
        DeckManager = this.transform.parent.GetComponentInChildren<DeckManager>();
        pos = this.transform.position;
        offset = 0;
        cardNum = 1;
    }
    public int getCardNum() {
        return cardNum;
    }
    public void addToHand(DeckManager.Card c) {
        GameObject currentCard = translateCard(c);
        currentCard.transform.position = pos;
        currentCard.GetComponentInChildren<TMP_Text>().text = c.getPower().ToString();
        currentCard.GetComponent<Renderer>().sortingOrder = (cardNum * 2);
        currentCard.transform.GetChild(0).GetComponent<Renderer>().sortingOrder = (cardNum * 2 + 1);
        currentCard.transform.SetParent(this.transform);
        if(cardNum % 2 == 0 && cardNum != 0) {
            offset = offset + ((cardNum) * gapSize);
        } else {
            offset = offset + ((cardNum) * -gapSize);
        }
        pos = new Vector3(offset, this.transform.position.y, this.transform.position.z);
        cardNum += 1;
    }
    GameObject translateCard(DeckManager.Card c) {
        if(c.getType() == 1) {
            return (GameObject)Instantiate(Resources.Load("Cards/Chalice"));
        }
        else if(c.getType() == 2) {
            return (GameObject)Instantiate(Resources.Load("Cards/Sword"));
        }
        else if(c.getType() == 3) {
            return (GameObject)Instantiate(Resources.Load("Cards/Wand"));
        }
        else {
            return (GameObject)Instantiate(Resources.Load("Cards/Star"));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandManager : MonoBehaviour
{
    Vector3 pos;  
    Vector3 handPos;
    float offset;
    float handOffset;
    int cardNum;
    public float gapSize;
    public int shownCards;
    DeckManager DeckManager;
    AudioSource sound;
    void Awake() {
        DeckManager = this.transform.parent.GetComponentInChildren<DeckManager>();
        pos = this.transform.position;
        handPos = this.transform.position;
        offset = 0f;
        handOffset = 0f;
        cardNum = 0;
        shownCards = 0;
        sound = this.gameObject.transform.GetComponentInChildren<AudioSource>();
    }
    public int getCardNum() {
        return cardNum;
    }
    public void addToHand(DeckManager.Card c) {
        Debug.Log("CardNum: " + cardNum);
        //card setup
        GameObject currentCard = translateCard(c);
        //card positioning
        currentCard.transform.position = pos;
        gameObject.transform.position = handPos;
        currentCard.GetComponentInChildren<TMP_Text>().text = c.getPower().ToString();
        //order sort
        currentCard.transform.GetChild(0).GetComponent<Renderer>().sortingOrder = (cardNum * 2);
        currentCard.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().sortingOrder = (cardNum * 2 + 1);
        currentCard.transform.SetParent(this.transform);
        //ID Provider
        currentCard.GetComponent<DragCard>().c = c;
        //transform
        shownCards++;
        cardNum = checkCardNum();
        offsetDetermind();
        //updates poisitoning
        pos = new Vector3(offset, this.transform.position.y, this.transform.position.z);
        handPos = new Vector3(handOffset, this.transform.position.y, this.transform.position.z);
        sound.Play();
    }
    public int checkCardNum() {
        for(int i = 0; i < DeckManager.d.getHandSize() - 1; i++) {
            if(!DeckManager.d.handCheck(i)) {
                cardNum = (i);
                DeckManager.d.replaceCardWithLatest(i);
                return i;
            }
        }
        return cardNum += 1;
    }
    GameObject translateCard(DeckManager.Card c) {
        if(c.getType() == 1) {
            Debug.Log("Chalice");
            return (GameObject)Instantiate(Resources.Load("Cards/Chalice"));
        }
        else if(c.getType() == 2) {
            Debug.Log("Sword");
            return (GameObject)Instantiate(Resources.Load("Cards/Sword"));
        }
        else if(c.getType() == 3) {
            Debug.Log("Wand");
            return (GameObject)Instantiate(Resources.Load("Cards/Wand"));
        }
        else {
            Debug.Log("Star");
            return (GameObject)Instantiate(Resources.Load("Cards/Star"));
        }
    }
    void offsetDetermind() {
        /* Antiquated wierd method
        if(cardNum % 2 == 0) {
            offset = (((cardNum + 1)/ 2) * -gapSize);
        } else {
            offset = (((cardNum + 1)/ 2) * gapSize);
        } */
        offset += (gapSize - 1.5f);
        handOffset -= (gapSize / 2f);
    }
}

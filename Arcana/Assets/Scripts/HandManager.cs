using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HandManager : MonoBehaviour
{
    Vector3 pos;  
    Vector3 handPos;
    float offset;
    public float handOffset;
    public int cardNum;
    public float gapSize;
    public int shownCards;
    DeckManager DeckManager;
    AudioSource sound;
    public bool cardsCanBeClicked;
    public bool finishedInitial;
    Enemy enemy;
    public void setEnemy(Enemy e) {
        enemy = e;
    }
    void Awake() {
        DeckManager = this.transform.parent.GetComponentInChildren<DeckManager>();
        pos = this.transform.position;
        handPos = this.transform.position;
        offset = 0f;
        handOffset = 0f;
        cardNum = 0;
        shownCards = 0;
        sound = this.gameObject.transform.GetComponentInChildren<AudioSource>();
        cardsCanBeClicked = false;
        finishedInitial = false;    
    }
    public int getCardNum() {
        return cardNum;
    }
    public void addToHand(DeckManager.Card c) {
        if (enemy != null) {
            if(enemy.getHealth() <= 0) {
                return;
            }
        }
        if(finishedInitial) {
            DeckManager.d.replaceCardWithLatest(cardNum);
        }
        Debug.Log("CardNum: " + cardNum);
        //card setup
        GameObject currentCard = translateCard(c);
        shownCards++;
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
        sound.Play();
        updatePosition();
    }
    public int checkCardNum() {
        for(int i = 0; i < DeckManager.handSize; i++) {
            if(!DeckManager.d.handCheck(i)) {
                cardNum = i;
                DeckManager.d.replaceCardWithLatest(i);
                return i;
                Debug.Log("FOUND ME");
            }
        }
        cardNum += 1;
        return cardNum;
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
    void offsetDetermind() {
        /* Antiquated wierd method
        if(cardNum % 2 == 0) {
            offset = (((cardNum + 1)/ 2) * -gapSize);
        } else {
            offset = (((cardNum + 1)/ 2) * gapSize);
        } */
        if(!finishedInitial) {
            offset = ((cardNum) * (gapSize / 2f));
            handOffset = (shownCards * gapSize * -1) / 2f;
        } else {
            offset = ((cardNum) * (gapSize)) - 6;
            handOffset = (shownCards * gapSize * -1) / 2f;
        }
        
    }
    public void updatePosition() {
        if(!finishedInitial) {checkCardNum(); }
        offsetDetermind();
        //updates poisitoning
        pos = new Vector3(offset, this.transform.position.y, this.transform.position.z);
        handPos = new Vector3(handOffset, this.transform.position.y, this.transform.position.z);
    }
    public void TurnEnd() {
        cardsCanBeClicked = false;
        
        StartCoroutine(waitASec(1f));
        
    }
    IEnumerator waitASec(float time) {
            yield return new WaitForSeconds(time);
            if(enemy == null) {
                yield break;
            }
            enemy.animationIsPlaying();
            enemy.playAttackAnimation();
            yield return new WaitForSeconds(time / 2f);
            enemy.takeTurn();
            yield return new WaitForSeconds(time / 2f);
            enemy.animationIsNotPlaying();
            cardsCanBeClicked = true;
            Debug.Log("Cards can be clicked");
        
    }
    void Update() {
        if (SceneManager.GetActiveScene().name != "BattleScene") {
            Destroy(this.gameObject);
        }
    }
}

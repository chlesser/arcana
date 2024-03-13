using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckManager : MonoBehaviour
{
    public int deckSize = 0;
    public int handSize = 5;
    HandManager HandManager;
    public class Deck{
        private List<Card> cards = new List<Card>();
        private List<Card> hand = new List<Card>();
        private List<Card> discard = new List<Card>();

        Player p;
        Enemy e;

        public void addCard(Card c) {
            cards.Add(c);
        }
        public void removeCard(Card c) {
            cards.Remove(c);
        }
        public override string ToString() {
            string s = "";
            foreach(Card c in cards) {
                s += c.getPower() + " " + c.getType() + "\n";
            }
            return s;
        }
        public void drawCard(HandManager H, int handSize) {
            if(cards.Count == 0) {
                reshuffle();
            }
            var rand = new System.Random();
            int i = rand.Next(cards.Count);
            Card c = cards[i];
            if(H.shownCards < handSize) {
                hand.Add(c);
                H.addToHand(c);
                cards.RemoveAt(i);

            }
        }
        public void reshuffle() {
            List<Card> temp = new List<Card>();
            foreach(Card c in discard) {
                cards.Add(c);
                temp.Add(c);
            }
            discard.Clear();
        }
        public void playCard(Card c, HandManager H) {
            int i = hand.IndexOf(c);
            hand[i].effect(e, p);
            if(hand.Count == 0) {
                return;
            }
            hand[i].playSound();
            discard.Add(new Card(c));
            hand[i].setPower(0);
            hand[i].setType(0);
            Debug.Log("eliminated position " + i);
            H.shownCards--;
            H.cardNum = i;
            H.updatePosition();
        }
        public void discardCard(Card c) {
            discard.Add(c);
        }
        public bool handCheck(int n) {
            if(n > hand.Count - 1) {
                return true;
            }
            if(hand[n].getType() == 0) {
                //this slot is empty
                return false;
            }
            return true;
        }
        public int getHandSize() {
            return hand.Count;
        }
        public void replaceCardWithLatest(int index) {
            hand[index] = hand[hand.Count - 1];
            hand.RemoveAt(hand.Count - 1);
        }
        public Card getCardInHand(int index) {
            return hand[index];
        }
        public void setEnemy(Enemy en) {
            e = en;
        }
        public void setPlayer(Player pl) {
            p = pl;
        }
        public void gameEnd() {
            foreach(Card c in hand) {
                discard.Add(c);
            }
            hand.Clear();
            foreach(Card c in discard) {
                cards.Add(c);
            }
            discard.Clear();
        }
        public void store() {
            List<List<int>> temp = new List<List<int>>();
            gameEnd();
            foreach(Card c in cards) {
                List<int> t = new List<int>();
                t.Add(c.getPower());
                t.Add(c.getType());
                temp.Add(t);
            }
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().save(temp);
        }
        public void load(List<List<int>> temp) {
            cards.Clear();
            foreach(List<int> t in temp) {
                if(t[0] != 0) {
                    Card c = new Card(t[0], t[1]);
                    cards.Add(c);
                    Debug.Log(c);
                }
            }
        }
        public int getDeckSize() {
            return cards.Count + hand.Count + discard.Count;
        }
    }
    public Deck d = new Deck();
    public class Card{
        private int power;
        private int type;
        private AudioSource sound;

        public Card(int p, int t) {
            power = p;
            type = t;
            sound = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        }
        public Card() {
            power = 15;
            type = 15;
            sound = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();;
        }
        public Card(int p, int t, AudioSource s) {
            power = p;
            type = t;
            sound = s;
        }
        public Card(Card c) {
            power = c.getPower();
            type = c.getType();
            sound = c.sound;
        }
        public override string ToString() {
            return power + " " + type;
        }
        public int getPower() {
            return power;
        }
        public int getType() {
            return type;
        }
        public void setPower(int p) {
            power = p;
        }
        public void setType(int t) {
            type = t;
        }
        public void playSound() {
            //play sound
            sound.Play();
        }
        public void setSound(AudioSource s) {
            sound = s;
        }  
        public void effect(Enemy e, Player p) {
            if(type == 1) {
                p.heal(power);
                Debug.Log("heal");
            }
            else if(type == 2) {
                e.takeDamage(power + p.getAttack());
                Debug.Log("hit");
            }
            else if(type == 3) {
                e.setAttack(e.getAttack() - power);
                Debug.Log("debuff");
            }
            else if(type == 4){
                p.setAttack(p.getAttack() + power);
                Debug.Log("buff");
            } 
        }
    }
    void Awake() {
        
        //Change to a list of enemies
        d.setEnemy(GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>());
        d.setPlayer(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>());
        //this section is temporary while card drawing is random
        if(this.transform.parent.GetComponent<GameManager>().first) {
            drawDeck();
            this.transform.parent.GetComponent<GameManager>().first = false;
        }
        else {
            d.load(this.transform.parent.GetComponent<GameManager>().storedDeck);
            deckSize = d.getDeckSize();
            StartCoroutine(handDraw());
        }
    }
    void drawDeck() {
        deckSize += 10;
        Card c = new Card(1, 2);
        d.addCard(c);
        c = new Card(2, 2);
        d.addCard(c);
        c = new Card(3, 2);
        d.addCard(c);
        c = new Card(1, 1);
        d.addCard(c);
        c = new Card(2, 1);
        d.addCard(c);
        c = new Card(3, 1);
        d.addCard(c);
        c = new Card(1, 3);
        d.addCard(c);
        c = new Card(2, 3);
        d.addCard(c);
        c = new Card(1, 4);
        d.addCard(c);
        c = new Card(2, 4);
        d.addCard(c);
        Debug.Log(d);
        StartCoroutine(handDraw());
    }
    IEnumerator handDraw() {
        yield return new WaitForSeconds(0.1f);
        HandManager = this.transform.parent.GetComponentInChildren<HandManager>();
        for(int i = 0; i < handSize; i++) {
            d.drawCard(HandManager, handSize);
            yield return new WaitForSeconds(1f);
        }
        HandManager.cardsCanBeClicked = true;
        HandManager.finishedInitial = true;
    }
    void Update() {
        if (SceneManager.GetActiveScene().name != "BattleScene") {
            Destroy(this.gameObject);
        }
    }
}

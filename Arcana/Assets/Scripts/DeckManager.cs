using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public int deckSize = 7;
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
            foreach(Card c in discard) {
                cards.Add(c);
                discard.Remove(c);
            }
        }
        public void playCard(Card c) {
            int i = hand.IndexOf(c);
            hand[i].effect(e, p);
            hand[i].playSound();
            discard.Add(c);
            hand[i].setPower(0);
            hand[i].setType(0);
        }
        public void discardCard(Card c) {
            discard.Add(c);
        }
        public bool handCheck(int n) {
            if(n > hand.Count - 1) {
                return true;
            }
            if(hand[n].getType() == 0) {
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
        
    }
    public Deck d = new Deck();
    public class Card{
        private int power;
        private int type;
        private AudioSource sound;

        public Card() {
            power = 0;
            type = 0;
        }
        public Card(int p, int t) {
            power = p;
            type = t;
            sound = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
        }
        public Card(int p, int t, AudioSource s) {
            power = p;
            type = t;
            sound = s;
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
                Debug.Log("Heal");
            }
            else if(type == 2) {
                e.takeDamage(power + p.getAttack());
                Debug.Log("Damage");
            }
            else if(type == 3) {
                e.setAttack(e.getAttack() - (power/2) + 1);
                Debug.Log("Debuff");
            }
            else {
                p.setAttack(p.getAttack() + (power/2) + 1);
                Debug.Log("Buff");
            } 
        }
    }
    void Awake() {
        HandManager = this.transform.parent.GetComponentInChildren<HandManager>();
        //Change to a list of enemies
        d.setEnemy(GameObject.FindGameObjectsWithTag("Enemy")[0].GetComponent<Enemy>());
        d.setPlayer(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>());
        //this section is temporary while card drawing is random
        var rand = new System.Random();

        for(int i = 0; i < deckSize; i++) {
            Card c = new Card(rand.Next(1, 10), rand.Next(1, 5));
            d.addCard(c);
        }
        StartCoroutine(handDraw());
        DontDestroyOnLoad(this.gameObject);
    }
    IEnumerator handDraw() {
        for(int i = 0; i < handSize; i++) {
            Debug.Log($"Started at {Time.time}, waiting for {1} seconds");
            d.drawCard(HandManager, handSize);
            yield return new WaitForSeconds(1f);
            Debug.Log($"Ended at {Time.time}"); 
        }
    }
}

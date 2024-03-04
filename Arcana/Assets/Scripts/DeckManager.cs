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
        private List<Card> discard = new List<Card>();

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
        public Card drawCard() {
            var rand = new System.Random();
            int i = rand.Next(cards.Count);
            Card c = cards[i];
            cards.RemoveAt(i);
            return c;
        }
        public void reshuffle() {
            foreach(Card c in discard) {
                cards.Add(c);
                discard.Remove(c);
            }
        }
        public void discardCard(Card c) {
            discard.Add(c);
        }
    }
    public class Card{
        private int power;
        private int type;

        public Card() {
            power = 0;
            type = 0;
        }
        public Card(int p, int t) {
            power = p;
            type = t;
        }
        public int getPower() {
            return power;
        }
        public int getType() {
            return type;
        }
    }
    void Awake() {
        HandManager = this.transform.parent.GetComponentInChildren<HandManager>();
        //this section is temporary while card drawing is random
        var rand = new System.Random();
        Deck d = new Deck();

        for(int i = 0; i < deckSize; i++) {
            Card c = new Card(rand.Next(1, 10), rand.Next(1, 5));
            d.addCard(c);
        }
        for(int i = 0; i < handSize; i++) {
            Card curr = d.drawCard();
            HandManager.addToHand(curr);
        }
    }
}

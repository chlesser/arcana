using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] FloatingHealthBar healthBar;
    private int maxHealth;
    private int currentHealth;
    private int attack;
    private int originalAttack;
    GameManager gameManager;
    int firstTimes = 0;

    public Player() {
        maxHealth = 10;
    }
    public Player(int h) {
        maxHealth = h;
    }
    public int getHealth() {
        return currentHealth;
    }
    public void setAttack(int p) {
        //if(firstTimes >= 1) {StartCoroutine(yellowFade(1f));} else {firstTimes++;}
        attack = p;
        StartCoroutine(yellowFade(1f));
    }
    public int getAttack() {
        return attack;
    }
    public int getMaxHealth() {
        return maxHealth;
    }
    public void setMaxHealth(int h) {
        maxHealth = h;
    }
    public void takeDamage(int d) {
        currentHealth -= d;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if (currentHealth <= 0) {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Card")) {
                Destroy(g);
            }
            gameManager.Lost();
        }
        Debug.Log("takeDamageTaken");
    }
    public void heal(int h) {
        currentHealth += h;
        StartCoroutine(greenFade(1f));
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }
    public void healToFull() {
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }
    public void die() {
        Destroy(gameObject);
    }
    public void attackPlayer(Enemy e) {
        e.takeDamage(1);
    }
    public void attackPlayer(Enemy e, int d) {
        e.takeDamage(d);
    }
    void Awake()
    {
        healthBar = this.gameObject.GetComponentInChildren<FloatingHealthBar>();
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        DontDestroyOnLoad(this.gameObject);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void OnEnable() {
        attack = 0;
    }
    IEnumerator greenFade(float time) {
        time = time / 2;
        for(float i = 1; i > 0; i -= Time.deltaTime / time) {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(i, 1, i, 1);
                yield return null;
        }
        for(float i = 0; i < 1; i += Time.deltaTime / time) {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(i, 1, i, 1);
                yield return null;
        }
        /*for(int i = 0; i < 5; i++) {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
            yield return new WaitForSeconds(time);
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(time);
        } */
    }
    IEnumerator yellowFade(float time) {
        time = time / 2;
        for(float i = 1; i > 0; i -= Time.deltaTime / time) {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, i, 1);
                yield return null;
        }
        for(float i = 0; i < 1; i += Time.deltaTime / time) {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, i, 1);
                yield return null;
        }
        /*for(int i = 0; i < 5; i++) {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
            yield return new WaitForSeconds(time);
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(time);
        } */
    }
}

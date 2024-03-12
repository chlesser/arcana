using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] FloatingHealthBar healthBar;
    private int maxHealth;
    private int currentHealth;
    private int attack;
    BasicEnemy basicEnemy;
    public GameManager GameManager;
    int firstTimes = 0;

    public Enemy() {
        maxHealth = 5;
        attack = 0;
    }
    public Enemy(int h) {
        maxHealth = h;
        attack = 0;
    }
    public Enemy(int h, int p) {
        maxHealth = h;
        attack = p;
    }
    public int getHealth() {
        return currentHealth;
    }
    public int getMaxHealth() {
        return maxHealth;
    }
    public void setAttack(int p) {
        
        if(firstTimes > 1) {StartCoroutine(purpleFade(0.1f));} else {firstTimes++;}
        if(p <= 1) {
            attack = 1;
        }
        else {
            attack = p;
        }
    }
    public int getAttack() {
        return attack;
    }
    public void setMaxHealth(int h) {
        maxHealth = h;
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }
    public void takeDamage(int d) {
        currentHealth -= d;
        if(currentHealth <= 0) {
            defeated();
        }
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        StartCoroutine(redFade(0.1f));
    }
    public void heal(int h) {
        currentHealth += h;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }
    public void die() {
        Destroy(gameObject);
    }
    public void attackPlayer(Player p) {
        p.takeDamage(attack);
    }
    public void attackPlayer(Player p, int d) {
        p.takeDamage(d);
    }
    void Awake()
    {
        healthBar = this.gameObject.GetComponentInChildren<FloatingHealthBar>();
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        basicEnemy = this.gameObject.GetComponent<BasicEnemy>();
        GameManager = GameObject.FindGameObjectWithTag("GameController").transform.GetComponentInChildren<GameManager>();
        StartCoroutine(waitHandOff());
    }
    public void takeTurn() {
        basicEnemy.play();
    }
    public void playAttackAnimation() {
        Debug.Log("There");
        this.gameObject.GetComponent<Animator>().Play("BasicEnemy");
    }
    public void animationIsPlaying() {
        this.gameObject.GetComponent<Animator>().SetBool("Playing", true);
    }
    public void animationIsNotPlaying() {
        this.gameObject.GetComponent<Animator>().SetBool("Playing", false);
    }
    IEnumerator redFade(float time) {
        for(int i = 0; i < 5; i++) {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
            yield return new WaitForSeconds(time);
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(time);
        }
    }
    IEnumerator purpleFade(float time) {
        for(int i = 0; i < 5; i++) {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 1, 1);
            yield return new WaitForSeconds(time);
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(time);
        }
    }
    void defeated() {
        GameManager.battleWin();
        Destroy(this.gameObject);
    }
    IEnumerator waitHandOff() {
        yield return new WaitForSeconds(0.2f);
        GameManager.GetComponentInChildren<HandManager>().setEnemy(this.gameObject.GetComponent<Enemy>());
    }
}

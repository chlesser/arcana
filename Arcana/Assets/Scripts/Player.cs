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
        attack = p;
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
        if (currentHealth > maxHealth) {
            maxHealth = currentHealth;
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
        attack = 1;
    }
}

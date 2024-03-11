using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] FloatingHealthBar healthBar;
    private int maxHealth;
    private int currentHealth;
    private int attack;

    public Enemy() {
        maxHealth = 5;
    }
    public Enemy(int h) {
        maxHealth = h;
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
        attack = p;
    }
    public int getAttack() {
        return attack;
    }
    public void setMaxHealth(int h) {
        maxHealth = h;
    }
    public void takeDamage(int d) {
        currentHealth -= d;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
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
        p.takeDamage(1);
    }
    public void attackPlayer(Player p, int d) {
        p.takeDamage(d);
    }
    void Awake()
    {
        healthBar = this.gameObject.GetComponentInChildren<FloatingHealthBar>();
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }
}

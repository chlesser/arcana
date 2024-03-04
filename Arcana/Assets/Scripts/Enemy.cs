using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int health;

    public Enemy() {
        health = 5;
    }
    public Enemy(int h) {
        health = h;
    }
    public int getHealth() {
        return health;
    }
    public void setHealth(int h) {
        health = h;
    }
    public void takeDamage(int d) {
        health -= d;
    }
    public void heal(int h) {
        health += h;
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
}

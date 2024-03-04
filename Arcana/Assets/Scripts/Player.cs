using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int health;

    public Player() {
        health = 10;
    }
    public Player(int h) {
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
    public void attackEnemy(Enemy e) {
        e.takeDamage(1);
    }
    public void attackEnemy(Enemy e, int d) {
        e.takeDamage(d);
    }
}

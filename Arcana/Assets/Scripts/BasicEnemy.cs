using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    Enemy enemy;
    Player player;
    public bool boss;
    public int attackNum = 0;
    void Awake()
    {
        enemy = this.gameObject.GetComponent<Enemy>();
        player = GameObject.Find("Player").GetComponent<Player>();
        if(!boss) {
            enemy.setMaxHealth(5);
            enemy.setAttack(1);
        }
        else {
            enemy.setMaxHealth(15);
            enemy.setAttack(2);
        }
    }
    public void play()
    {
        if(this.gameObject.name == "Boss(Clone)") {
            if(attackNum == 0) {
                enemy.attackPlayer(player);
                attackNum++;
            } else if(attackNum == 1) {
                enemy.heal(5);
                attackNum++;
            } else if(attackNum == 2) {
                enemy.setAttack(enemy.getAttack() + 2);
                attackNum = 0;
            }
        }
        else {
            Debug.Log("Enemy attack");
            enemy.attackPlayer(player);
        }

    }
    public void bossSound() {
        GameObject AudioManager = GameObject.FindGameObjectWithTag("Audio");
        if(attackNum == 0) {
            player.transform.GetChild(1).transform.GetChild(2).GetComponent<AudioSource>().Play();
        } else if(attackNum == 1) {
            AudioManager.transform.GetChild(3).GetComponent<AudioSource>().Play();
        } else {
            AudioManager.transform.GetChild(2).GetComponent<AudioSource>().Play();
        }
    }
}

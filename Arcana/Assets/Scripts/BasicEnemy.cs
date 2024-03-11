using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    Enemy enemy;
    Player player;
    void Awake()
    {
        enemy = this.gameObject.GetComponent<Enemy>();
        player = GameObject.Find("Player").GetComponent<Player>();
        enemy.setMaxHealth(5);
        enemy.setAttack(1);
    }
    public void play()
    {
        enemy.attackPlayer(player);
    }

}

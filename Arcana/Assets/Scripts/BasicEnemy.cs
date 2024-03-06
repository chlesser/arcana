using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    Enemy enemy;
    void Awake()
    {
        enemy = this.gameObject.GetComponent<Enemy>();
        enemy.setMaxHealth(20);
        Debug.Log("Enemy health: " + enemy.getHealth());
    }

}

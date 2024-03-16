using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] FloatingHealthBar healthBar;
    private int maxHealth;
    private int currentHealth;
    private int attack;
    public BasicEnemy basicEnemy;
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
        
        if(firstTimes > 1 && p < attack && this.gameObject.name != "Boss(Clone)")
        {StartCoroutine(purpleFade(0.1f));}
        else if(firstTimes > 2 && p > attack)
        {StartCoroutine(yellowFade(1f));}
        else if(firstTimes > 2 && p < attack && this.gameObject.name == "Boss(Clone)")
        {StartCoroutine(purpleFade(0.1f));}
        else
        {firstTimes++;}

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
            StartCoroutine(defeated());
        }
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        StartCoroutine(redFade(0.1f));
    }
    public void heal(int h) {
        currentHealth += h;
        StartCoroutine(greenFade(1f));
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
        StartCoroutine(waitHandOff(1f));
    }
    public void takeTurn() {
        basicEnemy.play();
    }
    public void playAttackAnimation() {
        if(this.gameObject.name == "Boss(Clone)") {
            if(basicEnemy.attackNum == 0) {
                this.gameObject.GetComponent<Animator>().Play("BossAttack");
            }
            return;
        }
        this.gameObject.GetComponent<Animator>().Play("BasicEnemy");
        if(this.gameObject.name == "Enemy 1(Clone)") {
        Debug.Log("Enemy 1(Clone)");
            GameObject bastard = (GameObject)Instantiate(Resources.Load("Enemies/Web"), new Vector3(4.5f, -0.75f, 0), Quaternion.identity);
            StartCoroutine(goKill(bastard));
        }
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
    IEnumerator defeated() {
        GameManager.battleWin();
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
    IEnumerator waitHandOff(float t) {
        yield return new WaitForSeconds(t);
        GameManager.GetComponentInChildren<HandManager>().setEnemy(this.gameObject.GetComponent<Enemy>());
    }
    IEnumerator goKill(GameObject g) {
        float timeFrame = 0.005f;
        yield return new WaitForSeconds(0.2f);
        for(float i = 4.5f; i > -5.5f; i -= 0.2f) {
            g.transform.position = new Vector3(i, -0.75f, 0);
            yield return new WaitForSeconds(timeFrame);
        }
        Destroy(g);
    }
}

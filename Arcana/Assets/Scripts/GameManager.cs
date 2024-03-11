using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    float enemyHealth = 0f;
    float enemydamage = 0f;
    int alternating = 0;
    Player player;
    public List<List<int>> storedDeck;
    public bool first = true;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        DontDestroyOnLoad(this.gameObject);
    }
    public void save(List<List<int>> deck) {
        storedDeck = deck;
    }
    public void Lost() {
        SceneManager.LoadScene("Loss");
    }
    public void nextNode() {
        enemyHealth += 2;
        if(alternating == 0) {
            enemydamage += 0;
            alternating = 1;
        } else {
            enemydamage += 1;
            alternating = 0;
        }
    }
    void onSceneLoaded() {
        if (SceneManager.GetActiveScene().name == "BattleScene") {
            first = false;
            this.gameObject.GetComponentInChildren<DeckManager>().load(storedDeck);
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(2).gameObject.SetActive(true);
            player.transform.parent.gameObject.SetActive(true);
            GameObject bastard = (GameObject)Instantiate(Resources.Load("Enemies/Enemy"));
            bastard.GetComponent<Enemy>().setMaxHealth((int)enemyHealth);
            bastard.GetComponent<Enemy>().setAttack((int)enemydamage);
            bastard.transform.position = new Vector3(6, 0, 0);
        } else {
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
            player.transform.parent.gameObject.SetActive(false);
        }
    }
    public void battleWin() {
        this.gameObject.transform.GetComponentInChildren<DeckManager>().store();
        SceneManager.LoadScene("MapScene");
    }
}

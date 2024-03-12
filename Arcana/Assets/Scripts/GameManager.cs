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
    int currSceneID = 0;
    public Dictionary<int, int> nodes = new Dictionary<int, int>();
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        player.gameObject.SetActive(false);
        DontDestroyOnLoad(this.gameObject);
        nodes.Add(0, 1);
    }
    public void save(List<List<int>> deck) {
        storedDeck = deck;
    }
    public void Lost() {
        SceneManager.LoadScene("Loss");
        Destroy(player.gameObject);
    }
    public int addNode(int id) {
        if (nodes.ContainsKey(id)) {
            return nodes[id];
        } else {
            nodes.Add(id, 0);
            return 0;
        }
    }
    public void nextNode() {
        enemyHealth += 1;
        if(alternating == 0) {
            enemydamage += 0;
            alternating = 1;
        } else {
            enemydamage += 1;
            alternating = 0;
        }
    }
    public void battle(int id) {
        nodes[id] = 2;
        this.gameObject.transform.GetComponentInChildren<nodeEnabler>().updateNodes(id);
        SceneManager.LoadScene("BattleScene");
    }
    public void map() {
        SceneManager.LoadScene("MapScene");
    }
    void OnSceneLoaded () {
        Debug.Log("SceneLoaded");
        if (SceneManager.GetActiveScene().name == "BattleScene") {
            //lock and load
            player.gameObject.SetActive(true);
            GameObject bastard = (GameObject)Instantiate(Resources.Load("Enemies/Enemy"), new Vector3(6, 0, 0), Quaternion.identity);
            putThemBack();
            //enemy and player
            bastard.GetComponent<Enemy>().setMaxHealth((int)enemyHealth);
            bastard.GetComponent<Enemy>().setAttack((int)enemydamage);
            bastard.transform.position = new Vector3(6, 0, 0);
        } else if (SceneManager.GetActiveScene().name == "Loss") {
            player.gameObject.SetActive(false);
        } 
        else {
            player.gameObject.SetActive(false);
        }
    }
    public void battleWin() {
        for(int i = 0; i < this.transform.GetChild(1).transform.childCount; i++) {
            Destroy(this.transform.GetChild(1).transform.GetChild(i).gameObject);
        }
        player.healToFull();
        this.gameObject.transform.GetComponentInChildren<DeckManager>().d.store();
        SceneManager.LoadScene("MapScene");
    }
    void putThemBack() {
        GameObject currDeck = (GameObject)Instantiate(Resources.Load("Managers/DeckManager"), new Vector3(0, 0, 0), Quaternion.identity, this.transform);
        currDeck.transform.SetSiblingIndex(0);
        StartCoroutine(handDelay());
    }
    void Update() {
        if (currSceneID != SceneManager.GetActiveScene().buildIndex) {
            Debug.Log(SceneManager.GetActiveScene().name);
            currSceneID = SceneManager.GetActiveScene().buildIndex;
            OnSceneLoaded();
        }
    }
    IEnumerator handDelay() {
        yield return new WaitForSeconds(0.05f);
        GameObject currHand = (GameObject)Instantiate(Resources.Load("Managers/HandManager"), new Vector3(0, -4, 0), Quaternion.identity, this.transform);
        currHand.transform.SetSiblingIndex(1);
    }
}

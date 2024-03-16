using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    public GameObject backs;
    public bool rewardScreen = false;
    public bool nodesClickable = true;
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
            //Audio!
            GameObject.FindGameObjectWithTag("Audio").GetComponent<MusicSelect>().battleScene();
            //lock and load
            player.gameObject.SetActive(true);
            var rand = new System.Random();
            int i = rand.Next(2);
            GameObject bastard;
            if(i == 1) { bastard = (GameObject)Instantiate(Resources.Load("Enemies/Enemy"), new Vector3(6, 0, 0), Quaternion.identity); } else
            { bastard = (GameObject)Instantiate(Resources.Load("Enemies/Enemy 1"), new Vector3(6, -1.5f, 0), Quaternion.identity); }
            putThemBack();
            if (this.gameObject.GetComponent<nodeEnabler>().currNode >= 6) {
                //activate final background & deactivate basic background
                backs.transform.GetChild(0).gameObject.SetActive(false);
                backs.transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color((float)this.gameObject.GetComponent<nodeEnabler>().currNode / 10 - 0.2f, (float)this.gameObject.GetComponent<nodeEnabler>().currNode / 10 - 0.2f, (float)this.gameObject.GetComponent<nodeEnabler>().currNode / 10 - 0.2f, 1);
                backs.transform.GetChild(1).gameObject.SetActive(true);
            } else {
                //activate basic background & deactivate final background
                backs.transform.GetChild(0).gameObject.SetActive(true);
                backs.transform.GetChild(1).gameObject.SetActive(false);
            }
            //enemy and player
            bastard.GetComponent<Enemy>().setMaxHealth((int)enemyHealth);
            bastard.GetComponent<Enemy>().setAttack((int)enemydamage);
            bastard.transform.position = new Vector3(6, 0, 0);
        } else if (SceneManager.GetActiveScene().name == "MapScene") {
            //audio
            GameObject.FindGameObjectWithTag("Audio").GetComponent<MusicSelect>().mapScene();
            //setup
            player.gameObject.SetActive(false);
            backs.transform.GetChild(0).gameObject.SetActive(false);
            backs.transform.GetChild(1).gameObject.SetActive(false);
            if (nodes[10] == 2) {
                SceneManager.LoadScene("Win");
            }
        }
        else {
            player.gameObject.SetActive(false);
            backs.transform.GetChild(0).gameObject.SetActive(false);
            backs.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    public void battleWin() {
        for(int i = 0; i < this.transform.GetChild(1).transform.childCount; i++) {
            Destroy(this.transform.GetChild(1).transform.GetChild(i).gameObject);
        }
        StartCoroutine(victoryScreen());
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
    IEnumerator victoryScreen() {
        List<int> newCard = nodeParse();
        Debug.Log("New Card: " + newCard[0] + " " + newCard[1]);
        GameObject winUI = GameObject.FindGameObjectWithTag("WinScreen");
        winUI.GetComponentInChildren<TMP_Text>().text = "Reward";
        winUI.GetComponentInChildren<Renderer>().sortingLayerID = SortingLayer.NameToID("UI");
        //GameObject.FindGameObjectWithTag("Audio").transform.GetChild(0).GetComponent<AudioSource>().Play();
        GameObject reward = (GameObject)Instantiate(Resources.Load("Cards/" + typeTranslate(newCard[1])), new Vector3(0, 0, 0), Quaternion.identity);
        reward.GetComponentInChildren<TMP_Text>().text = newCard[0].ToString();
        //how long the end screen is for
        rewardScreen = true;
        while(rewardScreen) {
            yield return new WaitForSeconds(0.1f);
        }
            Destroy(reward);
            winUI.GetComponentInChildren<Renderer>().sortingLayerID = SortingLayer.NameToID("Default");
            player.healToFull();
            this.gameObject.transform.GetComponentInChildren<DeckManager>().deckSize += 1;
            this.gameObject.transform.GetComponentInChildren<DeckManager>().d.store();
            storedDeck.Add(newCard);
            GameObject.FindGameObjectWithTag("Audio").transform.GetChild(0).GetComponent<AudioSource>().Play();
            SceneManager.LoadScene("MapScene");
    }
    List<int> nodeParse() {
        int temp = this.gameObject.transform.GetComponentInChildren<nodeEnabler>().getNodeReward();
        int power = 0;
        int type = 0;
        power = temp / 10;
        type = temp % 10;
        List<int> newCard = new List<int>();
        newCard.Add(power);
        newCard.Add(type);
        return newCard;
    }
    string typeTranslate(int type) {
        if(type == 1) {
            return "Chalice";
        }
        else if(type == 2) {
            return "Sword";
        }
        else if(type == 3) {
            return "Wand";
        }
        else {
            return "Star";
        }
    }
    public void holdOn() {
        StartCoroutine(hold());
    }
    IEnumerator hold() {
        yield return new WaitForSeconds(1f);
    }
    public void fadeOut() {
        StartCoroutine(fade(false, 1));
    }
    public void fadeIn() {
        StartCoroutine(fade(true, 1));
    }
    
    IEnumerator fade(bool goingIn, float time) {
        if(goingIn) {
            for(float i = 0; i < 1; i += Time.deltaTime / time) {
                foreach(GameObject child in UnityEngine.Object.FindObjectsOfType<GameObject>()) {
                    if(child.transform.GetComponent<SpriteRenderer>() != null && child.activeInHierarchy) {
                        child.transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i);
                    }
                }
                yield return null;
            }
        } else {
            for(float i = 1; i > 0; i -= Time.deltaTime / time) {
                foreach(GameObject child in UnityEngine.Object.FindObjectsOfType<GameObject>()) {
                    if(child.transform.GetComponent<SpriteRenderer>() != null && child.activeInHierarchy) {
                        child.transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i);
                    }
                }
                yield return null;
            }
        }
    }
}
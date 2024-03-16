using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicSelect : MonoBehaviour
{
    bool map = false;
    bool battle = false;
    bool start = true;

    public void mapScene() {
        map = true;
        battle = false;
        start = false;
    }
    public void battleScene() {
        map = false;
        battle = true;
        start = false;
    }
    public void startScene() {
        map = false;
        battle = false;
        start = true;
    }
    void playOrStop() {
        if(map) {
            if(!GameObject.Find("MapMusic").GetComponent<AudioSource>().isPlaying) {
                GameObject.Find("MapMusic").GetComponent<AudioSource>().Play();
            }
            if(GameObject.Find("BattleMusic").GetComponent<AudioSource>().isPlaying) {
                GameObject.Find("BattleMusic").GetComponent<AudioSource>().Stop();
            }
            if(GameObject.Find("StartMusic").GetComponent<AudioSource>().isPlaying) {
                GameObject.Find("StartMusic").GetComponent<AudioSource>().Stop();
            }
        }
        if(battle) {
            if(!GameObject.Find("BattleMusic").GetComponent<AudioSource>().isPlaying) {
                GameObject.Find("BattleMusic").GetComponent<AudioSource>().Play();
            }
            if(GameObject.Find("MapMusic").GetComponent<AudioSource>().isPlaying) {
                GameObject.Find("MapMusic").GetComponent<AudioSource>().Stop();
            }
            if(GameObject.Find("StartMusic").GetComponent<AudioSource>().isPlaying) {
                GameObject.Find("StartMusic").GetComponent<AudioSource>().Stop();
            }
        }
        if(start) {
            if(!GameObject.Find("StartMusic").GetComponent<AudioSource>().isPlaying) {
                GameObject.Find("StartMusic").GetComponent<AudioSource>().Play();
            }
            if(GameObject.Find("MapMusic").GetComponent<AudioSource>().isPlaying) {
                GameObject.Find("MapMusic").GetComponent<AudioSource>().Stop();
            }
            if(GameObject.Find("BattleMusic").GetComponent<AudioSource>().isPlaying) {
                GameObject.Find("BattleMusic").GetComponent<AudioSource>().Stop();
            }
    }
    }
    void Update() {
        playOrStop();
    }
}

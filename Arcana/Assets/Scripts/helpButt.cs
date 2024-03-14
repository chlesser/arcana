using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class helpButt : MonoBehaviour
{
    public void off() {
        this.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }
    public void on() {
        this.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
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
                this.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, i);
                yield return null;
            }
        } else {
            for(float i = 1; i > 0; i -= Time.deltaTime / time) {
                this.gameObject.GetComponent<Image>().color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }
    void Awake() {
        off();
    }
}

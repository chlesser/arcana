using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    public void UpdateHealthBar(int health, int maxHealth)
    {
        if(slider == null){
            slider = this.gameObject.GetComponent<Slider>();
        }
        slider.value = (float)health / (float)maxHealth;
    }
    void Start()
    {
        slider = this.gameObject.GetComponent<Slider>();
        camera = Camera.main;
        target = this.transform.parent.parent.transform;
        offset = new Vector3(0, -1.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(camera == null){
            camera = Camera.main;
        }
        transform.rotation = camera.transform.rotation;
        transform.position = target.position + offset;
    }
}

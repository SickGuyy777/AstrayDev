using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sliderscrpt : MonoBehaviour
{
    public float hp = 10;
    public Slider hps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hp>=0)
        hps.value = hp/10;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            hp = hp - (1*Time.deltaTime);
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            hp = hp - (1*Time.deltaTime);
    }
}

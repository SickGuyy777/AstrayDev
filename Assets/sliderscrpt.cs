using UnityEngine;
using UnityEngine.UI;

public class sliderscrpt : MonoBehaviour
{
    public float hp = 10;
    public Slider hps;

    
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

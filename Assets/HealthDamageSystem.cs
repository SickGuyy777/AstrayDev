using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDamageSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public static float hp=10f;
   
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            hp = hp - 1;
    }
}

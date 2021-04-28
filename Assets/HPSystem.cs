using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSystem : MonoBehaviour
{
    public Slider HPBar;
    public static int HP = 10;
    // Start is called before the first frame update
    void Start()
    {
        HealthDamageSystem.hp = HP;

    }

    // Update is called once per frame
    void Update()
    {
        

        HPBar.value = HealthDamageSystem.hp/10; 
    }
    
}

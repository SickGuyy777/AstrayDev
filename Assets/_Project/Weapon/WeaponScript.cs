using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponScript : MonoBehaviour
{
    public bool isshooting;
    public bool Isprimary = false;
    public bool Issecondary = false;
    [SerializeField] private GameObject Location;
    [SerializeField] private GameObject bullet;
    int bulletnumber=30;
    public Image Rifle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Input1();
        rifle();
    }
    public void rifle()
    {
       

        if (Rifle.sprite.name == "3241bedbdd6626c4eccdda210df375f6_14")
        {
            Debug.Log("image yes");
            if ( isshooting==true && bulletnumber > 0)
            {
                Debug.Log("entered");
                Invoke("shoot", 0.2f);
                bulletnumber--;
            }

        }
    }
    public void Input1()
    {
        {
            /*
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Isprimary = true;
                Issecondary = false;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Isprimary = false;
                Issecondary = true;
            }*/
        }
        //if (Isprimary == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isshooting = true;
                
            }
            if (Input.GetMouseButtonUp(0))
            {
                isshooting = false;
            }
        }
    }
    
    public void shoot()
    {
            Instantiate(bullet, Location.transform.position, Location.transform.rotation);
    }
   
}


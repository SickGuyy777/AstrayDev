using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
    [SerializeField] private Image targetGraphic;
    [SerializeField] private HealthManager healthManager;
    
    [Header("Options")] 
    [SerializeField] private float minHeartRate = 1;
    [SerializeField] private float maxHeartRate = 3;
    [SerializeField] private float blinkRate;
    
    [SerializeField] private Color hurtColor = Color.red;
    [SerializeField] private Color deadColor = Color.grey;
    
    
    //private SoundCreater soundCreator;
    private Animator anim;
    private Color originalColor;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        //soundCreator = GetComponent<SoundCreator>();
        originalColor = targetGraphic.color;

        UpdateHealthRate();
        healthManager.OnHealthChanged += UpdateHealthRate;
    }

    private void UpdateHealthRate()
    {
        if (healthManager.Health > 0)
        {
            float heartRate = Mathf.Lerp(minHeartRate, maxHeartRate, healthManager.ReversedHealthPercentage);
            targetGraphic.color = Color.Lerp(originalColor, hurtColor, healthManager.ReversedHealthPercentage);
            anim.SetFloat("Speed", heartRate);
        }
        else
        {
            targetGraphic.color = deadColor;
            anim.SetFloat("Speed", 0);
        }
    }
    
    //Called In Animation
    public void Beat()
    {
        //soundCreator.PlaySound("Beat");
    }
}

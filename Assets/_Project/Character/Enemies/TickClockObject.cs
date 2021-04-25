using UnityEngine;

public class TickClockObject : MonoBehaviour
{
    public static TickClockObject Instance;
    
    [SerializeField] private float rate = 1;


    private void Awake()
    {
        #region Singleton

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        
        #endregion
        
        InvokeRepeating(nameof(Tick), rate, rate);
    }

    private void Tick() => TickClock.Tick();
}

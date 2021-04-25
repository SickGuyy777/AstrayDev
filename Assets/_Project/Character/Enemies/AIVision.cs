using System;
using UnityEngine;

public abstract class AIVision : MonoBehaviour
{
    public event Action<PlayerController> OnTargetFound = delegate { };
    
    public PlayerController Target { get; private set; }

    protected abstract PlayerController DetectPlayer();

    private void OnEnable() => TickClock.OnTick += UpdateTarget;

    private void OnDisable() => TickClock.OnTick -= UpdateTarget;

    private void OnDestroy() => TickClock.OnTick -= UpdateTarget;

    private void UpdateTarget()
    {
        PlayerController player = DetectPlayer();
        Target = player;
        
        if(player != null)
            OnTargetFound.Invoke(player);
    }
}

using System;

public static class TickClock
{
    public static int CurrentTick { get; private set; }
    public static event Action OnTick = delegate { };

    
    public static void Tick()
    {
        OnTick.Invoke();
        CurrentTick++;
    }
}

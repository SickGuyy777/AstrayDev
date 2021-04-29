using System;

public static class TickClock
{
    public static event Action OnTick = delegate { };

    
    public static void Tick() => OnTick.Invoke();
}

using System;

public abstract class Functionality
{
    public Action Function => Execute;
    
    public abstract void Execute();
}

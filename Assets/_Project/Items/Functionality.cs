using System;

public abstract class Functionality
{
    public Action Function => Execute;
    
    protected abstract void Execute();
}

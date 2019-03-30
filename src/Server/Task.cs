using System;

/// <summary>
/// Task class for execute some action in certain time
/// </summary>
public class Task
{
    public enum Operations { TURNON, TURNOFF };

    public Operations Operation;
    public Plug Device;

    public Task(Operations op, Plug dev)
	{
		//need to set properties 
	}

    public void Execute()
    {
        //when entering this function we need to execute the task
    }
}

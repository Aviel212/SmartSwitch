using System;

/// <summary>
/// A task that repeats itself in a certain time
/// </summary>
public class RepeatedTask : Task
{
    public DateTime StartDate;
    public int RepeatEvery;

	public RepeatedTask()
	{
		
	}
}

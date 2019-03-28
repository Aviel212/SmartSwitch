using System;

/// <summary>
/// A task that is un-repeatable (one-time-task)
/// </summary>
public class OneTimeTask : Task
{
    public DateTime DateToBeExecuted;

	public OneTimeTask()
	{
		
	}
}

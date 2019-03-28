using System;

/// <summary>
/// A current and voltage sample of a given device at a given time
/// </summary>
public class PowerUsageSample
{
    public readonly double Current;
    public readonly DateTime SampleDate;
    public readonly double Voltage;

	public PowerUsageSample(double c, DateTime dt, double v)
	{
        Current = c;
        SampleDate = dt;
        Voltage = v;
	}

    public double GetWattage()
    {
        return Current * Voltage;
    }
}

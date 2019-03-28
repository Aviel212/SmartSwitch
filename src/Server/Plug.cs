using System;

/// <summary>
/// this class representing a smart device which you can turn on and off and use simple tasks on it
/// </summary>
public class Plug
{
    enum Priorities {ESSENTIAL, NONESSENTIAL, IRRELEVANT};

    public readonly string Mac;
    public string NickName;
    public bool IsOn;
    public bool Approved;
    public Priorities Priority;

    public Plug(string mac)
	{
        Mac = mac;
        IsOn = false;
        Approved = false;
        Priority = Priorities.IRRELEVANT;
	}

    public void TurnOn()
    {
        //turn on the device
    }

    public void TurnOff()
    {
        //turn off the device
    }

    public void addSample(PowerUsageSample pus)
    {
        //add a new sample to the list of samples
    }

}

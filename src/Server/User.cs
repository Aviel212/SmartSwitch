using System;

/// <summary>
/// User can add new plugs to his acctive plugs, remove plugs and more
/// </summary>
public class User
{
    public string UserName;
    private string Password;
    public List<Plug> Plugs;

	public User(string name, string pass)
	{
        UserName = name;
        Password = pass;
	}

    public void AddPlug(Plug p)
    {
        //add new plug 
    }

    public Lazy<Plug> GetUnapprovedPlugs()
    {
        //need to return all unapproved Plugs
    }

    public void RemovePlug(Plug p)
    {
        //need to remove certain device
    }

    public Plug GetPlug(string mac)
    {
        //need to return certain plug
    }
}

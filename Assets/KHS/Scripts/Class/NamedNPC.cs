using UnityEngine;

public class NamedNPC : IName
{
    private string name;
    public NamedNPC(string name) { this.name = name; }
    public string GetName() => name;
}

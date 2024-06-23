using UnityEngine;

[CreateAssetMenu]
public class AgentPrompts : ScriptableObject
{
    public string Name;
    [TextArea]
    public string Description;
    [TextArea]
    public string Instructions;
}

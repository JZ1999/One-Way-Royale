using UnityEngine;

[CreateAssetMenu(fileName = "Debuff", menuName = "Debuff/Debuff", order = 3)]
public class DebuffData : ScriptableObject
{
    [InspectorName("name")]
    public string nameDebuff;

    public string description;
    public int costOfAplication;
    public int playersEffected;
}
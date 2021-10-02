using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Debuff", menuName = "Debuff/Debuff", order = 3)]
public class DebuffData : ScriptableObject
{
    [InspectorName("namse")]
    public string nameDebuff;

    public string description;
    public int costOfAplication;
    public int playersEffected;
    public Sprite logo;
}
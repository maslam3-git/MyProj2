
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewCrystalData",
    menuName = "Lava Rush/Crystal Data"
)]
public class CrystalData : ScriptableObject
{
    public string crystalName;
    public Color crystalColor = Color.cyan;
    public int points = 5;
}

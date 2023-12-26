using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Inventory Item")]
public class ItemData : ScriptableObject
{
    public Sprite Icon;
    public string DisplayName;
    [TextArea(4,4)]
    public string DisplayDescription;

    //dimensions for the grid
    public int Width;
    public int Height;
}

using System.Collections.Generic;
using UnityEngine;
using PnCCasualGameKit;

/// <summary>
/// ScriptableObject.
/// List of skin items. 
/// </summary>
[CreateAssetMenu(fileName = "Skins List", menuName = "Stack the Blocks/Skins List", order = 1)]
public class SkinList : ScriptableObject
{
    public List<SkinItemData> skins;
}

/// <summary>
/// This is the data class for skin. Extends ItemData. 
/// </summary>
[System.Serializable]
public class SkinItemData:ShopItemData
{
    public Texture texture;
}

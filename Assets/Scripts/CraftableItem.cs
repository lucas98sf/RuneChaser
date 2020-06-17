using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Craftable Item", menuName  = "Craftable Item")]
public class CraftableItem : ScriptableObject //baseado no tutorial de scriptable objects do canal Brackeys
{
    public GameObject prefab;
    public bool needWorkbench;
    public bool needFurnace;
    public bool needAnvil;
    public int quantCrafted = 1;
    public Vector3 position = new Vector3(0,0.2f,0);
    [System.Serializable]
    public struct needed {
     public int quant;
     public string item;
    }
    public needed[] NeededItems;

}
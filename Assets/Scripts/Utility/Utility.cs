
using UnityEngine;
public class Utility
{
    public static int LayerNameToBitMap(string layerName)
    {
        return 1 << LayerMask.NameToLayer(layerName);
    }
}

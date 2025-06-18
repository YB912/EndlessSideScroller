
using UnityEngine;
public class Utility
{
    public static int LayerNameToBitMap(string layerName)
    {
        return 1 << LayerMask.NameToLayer(layerName);
    }

    public static Vector2 RotateVector(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        );
    }
}

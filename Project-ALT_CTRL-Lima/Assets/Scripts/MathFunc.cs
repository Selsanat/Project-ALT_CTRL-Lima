using UnityEngine;

public static class MathFunc
{
    public static bool EqualFloats(float a, float b, float tollerance)
    {
        return Mathf.Abs(a - b) <= tollerance;
    }

    public static bool EqualQuaternions(Quaternion a, Quaternion b, float tollerance)
    {
        return EqualFloats(a.x, b.x, tollerance) && EqualFloats(a.y, b.y, tollerance) && EqualFloats(a.z, b.z, tollerance) && EqualFloats(a.w, b.w, tollerance);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GeneralMath
{
    public static uint GetPermutation(uint number)
    {
        uint result = 1;
        for (uint i = 1; i <= number; i++)
        {
            result *= i;
        }

        return result;
    }

    public static uint GetNumberOfCombination(uint set, uint subSet)
    {
        if (subSet > set)
            return 0;

        return (uint)GetCombination(set, subSet);
    }

    private static float GetCombination(float n, float r)
    {
        if (r > 0)
        {
            return (n / r) * GetCombination(n - 1, r - 1);
        }
        else
            return 1;
    }

    public static Vector3 GetMidPoint(Vector3 vec1, Vector3 vec2)
    {
        return (vec1 + vec2) / 2;
    }

    public static Vector3 Lerp(Vector3 fromVec, Vector3 toVec, float time)
    {
        time = Mathf.Clamp01(time);
        return fromVec + (toVec - fromVec) * time;
    }
}

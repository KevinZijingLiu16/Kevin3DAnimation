using UnityEngine;

public static class NoiseSystem
{
    public static event System.Action<Vector3, GameObject> OnNoiseEmitted;

    public static void EmitNoise(Vector3 position, GameObject source)
    {
        OnNoiseEmitted?.Invoke(position, source);
    }
}

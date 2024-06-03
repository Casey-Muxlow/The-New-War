#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public enum ReflectionProbeResolution
{
    Resolution16 = 16,
    Resolution32 = 32,
    Resolution64 = 64,
    Resolution128 = 128,
    Resolution256 = 256,
    Resolution512 = 512,
    Resolution1024 = 1024,
    Resolution2048 = 2048
}

[System.Serializable]
public class LightGroupController : MonoBehaviour
{
    [Header("[Light Control]")]
    public bool useLightsEnabled = true;
    public bool lightsEnabled = true;

    [Header("[Light Intensity Control]")]
    public bool useLightIntensity = false;
    public float lightIntensity = 1f;

    [Header("[Shadows Control]")]
    public bool useShadowControl = false;
    public bool enableShadows = true;

    [Header("[Light Color Control]")]
    public bool useLightColor = false;
    public Color lightColor = Color.white;

    [Header("[Light Range Control]")]
    public bool useLightRange = false;
    public float lightRange = 10f;

    [Header("[Reflection Probe Control]")]
    public bool useProbeControl = false;
    public bool enableReflectionProbes = true;
    public ReflectionProbeResolution reflectionProbeResolution = ReflectionProbeResolution.Resolution32;

    [Header("[Light Probe Control]")]
    public bool useLightProbeControl = false;
    public bool enableLightProbes = true;

    private void SetLightsState(Transform parent, bool enabled)
    {
        Light[] lights = parent.GetComponentsInChildren<Light>();

        foreach (Light light in lights)
        {
            if (useLightsEnabled)
            {
                light.enabled = lightsEnabled && enabled;
            }
            else
            {
                light.enabled = enabled;
            }

            if (enabled && light.enabled)
            {
                if (useLightIntensity)
                {
                    light.intensity = lightIntensity;
                }

                if (useShadowControl)
                {
                    light.shadows = enableShadows ? LightShadows.Soft : LightShadows.None;
                }

                if (useLightColor)
                {
                    light.color = lightColor;
                }

                if (useLightRange)
                {
                    light.range = lightRange;
                }
            }
        }
    }

    private void SetProbeState(bool state)
    {
        ReflectionProbe[] reflectionProbes = GetComponentsInChildren<ReflectionProbe>();
        foreach (ReflectionProbe probe in reflectionProbes)
        {
            probe.enabled = state && enableReflectionProbes;
            probe.resolution = (int)reflectionProbeResolution;
        }

        LightProbeGroup[] lightProbeGroups = GetComponentsInChildren<LightProbeGroup>();
        foreach (LightProbeGroup group in lightProbeGroups)
        {
            group.enabled = state && enableLightProbes;
        }
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        SetLightsState(transform, true);
        SetProbeState(true);
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        SetProbeState(false);
#endif
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        SetLightsState(transform, true);
        SetProbeState(enableReflectionProbes || enableLightProbes);
#endif
    }
}

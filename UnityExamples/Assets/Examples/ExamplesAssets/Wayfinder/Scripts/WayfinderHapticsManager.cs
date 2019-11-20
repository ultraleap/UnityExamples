using System.Collections;
using UnityEngine;
using UltrahapticsCoreAsset;

public class WayfinderHapticsManager : MonoBehaviour
{
    public SensationSource notchHaptic;

    private IEnumerator StartRunningAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        notchHaptic.Running = true;
    }

    // Note: Longer term this should move into the core UCA behaviour
    public void MuteForDuration(float seconds)
    {
        if (seconds < 0)
        {
            UCA.Logger.LogWarning("Playback Duration cannot be negative");
            return;
        }

        notchHaptic.Running = false;
        StartCoroutine(StartRunningAfterSeconds(seconds));
    }

    public void PlayNotchForDuration(float duration)
    {
        MuteForDuration(duration);
    }
}

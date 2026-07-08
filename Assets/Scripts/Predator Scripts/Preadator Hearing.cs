using UnityEngine;

public class PreadatorHearing : MonoBehaviour
{
    public Opponent opp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Noise Object"))
        {
            // Resolve the noise position from the NoiseItem on the same object or its parent.
            NoiseItem item = other.GetComponentInParent<NoiseItem>();
            Vector3 noisePosition = item != null ? item.transform.position : other.transform.position;

            opp.HearNoise(noisePosition);
        }
    }
}

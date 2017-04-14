using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float shakeDecay = 0.3f;
    [SerializeField]
    private float shakeIntensityStart = 0.3f;

    private Vector3 originPosition;
    private Vector3 originRotation;

    private void Start()
    {
        originPosition = Vector3.zero + Vector3.back * 10f;
        originRotation = Quaternion.identity.eulerAngles;
    }

    public IEnumerator Shake()
    {
        float shakeIntensity = shakeIntensityStart;

        while (shakeIntensity > 0f)
        {
            transform.localPosition = originPosition + Random.insideUnitSphere * shakeIntensity;

            transform.localRotation = Quaternion.Euler(
                originRotation.x + Random.Range(-shakeIntensity, shakeIntensity),
                originRotation.y + Random.Range(-shakeIntensity, shakeIntensity),
                originRotation.z + Random.Range(-shakeIntensity, shakeIntensity)
            );

            shakeIntensity -= shakeDecay * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = originPosition;
        transform.localRotation = Quaternion.Euler(originRotation);
    }
}
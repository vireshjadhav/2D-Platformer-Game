using System.Collections;
using UnityEngine;

public class CameraShakeController: MonoBehaviour
{
    
    [Header("CameraShake")]
    [SerializeField] public float duration = 0f;
    [SerializeField] public float magnitude = 1f;

    [Header("CameraMovement")]
    [SerializeField] private float cameraMoveUnit = 4f;
    [SerializeField] private float speed  = 5f;
    [SerializeField] private float returnSpeed = 10f;
    private Vector3 originalLocalPos;
    private Vector3 targetLocalPos;
    private float currentSpeed;


    public static CameraShakeController instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        originalLocalPos = transform.localPosition;
        targetLocalPos = originalLocalPos;
    }

    private void Update()
    {
        HandleCameraUpDown();

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPos, currentSpeed * Time.deltaTime);
    }

    private void HandleCameraUpDown()
    {
        if (Input.GetKey(KeyCode.W))
        {
            targetLocalPos = originalLocalPos + Vector3.up * cameraMoveUnit;
            currentSpeed = speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            targetLocalPos = originalLocalPos + Vector3.down * cameraMoveUnit;
            currentSpeed = speed;
        }
        else
        {
            targetLocalPos = originalLocalPos;
            currentSpeed = returnSpeed;
        }

    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }

    public void TriggerShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }
}

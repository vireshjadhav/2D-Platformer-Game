using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("FollowTarget")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private float followSmooth = 0.0125f;    
    
    [Header("CameraShake")]
    [SerializeField] public float duration = 0.2f;
    [SerializeField] public float magnitude = 0.2f;
    private Vector3 shakeOffset = Vector3.zero;
    private Coroutine shakeRoutine;

    [Header("CameraMovement")]
    [SerializeField] private float cameraMoveUnit = 4f;
    [SerializeField] private float speed  = 5f;
    [SerializeField] private float returnSpeed = 10f;
    private Vector3 originalLocalPos;
    private Vector3 targetLocalPos;
    private float currentSpeed;


    public static CameraController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        originalLocalPos = transform.localPosition;
        targetLocalPos = originalLocalPos;
    }

    private void LateUpdate()
    {
        HandleCameraUpDown();
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (target ==  null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 extraOffset  = targetLocalPos - originalLocalPos;
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition + extraOffset, followSmooth);

        transform.position = smoothed + shakeOffset;

    }

    //private void Update()
    //{
    //    HandleCameraUpDown();

    //    transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPos, currentSpeed * Time.deltaTime);
    //}

    private void HandleCameraUpDown()
    {
        if (Input.GetKey(KeyCode.W))
        {
            targetLocalPos = originalLocalPos + Vector3.up * cameraMoveUnit;
            currentSpeed = speed;

            Debug.Log("UpSpeed: " + currentSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            targetLocalPos = originalLocalPos + Vector3.down * cameraMoveUnit;
            currentSpeed = speed;

            Debug.Log("DownSpeed: " + currentSpeed);
        }
        else
        {
            targetLocalPos = originalLocalPos;
            currentSpeed = returnSpeed;

            //Debug.Log("ReturnSpeed: " + currentSpeed);
        }

        targetLocalPos = Vector3.Lerp(targetLocalPos, targetLocalPos, currentSpeed * Time.deltaTime);

    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        //Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            shakeOffset = new Vector3(x, y, 0f);

            //transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        shakeOffset = Vector3.zero;
    }

    public void TriggerShake(float duration, float magnitude)
    {
        if (shakeRoutine != null) StopCoroutine(shakeRoutine);
        shakeRoutine = StartCoroutine(Shake(duration, magnitude));
    }

    public void SetTarget (Transform newTarget)
    {
        target = newTarget;
    }
}

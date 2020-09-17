using UnityEngine;
public class CameraController : MonoBehaviour
{
    [Header("Boundaries")]
    [SerializeField] private float minZoomDistance = 4f;
    [SerializeField] private float maxZoomDistance = 12.5f;
    [SerializeField] private float maxXPos = 10f;
    [SerializeField] private float maxYPos = 10.5f;

    [Header("Speed")]
    [SerializeField] private float cameraPanSpeed = 12f;
    [SerializeField] private float cameraZoomSpeed = 12f;
    [SerializeField] private float cameraRotateSpeedDegrees = 180f;

    private Camera controlledCamera;
    private int targetCameraAngle;

    public bool MovementEnabled { get; set; } = true;
    public bool ZoomEnabled { get; set; } = true;
    public bool RotationEnabled { get; set; } = true;

    private void Awake()
    {
        controlledCamera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (MovementEnabled) PanCamera();
        if (ZoomEnabled) ZoomCamera();
        if (RotationEnabled) RotateCamera();
    }

    private void PanCamera()
    {
        float verticalMod = Input.GetAxisRaw("Vertical");
        float horizontalMod = Input.GetAxisRaw("Horizontal");
        if (verticalMod == 0 && horizontalMod == 0) return;

        Vector3 horizontalMove = transform.right *
            horizontalMod * cameraPanSpeed * Time.deltaTime;
        Vector3 verticalMove = transform.up *
            verticalMod * cameraPanSpeed * Time.deltaTime;
        Vector3 moveVector = Vector3.ClampMagnitude(
            horizontalMove + verticalMove, cameraPanSpeed);
        Vector3 newPosUnclamped = transform.position + moveVector;
        transform.position = new Vector3(
            Mathf.Clamp(newPosUnclamped.x, -maxXPos, maxXPos),
            Mathf.Clamp(newPosUnclamped.y, -maxYPos, maxYPos),
            transform.position.z);
    }

    private void ZoomCamera()
    {
        float mouseWheelDelta = -Input.mouseScrollDelta.y;
        float newZoom = Mathf.Clamp(controlledCamera.orthographicSize +
            mouseWheelDelta * cameraZoomSpeed * Time.deltaTime,
            minZoomDistance, maxZoomDistance);
        controlledCamera.orthographicSize = newZoom;
    }

    private void RotateCamera()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            targetCameraAngle = GetAnglePositive(targetCameraAngle - 90);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            targetCameraAngle = (targetCameraAngle + 90) % 360;
        }
        float currentAngle = GetAnglePositive(transform.rotation.eulerAngles.z);
        float angleDifference = targetCameraAngle - currentAngle;
        float angleDifferenceAbs = Mathf.Abs(angleDifference);
        int rotationMod = 0;
        if (angleDifference > 0)
        {
            rotationMod = angleDifferenceAbs >= 180 ? -1 : 1;
        }
        else if (angleDifference < 0)
        {
            rotationMod = angleDifferenceAbs >= 180 ? 1 : -1;
        }
        float angleDelta = cameraRotateSpeedDegrees * rotationMod * Time.deltaTime;
        float newAngle = Mathf.Abs(angleDelta) >= angleDifferenceAbs ?
            targetCameraAngle : currentAngle + angleDelta;
        transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }

    private float GetAnglePositive(float angle)
    {
        if (angle >= 0) return angle;
        return 360 + (angle % -360);
    }

    private int GetAnglePositive(int angle)
    {
        if (angle >= 0) return angle;
        return 360 + (angle % -360);
    }
}

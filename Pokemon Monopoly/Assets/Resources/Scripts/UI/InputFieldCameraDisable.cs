using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class InputFieldCameraDisable : MonoBehaviour
{
    [SerializeField] private CameraController camControls;

    private InputField input;

    private void Awake()
    {
        input = GetComponent<InputField>();
    }

    private void Update()
    {
        camControls.enabled = !input.isFocused;
    }
}

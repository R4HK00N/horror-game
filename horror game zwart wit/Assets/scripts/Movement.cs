using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float runSpeed = 12f;
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float gravity = -20f;
    [SerializeField] Slider staminaSlider;
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float runDepleteRate = 20f;
    [SerializeField] float regenRate = 10f;

    CharacterController controller;
    Vector3 velocity;
    bool isGrounded;
    float currentStamina;
    bool isRunning;

    float xRotation = 0f;
    float yRotation = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        currentStamina = maxStamina;
        if (staminaSlider)
        {
            staminaSlider.maxValue = 1f;
            staminaSlider.value = 1f;
        }
    }

    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);


        isRunning = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0f;
        if (isRunning)
        {
            currentStamina -= runDepleteRate * Time.deltaTime;
            if (currentStamina < 0f) currentStamina = 0f;
        }
        else if (currentStamina < maxStamina)
        {
            currentStamina += regenRate * Time.deltaTime;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
        }
        if (staminaSlider) staminaSlider.value = currentStamina / maxStamina;


        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;


        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        move.y = 0f;
        float speed = isRunning ? runSpeed : walkSpeed;
        move = move.normalized * speed;

        controller.Move(move * Time.deltaTime + velocity * Time.deltaTime);
    }
}
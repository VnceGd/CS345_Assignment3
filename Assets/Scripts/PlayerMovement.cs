using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float cameraSensitivity = 1.0f;
    public GameObject topDownCamera;
    public GameObject playerCamera;
    public bool topDown;

    private Rigidbody playerBody;
    private bool cursorLocked;

    // Start is called before the first frame update
    public void Start()
    {
        playerBody = GetComponent<Rigidbody>();

        topDown = true;
        topDownCamera.SetActive(true);
        playerCamera.SetActive(false);

        cursorLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move_force = Vector3.zero;

        if (topDown)
        {
            if (Mathf.Abs(h) > 0.0f)
            {
                transform.forward = Vector3.right * h;
                move_force += Vector3.right * Time.deltaTime * h * moveSpeed;
            }
            if (Mathf.Abs(v) > 0.0f)
            {
                transform.forward = Vector3.forward * v;
                move_force += Vector3.forward * Time.deltaTime * v * moveSpeed;
            }
            playerBody.MovePosition(transform.position + move_force);


        }
        else
        {
            float mouse_x = Input.GetAxis("Mouse X");
            float mouse_y = Input.GetAxis("Mouse Y");

            if (Mathf.Abs(h) > 0.0f)
            {
                move_force += transform.right * Time.deltaTime * h * moveSpeed;
            }
            if (Mathf.Abs(v) > 0.0f)
            {
                move_force += transform.forward * Time.deltaTime * v * moveSpeed;
            }
            playerBody.MovePosition(transform.position + move_force);

            if (Mathf.Abs(mouse_x) > 0.0f)
            {
                transform.Rotate(Vector3.up * Time.deltaTime * mouse_x * cameraSensitivity);
            }
            if (Mathf.Abs(mouse_y) > 0.0f)
            {
                playerCamera.transform.Rotate(Vector3.left * Time.deltaTime * mouse_y * cameraSensitivity);
            }
            if (playerCamera.transform.localEulerAngles.z > 0.0f || playerCamera.transform.localEulerAngles.z < 0.0f)
            {
                playerCamera.transform.localEulerAngles = new Vector3(playerCamera.transform.localEulerAngles.x,
                                                                      playerCamera.transform.localEulerAngles.y,
                                                                      0.0f);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TogglePerspective();
        }

        // Lock or unlock cursor
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (cursorLocked)
            {
                cursorLocked = false;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                cursorLocked = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    // Toggle between top down and first person
    public void TogglePerspective()
    {
        if (topDown)
        {
            topDown = false;
            topDownCamera.SetActive(false);
            playerCamera.SetActive(true);
        }
        else
        {
            topDown = true;
            topDownCamera.SetActive(true);
            playerCamera.SetActive(false);
        }
    }
}

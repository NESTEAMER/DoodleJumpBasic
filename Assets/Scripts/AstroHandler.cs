using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class AstroHandler : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera vCam;
    private float zoomSpeed = 10.0f;
    private bool zoomIn = false;

    private Vector3 startPos;
    private Vector3 currentPos;

    public TextMeshProUGUI scoreText;
    private float score = 0f;

    private float touchStart;
    private float touchEnd;

    public float moveSpeed = 12f;
    private float moveX;


    // Start is called before the first frame update
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // Update is called once per frame
    void Update()
    {
        if (zoomIn && vCam.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize >= 1.0f)
        {
            vCam.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize -= zoomSpeed * Time.deltaTime;
        }
        else if (!zoomIn && vCam.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize <= 5.0f)
        {
            vCam.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize += zoomSpeed * Time.deltaTime;
        }

        score = transform.position.y;
        if(score < 0)
        {
            score = 0;
        }
        else
        {
            scoreText.text = Mathf.RoundToInt(score).ToString();
        }
        //Debug.Log(score);

        var accelRead = Input.acceleration.x;
        scoreText.text = accelRead.ToString();
        var newX = Mathf.Lerp(moveX, accelRead * moveSpeed, 30 * Time.deltaTime);
        moveX = newX;
    }

    private void FixedUpdate()
    {
        Vector2 velocity = GetComponent<Rigidbody2D>().velocity;
        velocity.x = moveX;
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    public void OnDoubleTapZoom(InputAction.CallbackContext ctx)
    {
        if(ctx.phase == InputActionPhase.Performed)
        {
            if(zoomIn)
            {
                zoomIn = false;
            }
            else
            {
                zoomIn = true;
            }
        }
    }

    private void OnBecameInvisible()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void OnStartSwipe(InputAction.CallbackContext ctx)
    {
        startPos = ctx.ReadValue<Vector2>();
        touchStart = Time.time;
    }

    public void OnSwipe(InputAction.CallbackContext ctx)
    {
        currentPos = ctx.ReadValue<Vector2>();
    }

    public void OnReleaseSwipe(InputAction.CallbackContext ctx)
    {
        if(ctx.phase == InputActionPhase.Canceled)
        {
            touchEnd = Time.time;
            float swipeDuration = touchEnd - touchStart;
            float forceMultiplier = CalculateForceMultiplier(swipeDuration);

            var distance = startPos - transform.position;
            var jumpVector = currentPos - distance;
            GetComponent<Rigidbody2D>().AddForce(jumpVector / 10.0f * 5f);
        }
    }

    float CalculateForceMultiplier(float duration)
    {
        float minDuration = 0.1f;
        float maxDuration = 0.5f;
        float minMultiplier = 0.5f;
        float maxMultiplier = 2.0f;

        duration = Mathf.Clamp(duration, minDuration, maxDuration);
        float multiplier = Mathf.Lerp(maxMultiplier, minMultiplier, (duration - minDuration) / (maxDuration - minDuration));

        return multiplier;
    }

}

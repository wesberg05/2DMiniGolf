using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private GameObject goalFX;
    [SerializeField] private GameObject WaterFX;
     [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip waterSplashSound;
    [SerializeField] private AudioClip puttSound;
    [SerializeField] private AudioClip levelComplete;


    [Header("Attributes")]
    [SerializeField] private float maxPower = 10f;
    [SerializeField] private float power = 2f;
    [SerializeField] private float maxGoalSpeed = 4f;

    private bool isDragging;
    private bool inHole;
    private Vector2 lastVelocity; 

    private void Update()
    {
        PlayerInput();

        if (LevelManager.main.outOfStrokes && rb.velocity.magnitude <= 0.2f && !LevelManager.main.levelCompleted)
        {
            LevelManager.main.gameOver();
            isDragging = false;
        }
    }

    private void FixedUpdate()
    {
        lastVelocity = rb.velocity;
    }

    private bool isReady()
    {
        return rb.velocity.magnitude <= 0.2f;
    }

    private void PlayerInput()
    {
        if (!isReady())
        {
            return;
        }

        Vector2 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(transform.position, inputPos);

        if (Input.GetMouseButtonDown(0) && distance <= 0.5f) DragStart();
        if (Input.GetMouseButton(0) && isDragging) DragChange(inputPos);
        if (Input.GetMouseButtonUp(0) && isDragging) DragRelease(inputPos);
    }

    private void DragStart()
    {
        isDragging = true;
        lr.positionCount = 2;
    }

    private void DragChange(Vector2 pos)
    {
        Vector2 dir = (Vector2)transform.position - pos;

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, (Vector2)transform.position + Vector2.ClampMagnitude((dir * power) / 2, maxPower / 2));
    }

    private void DragRelease(Vector2 pos)
    {
        float distance = Vector2.Distance((Vector2)transform.position, pos);
        isDragging = false;
        lr.positionCount = 0;

        if (distance < 0.5f)
        {
            return;
        }

        LevelManager.main.IncreaseStroke();
        AudioSource.PlayClipAtPoint(puttSound, transform.position);
        Vector2 dir = (Vector2)transform.position - pos;
        rb.velocity = Vector2.ClampMagnitude(dir * power, maxPower);
    }

    private void CheckWinState()
    {
        if (inHole) return;

        if (rb.velocity.magnitude <= maxGoalSpeed)
        {
            inHole = true;

            rb.velocity = Vector2.zero;
            gameObject.SetActive(false);

            GameObject fx = Instantiate(goalFX, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
            AudioSource.PlayClipAtPoint(levelComplete, transform.position);
            LevelManager.main.LevelComplete();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Goal"))
    {
        CheckWinState();
    }
    else if (other.CompareTag("Water"))
    {
        
        AudioSource.PlayClipAtPoint(waterSplashSound, transform.position);
     
        GameObject wfx = Instantiate(WaterFX, transform.position, Quaternion.identity);
        LevelManager.main.gameOver();
        
        Destroy(gameObject);
        Destroy(wfx, 2f);
    }
}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Goal") CheckWinState();
    }

    private void OnCollisionEnter2D(Collision2D collision)
{
    Vector2 normal = collision.contacts[0].normal;
    Vector2 reflectedVelocity = Vector2.Reflect(lastVelocity, normal);

    float bounceFactor = 0.6f;
    rb.velocity = reflectedVelocity * bounceFactor;
}

}
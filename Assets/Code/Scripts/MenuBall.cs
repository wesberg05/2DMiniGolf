using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBall : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private GameObject goalFX;
    [SerializeField] private GameObject waterFX;
    [SerializeField] private AudioClip levelComplete;
    [SerializeField] private AudioClip puttSound;

    [Header("Attributes")]
    [SerializeField] private float maxPower = 10f;
    [SerializeField] private float power = 2f;
    private bool isDragging;
    private bool inHole;

    private void Update()
    {
        PlayerInput();
    }

    private void PlayerInput()
    {
        if (inHole) return;

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
        isDragging = false;
        lr.positionCount = 0;

        float distance = Vector2.Distance((Vector2)transform.position, pos);
        if (distance < 1f) return;

        AudioSource.PlayClipAtPoint(puttSound, transform.position);
        Vector2 dir = (Vector2)transform.position - pos;
        rb.velocity = Vector2.ClampMagnitude(dir * power, maxPower);
    }

    private void CheckWinState()
    {
        if (inHole) return;

        inHole = true;
        rb.velocity = Vector2.zero;
        gameObject.SetActive(false);

        AudioSource.PlayClipAtPoint(levelComplete, transform.position);
        GameObject fx = Instantiate(goalFX, transform.position, Quaternion.identity);
        Destroy(fx, 2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Level"))
        {
            CheckWinState();
        }
    }
}

using UnityEngine;
using DG.Tweening;

public class BubbleMovement : MonoBehaviour
{
    public GameObject[] bubbles;  // Array to store the bubbles
    public float speed = 5f;      // Movement speed
    public float spacing = 0.1f;  // Space between bubbles in the horde

    private bool isMousePressed = false;

    private void Start()
    {
        for (int i = 0; i < bubbles.Length; i++)
        {
            bubbles[i].transform.DOShakePosition(0.5f, 0.1f, 1, 90f, false, true);
        }
    }
    void Update()
    {
        // Detect if the mouse is pressed
        if (Input.GetMouseButton(0))  // 0 is the left mouse button
        {
            isMousePressed = true;
        }
        else
        {
            isMousePressed = false;
        }

        // If the mouse is pressed, move the bubbles in the horde direction
        if (isMousePressed)
        {
            MoveBubbles();
        }
    }

    void MoveBubbles()
    {
        // Get the mouse position in screen space and convert it to world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;  // Ensure the bubbles stay in 2D (no movement in the z-axis)

        // Calculate the direction (left or right) based on the mouse position
        Vector3 direction = (mousePos.x < transform.position.x) ? Vector3.left : Vector3.right;

        // Move each bubble in the direction of the mouse, considering the spacing
        for (int i = 0; i < bubbles.Length; i++)
        {
            // Move each bubble with the spacing taken into account
            Vector3 targetPosition = bubbles[i].transform.position + direction * spacing * i;
            bubbles[i].transform.position = Vector3.MoveTowards(bubbles[i].transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}

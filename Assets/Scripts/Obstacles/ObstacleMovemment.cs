using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovemment : MonoBehaviour
{
    public float Speed { private get; set; }
    private Camera mainCamera;
    private void Start()
    {
        mainCamera = Camera.main;
       // CheckChildrenBoundsX();
    }
    void Update()
    {
        transform.Translate(Vector3.down * Speed * Time.deltaTime);
    }

    private void CheckChildrenBoundsX()
    {
        // Lista para armazenar os filhos que precisam ser reposicionados
        List<Transform> childrenToReposition = new List<Transform>();

        foreach (Transform child in transform) // Itera pelos filhos do transform deste script
        {
            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(child.position);

            if (viewportPosition.x < 0 || viewportPosition.x > 1)
            {
                childrenToReposition.Add(child);
            }
        }

        foreach (Transform child in childrenToReposition)
        {
            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(child.position);
            if (viewportPosition.x < 0)
            {
                child.position = mainCamera.ViewportToWorldPoint(new Vector3(0.95f, viewportPosition.y, mainCamera.nearClipPlane));
            }
            else if (viewportPosition.x > 1)
            {
                child.position = mainCamera.ViewportToWorldPoint(new Vector3(0.05f, viewportPosition.y, mainCamera.nearClipPlane));
            }
        }
    }
}

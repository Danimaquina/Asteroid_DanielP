using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreenWrapper : MonoBehaviour
{
    private Renderer objectRenderer;
    private Camera mainCamera;
    private bool isWrappingX = false;
    private bool isWrappingY = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        ScreenWrapObject();
    }

    void ScreenWrapObject()
    {
        // Verifica si el objeto es visible en la cámara
        if (objectRenderer.isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }
        
        // Si el objeto ya ha sido envuelto en ambos ejes no hagas nada
        if (isWrappingX && isWrappingY)
        {
            return;
        }

        // Convierte la posición del objeto en coordenadas del viewport
        // Las coordenadas del viewport van de (0, 0) a (1, 1)
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        
        // Almacena la nueva posición del objeto
        Vector3 newPosition = transform.position;

        // Aplica el wrapping en el eje X si el objeto sale por los lados
        if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            // Se invierte la posición en el eje X para que aparezca en el lado opuesto
            newPosition.x = -newPosition.x;
            isWrappingX = true;
        }

        // Aplica el wrapping en el eje Y si el objeto sale por arriba o abajo
        if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            // Se invierte la posición en el eje Y para que aparezca en el lado opuesto
            newPosition.y = -newPosition.y;
            isWrappingY = true;
        }

        transform.position = newPosition;
    }
}
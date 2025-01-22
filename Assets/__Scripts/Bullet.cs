using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 5f; // Velocidad del objeto
    private Vector3 moveDirection; // Dirección del movimiento

    void Start()
    {
        // Obtener la posición del ratón en el mundo
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)
        );

        // Calcular la dirección inicial hacia el ratón
        moveDirection = (mouseWorldPosition - transform.position).normalized;
    }

    void Update()
    {
        // Mover el objeto en la dirección calculada al inicio
        transform.Translate(moveDirection * bulletSpeed * Time.deltaTime, Space.World);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class PlayerShip : MonoBehaviour
{
    public float moveSpeed = 10f; // Velocidad de movimiento
    public GameObject bulletPrefab;
    public Transform bulletAnchor;   // Referencia al objeto vacío donde se crearán las balas
    private Rigidbody rb; 




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento hacia adelante y hacia atrás
        float verticalMoveInput = CrossPlatformInputManager.GetAxis("Vertical"); // W/S o Flecha Arriba/Abajo o táctil
        float horizontalMoveInput = CrossPlatformInputManager.GetAxis("Horizontal"); // A/D o Flecha Izquierda/Derecha o táctil

        // Calcular la dirección del movimiento
        Vector2 movement = new Vector2(horizontalMoveInput, verticalMoveInput);

        // Aplicar la velocidad al Rigidbody2D
        rb.velocity = movement * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            
            bullet.transform.SetParent(bulletAnchor);

        }

        
    }
    
    
}

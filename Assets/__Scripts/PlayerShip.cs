using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class PlayerShip : MonoBehaviour
{
    public float moveSpeed = 10f; // Velocidad de movimiento
    public GameObject bulletPrefab;
    public Transform bulletAnchor;   // Referencia al objeto vacío donde se crearán las balas



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento hacia adelante y hacia atrás
        float VerticalmoveInput = CrossPlatformInputManager.GetAxis("Vertical"); // W/S o Flecha Arriba/Abajo o táctil
        transform.Translate(Vector3.up * VerticalmoveInput * moveSpeed * Time.deltaTime);
        
        float HorizontalmoveInput = CrossPlatformInputManager.GetAxis("Horizontal"); // A/D o Flecha Izquierda/Derecha o táctil
        transform.Translate(Vector3.right * HorizontalmoveInput * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            
            bullet.transform.SetParent(bulletAnchor);

        }

        
    }
    
    
}

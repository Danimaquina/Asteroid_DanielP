using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Obtén la posición del ratón en el mundo
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        // Para asegurarse de que las posiciones estén en el mismo plano 
        mouseWorldPosition.z = transform.position.z;

        transform.LookAt(mouseWorldPosition, Vector3.forward);
    }
}

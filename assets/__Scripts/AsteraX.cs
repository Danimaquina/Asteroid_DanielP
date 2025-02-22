using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteraX : MonoBehaviour
{
    public ScriptableObject AsteroidsSO;
    public Asteroid[] asteroids;
    public int numeroAsteroids;
   
    // Start is called before the first frame update
    void Start()
    {
        asteoridSpawn(numeroAsteroids);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void asteoridSpawn(int number)
    {
        for (int i = 0; i < number; i++)
        {
            // Obtener una posición aleatoria dentro de los límites de la pantalla
            Vector3 randomPosition = ScreenBounds.RANDOM_ON_SCREEN_LOC;

            // Instanciar un asteroide en la posición aleatoria
            Instantiate(asteroids[Random.Range(0, asteroids.Length)], randomPosition, Quaternion.identity);
        }
    }
    
}

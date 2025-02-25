using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteraX : MonoBehaviour
{
    public ScriptableObject AsteroidsSO;
    public int numeroAsteroids;
    public GameObject[] asteroids;
   
    // Start is called before the first frame update
    void Start()
    {
        asteroids = new GameObject[numeroAsteroids];
        asteoridSpawn(numeroAsteroids);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void asteoridSpawn(int number)
    {
        GameObject prefab = AsteroidsScriptableObject.S.GetAsteroidPrefab();
        
        for (int i = 0; i < number; i++)
        {
            // Obtener una posición aleatoria dentro de los límites de la pantalla
            Vector3 randomPosition = ScreenBounds.RANDOM_ON_SCREEN_LOC;

            // Instanciar un asteroide en la posición aleatoria
            GameObject asteroid = Instantiate(prefab, randomPosition, Quaternion.identity);
            
            asteroid.name = "Asteroid_0" + i;
            
            asteroids[i] = asteroid;
        }
    }
    
}

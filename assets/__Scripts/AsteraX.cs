// Definición opcional para activar logs de depuración
//#define DEBUG_AsteraX_LogMethods

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteraX : MonoBehaviour
{
    // Instancia Singleton privada. Se accede a través de la propiedad estática S
    static private AsteraX _S;

    // Listas estáticas para almacenar asteroides y balas en el juego
    static List<Asteroid> ASTEROIDS;
    static List<Bullet> BULLETS;
    
    // Distancia mínima de los asteroides con respecto a la nave del jugador
    const float MIN_ASTEROID_DIST_FROM_PLAYER_SHIP = 5;

    // Enumeración para definir los diferentes estados del juego
    // Se usa System.Flags para permitir combinaciones de valores en el Inspector
    [System.Flags]
    public enum eGameState
    {
        none = 0,       // Sin estado
        mainMenu = 1,   // Menú principal
        preLevel = 2,   // Antes de iniciar el nivel
        level = 4,      // Durante el nivel
        postLevel = 8,  // Después de completar el nivel
        gameOver = 16,  // Fin del juego
        all = 0xFFFFFFF // Todos los estados posibles
    }

    [Header("Configuración en el Inspector")]
    [Tooltip("Este objeto define las configuraciones de los asteroides.")]
    public AsteroidsScriptableObject asteroidsSO;

    private void Awake()
    {
#if DEBUG_AsteraX_LogMethods
        Debug.Log("AsteraX:Awake()");
#endif

        S = this; // Asigna la instancia del Singleton
    }

    void Start()
    {
#if DEBUG_AsteraX_LogMethods
        Debug.Log("AsteraX:Start()");
#endif

        ASTEROIDS = new List<Asteroid>();
        
        // Genera 3 asteroides principales
        for (int i = 0; i < 3; i++)
        {
            SpawnParentAsteroid(i);
        }
    }

    // Método para generar un asteroide principal
    void SpawnParentAsteroid(int i)
    {
#if DEBUG_AsteraX_LogMethods
        Debug.Log("AsteraX:SpawnParentAsteroid(" + i + ")");
#endif

        Asteroid ast = Asteroid.SpawnAsteroid(); // Crea una nueva instancia de asteroide
        ast.gameObject.name = "Asteroid_" + i.ToString("00");
        
        // Busca una posición adecuada para el asteroide, evitando que esté demasiado cerca del jugador
        Vector3 pos;
        do
        {
            pos = ScreenBounds.RANDOM_ON_SCREEN_LOC;
        } while ((pos - PlayerShip.POSITION).magnitude < MIN_ASTEROID_DIST_FROM_PLAYER_SHIP);

        ast.transform.position = pos;
        ast.size = asteroidsSO.initialSize; // Asigna el tamaño inicial del asteroide
    }

    // ---------------- Sección Estática ---------------- //

    // Propiedad estática para acceder al Singleton con protección adicional
    static private AsteraX S
    {
        get
        {
            if (_S == null)
            {
                Debug.LogError("AsteraX:S getter - Intento de acceder a S antes de que esté inicializado.");
                return null;
            }
            return _S;
        }
        set
        {
            if (_S != null)
            {
                Debug.LogError("AsteraX:S setter - Intento de sobrescribir S cuando ya está asignado.");
            }
            _S = value;
        }
    }

    // Propiedad estática para acceder a la configuración de los asteroides
    static public AsteroidsScriptableObject AsteroidsSO
    {
        get
        {
            if (S != null)
            {
                return S.asteroidsSO;
            }
            return null;
        }
    }
    
    // Agrega un asteroide a la lista si no está ya presente
    static public void AddAsteroid(Asteroid asteroid)
    {
        if (ASTEROIDS.IndexOf(asteroid) == -1)
        {
            ASTEROIDS.Add(asteroid);
        }
    }

    // Elimina un asteroide de la lista si está presente
    static public void RemoveAsteroid(Asteroid asteroid)
    {
        if (ASTEROIDS.IndexOf(asteroid) != -1)
        {
            ASTEROIDS.Remove(asteroid);
        }
    }
}

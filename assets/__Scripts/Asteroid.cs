// Estas definiciones de depuración se usaron para probar casos en los que algunos asteroides 
// se perdían fuera de la pantalla.
//#define DEBUG_Asteroid_TestOOBVel 
//#define DEBUG_Asteroid_ShotOffscreenDebugLines

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if DEBUG_Asteroid_TestOOBVel
using UnityEditor;
#endif

// Requiere que el objeto tenga un Rigidbody y un OffScreenWrapper
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OffScreenWrapper))]
public class Asteroid : MonoBehaviour
{
    [Header("Configuración Dinámica")]
    public int size = 3; // Tamaño del asteroide
    public bool immune = false; // Indica si el asteroide es inmune a colisiones

    Rigidbody rigid; // Referencia al Rigidbody
    OffScreenWrapper offScreenWrapper; // Referencia al componente que maneja los límites de pantalla

#if DEBUG_Asteroid_ShotOffscreenDebugLines
    [Header("Depuración de disparos fuera de pantalla")]
    bool trackOffscreen;
    Vector3 trackOffscreenOrigin;
#endif

    private PlayerShip playerShip;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        offScreenWrapper = GetComponent<OffScreenWrapper>();
        
    }

    void Start()
    {
        playerShip = PlayerShip.S;

        AsteraX.AddAsteroid(this); // Agrega el asteroide a la lista global

        transform.localScale = Vector3.one * size * AsteraX.AsteroidsSO.asteroidScale;
        if (parentIsAsteroid)
        {
            InitAsteroidChild();
        }
        else
        {
            InitAsteroidParent();
        }

        // Genera asteroides hijos si el tamaño es mayor que 1
        if (size > 1)
        {
            Asteroid ast;
            for (int i = 0; i < AsteraX.AsteroidsSO.numSmallerAsteroidsToSpawn; i++)
            {
                ast = SpawnAsteroid();
                ast.size = size - 1;
                ast.transform.SetParent(transform);
                Vector3 relPos = Random.onUnitSphere / 2;
                ast.transform.rotation = Random.rotation;
                ast.transform.localPosition = relPos;
                ast.gameObject.name = gameObject.name + "_" + i.ToString("00");
            }
        }
    }

    private void OnDestroy()
    {
        AsteraX.RemoveAsteroid(this);
    }

    // Inicializa un asteroide padre con movimiento y colisión activados
    public void InitAsteroidParent()
    {
#if DEBUG_Asteroid_ShotOffscreenDebugLines
        Debug.LogWarning(gameObject.name + " InitAsteroidParent() " + Time.time);
#endif
        tag = "Asteroid";
        offScreenWrapper.enabled = true;
        rigid.isKinematic = false;
        
        // Asegura que el asteroide esté en el plano z=0
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;
        
        InitVelocity(); // Asigna una velocidad inicial al asteroide
    }

    // Inicializa un asteroide hijo sin movimiento ni colisión
    public void InitAsteroidChild()
    {
        offScreenWrapper.enabled = false;
        rigid.isKinematic = true;
        transform.localScale = transform.localScale.ComponentDivide(transform.parent.lossyScale);
    }

    // Establece la velocidad inicial del asteroide
    public void InitVelocity()
    {
        Vector3 vel;

        if (ScreenBounds.OOB(transform.position))
        {
            // Si el asteroide está fuera de los límites, lo dirige hacia el centro
            vel = ((Vector3)Random.insideUnitCircle * 4) - transform.position;
            vel.Normalize();
        }
        else
        {
            // Si está en pantalla, asigna una dirección aleatoria
            do
            {
                vel = Random.insideUnitCircle;
                vel.Normalize();
            } while (Mathf.Approximately(vel.magnitude, 0f));
        }

        // Ajusta la velocidad según el tamaño del asteroide
        vel = vel * Random.Range(AsteraX.AsteroidsSO.minVel, AsteraX.AsteroidsSO.maxVel) / (float)size;
        rigid.velocity = vel;
        rigid.angularVelocity = Random.insideUnitSphere * AsteraX.AsteroidsSO.maxAngularVel;
    }

    // Verifica si este asteroide es hijo de otro asteroide
    bool parentIsAsteroid
    {
        get { return (parentAsteroid != null); }
    }

    // Obtiene el asteroide padre si existe
    Asteroid parentAsteroid
    {
        get
        {
            if (transform.parent != null)
            {
                return transform.parent.GetComponent<Asteroid>();
            }
            return null;
        }
    }
    

    public void OnCollisionEnter(Collision coll)
    {
        if (parentIsAsteroid)
        {
            parentAsteroid.OnCollisionEnter(coll);
            return;
        }

        if (immune)
        {
            return;
        }

        GameObject otherGO = coll.gameObject;

        if (otherGO.tag == "Bullet" || otherGO.transform.root.gameObject.tag == "Player")
        {
            if (otherGO.tag == "Bullet")
            {
                Destroy(otherGO);
            }

            if (otherGO.tag != "Bullet" && transform.parent == null)
            { 
                playerShip.Impact();
            }
            
            if (size > 1)
            {
                // Separa los asteroides hijos cuando el padre es destruido
                Asteroid[] children = GetComponentsInChildren<Asteroid>();
                playerShip.Destroyed();
                for (int i = 0; i < children.Length; i++)
                {
                    children[i].immune = true;
                    if (children[i] == this || children[i].transform.parent != transform)
                    {
                        continue;
                    }
                    children[i].transform.SetParent(null, true);
                    children[i].InitAsteroidParent();
                }
            }
            
            Destroy(gameObject); // Destruye este asteroide
        }
    }
    
    

    private void Update()
    {
        immune = false; // Restablece la inmunidad en cada fotograma
    }

    // Método estático para instanciar un nuevo asteroide
    static public Asteroid SpawnAsteroid()
    {
        GameObject aGO = Instantiate<GameObject>(AsteraX.AsteroidsSO.GetAsteroidPrefab());
        return aGO.GetComponent<Asteroid>();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
    private float minVel;
    private float maxVel;
    private float maxAngularVel;
    private int initialSize;
    private float asteroidScale;
    private int numSmallerAsteroidsToSpawn;
    private int[] pointsForAsteroidSize;

    // Variable para controlar si este asteroide es el padre inicial
    private bool isTheBigPapa = true;

    // Start is called before the first frame update
    void Start()
    {
        // Obtener los parámetros del ScriptableObject
        var parameters = AsteroidsScriptableObject.S.GetParameters();
        GameObject prefab = AsteroidsScriptableObject.S.GetAsteroidPrefab();
        Transform parentPosition = gameObject.transform;

        minVel = parameters.Item1;
        maxVel = parameters.Item2;
        maxAngularVel = parameters.Item3;
        initialSize = parameters.Item4;
        asteroidScale = parameters.Item5;
        numSmallerAsteroidsToSpawn = parameters.Item6;
        pointsForAsteroidSize = parameters.Item7;

        // Solo ejecutar la mitosis si es el asteroide inicial
        if (isTheBigPapa)
        {
            Mitosis(prefab, parentPosition, initialSize);
        }
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            GameObject bullet = other.gameObject;
            Destroy(bullet);
            Impacto();
            

        }
    }

    // Método para generar asteroides hijos
    void Mitosis(GameObject prefab, Transform parentPosition, int currentSize)
    {
        if (currentSize <= 1)
        {
            // Si el tamaño es 1, no generamos más asteroides
            return;
        }

        for (int i = 0; i < numSmallerAsteroidsToSpawn; i++)
        {
            // Crear un nuevo asteroide
            GameObject newAsteroid = Instantiate(prefab, parentPosition.position, Random.rotation);
            newAsteroid.transform.SetParent(parentPosition);

            // Ajustar la escala del nuevo asteroide (hacerlo más pequeño)
            int childSize = currentSize - 1; // Reducir el tamaño en 1
            newAsteroid.transform.localScale = Vector3.one * childSize * asteroidScale;

            // Desactivar el componente offScreenWrapper
            var offScreenWrapper = newAsteroid.GetComponent<OffScreenWrapper>();
            if (offScreenWrapper != null)
            {
                offScreenWrapper.enabled = false;
            }

            // Activar el componente Rigidbody como Kinematic
            var rigidbody = newAsteroid.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = true;
            }

            // Activar el componente Rigidbody como Kinematic
            var collider = newAsteroid.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            // Marcar el nuevo asteroide como no inicial (para evitar que ejecute la mitosis)
            var asteroidScript = newAsteroid.GetComponent<Asteroid>();
            if (asteroidScript != null)
            {
                asteroidScript.isTheBigPapa = false;
            }

            // Llamar a la mitosis para el nuevo asteroide con un tamaño reducido
            Mitosis(prefab, newAsteroid.transform, childSize);
        }
    }

    void Impacto()
    {
        // Obtener todos los hijos directos del asteroide impactado
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
        {
            children.Add(child);
        }

        // Liberar y activar cada hijo
        foreach (Transform child in children)
        {
            // Liberar al hijo del padre
            child.SetParent(null);

            // Activar el Rigidbody si es necesario
            var rigidbody = child.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = false;
            }

            // Activar el Collider si es necesario
            var collider = child.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true;
            }

            // Activar el OffScreenWrapper si es necesario
            var offScreenWrapper = child.GetComponent<OffScreenWrapper>();
            if (offScreenWrapper != null)
            {
                offScreenWrapper.enabled = true;
            }
        }

        // Destruir el asteroide padre
        Destroy(gameObject);
    }
}

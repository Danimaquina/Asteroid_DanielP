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

    // Variable para controlar si este asteroide es el padre inicial
    private bool isTheBigPapa = true;

    // Start is called before the first frame update
    void Start()
    {
        // Obtenemos los parámetros del ScriptableObject
        var parameters = AsteroidsScriptableObject.S.GetParameters();
        GameObject prefab = AsteroidsScriptableObject.S.GetAsteroidPrefab();
        Transform parentPosition = gameObject.transform;

        minVel = parameters.Item1;
        maxVel = parameters.Item2;
        maxAngularVel = parameters.Item3;
        initialSize = parameters.Item4;
        asteroidScale = parameters.Item5;
        numSmallerAsteroidsToSpawn = parameters.Item6;

        // Solo ejecutar la mitosis si es el asteroide inicial
        if (isTheBigPapa)
        {
            Mitosis(parentPosition, initialSize);
        }
        impulso();
    }

    void Update()
    {

    }

    void impulso()
    {
        var rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            float velocidad = Random.Range(minVel, maxVel);

            // Calcular una dirección aleatoria en 3D (en una esfera)
            Vector3 direccion = Random.onUnitSphere;

            rigidbody.velocity = direccion * velocidad;

            // Aplicar una rotación aleatoria, pero más lenta para asteroides más grandes
            float sizeFactor = transform.localScale.x;
            rigidbody.angularVelocity = Random.insideUnitSphere * maxAngularVel / sizeFactor;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            GameObject bullet = other.gameObject;
            Destroy(bullet);
            Impacto();
        }

        if (other.CompareTag("Player"))
        {
            Impacto();
        }
    }

    // Método para generar asteroides hijos
    void Mitosis(Transform parentPosition, int currentSize)
    {
        GameObject prefab = AsteroidsScriptableObject.S.GetAsteroidPrefab();

        if (currentSize <= 1)
        {
            // Si el tamaño es 1, no generamos más asteroides
            return;
        }

        for (int i = 0; i < numSmallerAsteroidsToSpawn; i++)
        {
            // Calcular un desplazamiento aleatorio alrededor de la posición del padre
            Vector3 randomOffset = Random.onUnitSphere * 0.5f;

            // Crear un nuevo asteroide con un ligero desplazamiento aleatorio
            GameObject newAsteroid = Instantiate(prefab, parentPosition.position + randomOffset, Random.rotation);
            newAsteroid.transform.SetParent(parentPosition);

            // Ajustar la escala del nuevo asteroide 
            int childSize = currentSize - 1; 
            newAsteroid.transform.localScale = Vector3.one * childSize * asteroidScale;
            
            string daddyName = newAsteroid.transform.parent.name;
            
            newAsteroid.name = daddyName + "-" + i;

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

            // Desactovar el componente Collider
            var collider = newAsteroid.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
            }

            // Marcar el nuevo asteroide como no inicial para que no haga la mitosis
            var asteroidScript = newAsteroid.GetComponent<Asteroid>();
            if (asteroidScript != null)
            {
                asteroidScript.isTheBigPapa = false;
            }

            // Llamar a la mitosis para la creacion de los nietos 
            Mitosis(newAsteroid.transform, childSize);
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
        for (int i = 0; i < children.Count; i++)
        {
            Transform child = children[i];

            // Liberar al hijo del padre
            child.SetParent(null);

            // Activar el Rigidbody 
            var rigidbody = child.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.isKinematic = false;

                // Aumentar la probabilidad de velocidades más altas para asteroides más pequeños
                float factorTamaño = 1f / child.localScale.x;
                float velocidad = Random.Range(minVel * factorTamaño, maxVel * factorTamaño);

                // Calcular una dirección opuesta para cada par de hijos
                Vector3 direccion = (i % 2 == 0) ? Random.onUnitSphere : -children[i - 1].position.normalized;

                // Aplicar la velocidad en la dirección calculada
                rigidbody.velocity = direccion * velocidad;

                // Aplicar una rotación aleatoria, pero más lenta para asteroides más grandes
                float sizeFactor = child.localScale.x;
                rigidbody.angularVelocity = Random.insideUnitSphere * maxAngularVel / sizeFactor;
            }

            // Activar el Collider 
            var collider = child.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = true;
            }

            // Activar el OffScreenWrapper 
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
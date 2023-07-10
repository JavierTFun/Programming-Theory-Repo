using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    // ENCAPSULATION (Static Variable)
    public static int globalCount = 0;
    public AudioClip collisionSound;

    // ENCAPSULATION (Static Variable)
    public static Counter instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // ABSTRACTION (calling to other class method)
        Hole.counter = this;

        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.StartCountdown();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Shape shape = other.GetComponent<Shape>();

        if (shape != null && shape.IsDestroyed == false)
        {
            Hole hole = GetComponent<Hole>();
            if (hole != null && shape.FitsHole(hole))
            {
                IncrementCount();

                // Reproducir el sonido de colisión
                if (collisionSound != null)
                {
                    AudioSource.PlayClipAtPoint(collisionSound, other.transform.position);
                }

                // Destruir la forma
                shape.DestroyShape();
            }
        }
    }

    //ABSTRACTION (method)
    public void IncrementCount()
    {
        globalCount++;
        Debug.Log("Forma colocada en su agujero adecuado. Puntos: " + globalCount);
        DontDestroyOnLoad(gameObject);

    }

    public static void RestartGlobalCount()
    {
        globalCount = 0;
    }

   
    // ABSTRACTION (property)
    public static int CurrentCount
    {
        get { return globalCount; }
    }
}
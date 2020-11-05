using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoordinateSpaceManager : MonoBehaviour
{
    public GameObject focus;
    public GameObject container;
    public float threshold = 1000f;

    public static CoordinateSpaceManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if (!focus)
        {
            focus = Camera.main.gameObject;
        }

        container = new GameObject("Container");

        GameObject[] sceneRoots = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach(GameObject g in sceneRoots)
        {
            g.transform.SetParent(container.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (focus)
        {
            if(focus.transform.position.magnitude > threshold)
            {
                container.transform.position -= focus.transform.position;
            }
        }

        GameObject[] sceneRoots = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject g in sceneRoots)
        {
            if (!g.Equals(container))
            {
                g.transform.SetParent(container.transform);
            }
        }
    }
}

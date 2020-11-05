using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UITargetIdentifier : MonoBehaviour
{

    public static UITargetIdentifier instance;

    public class Vector3ZComparer : IComparer<Vector3>
    {
        public int Compare(Vector3 a, Vector3 b)
        {
            if (a.z == b.z)
                return 0;
            if (a.z < b.z)
                return -1;

            return 1;
        }
    }

    public GameObject targetPrefab;

    public GameObject CenterTarget;

    public int numTargets = 1;

    public Color offTargetColor = Color.red;
    public Color centerTargetColor = Color.green;

    GameObject[] targets;
    Image[] targetRenderers;

    GameObject closestTargetToCenter;
    public GameObject closestEnemyToCenter;
    float closestTargetToCenterDistance = Mathf.Infinity;
    Vector2 center;

    Canvas HUD;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        center = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);

        targets = new GameObject[numTargets];
        targetRenderers = new Image[numTargets];

        HUD = GameObject.FindGameObjectWithTag("HUDCanvas").GetComponent<Canvas>();
        for (int i = 0; i < numTargets; i++)
        {
            targets[i] = Instantiate(targetPrefab, HUD.transform);
            targetRenderers[i] = targets[i].GetComponent<Image>();
            targetRenderers[i].color = offTargetColor;
        }

        closestTargetToCenter = targets[0];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Dictionary<Vector3, GameObject> indexDistEnt = new Dictionary<Vector3, GameObject>();
        Dictionary<GameObject, GameObject> indexTgtEnt = new Dictionary<GameObject, GameObject>();

        Vector3 min = new Vector3(0, 0, float.MaxValue);

        GameObject[] targetEntities = GameObject.FindGameObjectsWithTag("Target");
        Vector3[] distances = new Vector3[targetEntities.Length];

        for (int i = 0; i < targetEntities.Length; i++)
        {
            distances[i] = Camera.main.WorldToScreenPoint(targetEntities[i].transform.position);
            if (!indexDistEnt.ContainsKey(distances[i]))
            {
                indexDistEnt.Add(distances[i], targetEntities[i]);
            }
            
        }

        Array.Sort(distances, new Vector3ZComparer());

        

        for(int i = 0; i < numTargets; i++)
        {
            
            if (i < distances.Length)
            {
                GameObject ent;
                indexDistEnt.TryGetValue(distances[i], out ent);
                indexTgtEnt.Add(targets[i], ent);
                if (distances[i].z < 0)
                {
                    targets[i].transform.localPosition = new Vector3(0, Camera.main.pixelWidth * 2, -1);
                    
                    continue;
                }
                targets[i].transform.localPosition = distances[i] - new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, distances[i].z);
            } else
            {
                targets[i].transform.localPosition = new Vector3(0, Camera.main.pixelWidth * 2, -1);
            }
        }


        int closestI = 0;
        closestTargetToCenterDistance = Mathf.Infinity;

        for (int i = 0; i < numTargets; i++){

            if (targetRenderers[i].color.Equals(centerTargetColor))
            {
                targetRenderers[i].color = offTargetColor;
            }

            Vector2 thisPos = targets[i].GetComponent<RectTransform>().anchoredPosition;

            float thisDistance = Vector2.Distance(thisPos, Vector2.zero);
            if (thisDistance < closestTargetToCenterDistance)
            {
                closestTargetToCenter = targets[i];
                closestI = i;
                closestTargetToCenterDistance = thisDistance;
            }

        }

        if(closestTargetToCenterDistance < Camera.main.pixelWidth / 6)
        {
            indexTgtEnt.TryGetValue(closestTargetToCenter, out closestEnemyToCenter);
            targetRenderers[closestI].color = centerTargetColor;
            CenterTarget.SetActive(false);
        } else
        {
            closestEnemyToCenter = null;
            CenterTarget.SetActive(true);
        }
        
        //closestEnemyToCenter.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        
    }
}

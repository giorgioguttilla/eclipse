using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{

    public float range = 500f;
    public float radarSize = 1f;
    public int maxBlips = 10;
    public Color enemyColor = Color.red;
    public Color thisShipColor = Color.white;

    ParticleSystem ps;
    ParticleSystem.Particle[] particles;

    // Start is called before the first frame update
    void Start()
    {
        ps = gameObject.GetComponent<ParticleSystem>();
        ps.gameObject.GetComponent<Renderer>().material.renderQueue -= 1;
        particles = new ParticleSystem.Particle[maxBlips];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        int numParticles = ps.GetParticles(particles);

        particles[0].color = thisShipColor;
        particles[0].position = Vector3.zero;

        for (int i = 0; i < numParticles; i++)
        {
            if (i + 1 < particles.Length)
            {
                if (i < targets.Length)
                {
                    Vector3 scaledPosition = transform.InverseTransformPoint(targets[i].transform.position);
                    scaledPosition = (scaledPosition / range);
                    scaledPosition *= Mathf.Log(scaledPosition.magnitude + 1, 10);
                    if (scaledPosition.magnitude < radarSize)
                    {
                        particles[i + 1].position = scaledPosition;
                        particles[i + 1].size = 0.1f;
                        particles[i + 1].color = enemyColor;
                    }
                    else
                    {
                        particles[i + 1].size = 0;
                    }
                }
                else
                {
                    particles[i + 1].size = 0;
                }
            }
            //particles[i] = 
            //particles[i].position = scaledPosition;
            //particles[i].position = new Vector3(0, i, 0);

        }

        ps.SetParticles(particles, numParticles);


    }
}

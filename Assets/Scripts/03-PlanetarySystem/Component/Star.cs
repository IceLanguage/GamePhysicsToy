using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LinHowe_GamePhysics.PlanetarySystem
{
    /// <summary>
    /// 恒星
    /// </summary>
    public class Star : MonoBehaviour
    {
        public float mass = 100;
        private Planet[] planets;
        private readonly float G = 6.673f * Mathf.Pow(10, -11);
        //public float gravity;
        private void Start()
        {
            planets = FindObjectsOfType<Planet>();
        }
        private void Update()
        {
            foreach(Planet p in planets)
            {
                Vector3 direction = p.transform.position - transform.position;
                float angle = Vector3.SignedAngle(Vector3.right, direction, Vector3.forward);
                float r = direction.magnitude ;
                float F = (G * mass * p.mass) / Mathf.Pow(r, 2);
                float a = F / p.mass;
                float M = mass + p.mass;
                float v = Mathf.Sqrt(G * M * (2 * a - r) / (a * r));
                float len = v * Time.deltaTime;
                len %= (2 * Mathf.PI * r);
                float newangle = len / r / Mathf.Deg2Rad;
                angle += newangle;
                Vector3 newv = new Vector3(
                    Mathf.Cos(angle * Mathf.Deg2Rad) * r,
                    Mathf.Sin(angle * Mathf.Deg2Rad) * r,
                    p.transform.position.z);
                p.transform.position = newv;
            }
        }
    }
}
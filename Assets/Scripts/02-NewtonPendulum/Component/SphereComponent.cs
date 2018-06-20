using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CollisionDetection;
namespace LinHowe_GamePhysics.NewtonPendulum
{
    public class SphereComponent : MonoBehaviour
    {
        //public Sphere sphere;
        public float r;
        private new Rigidbody rigidbody;
        /// <summary>
        /// 已经检测过的AABB
        /// </summary>
        public AABB HasCheckedAABB ;
        private SphereComponent hasCheckedSphere;
        public SphereComponent HasCheckedSphere
        {
            get
            {
                return hasCheckedSphere;
            }
            set
            {
                hasCheckedSphere = value;
                StartCoroutine(CleanCheckedSphere());
            }
        }
        public Rigidbody Rigidbody
        {
            get
            {
                if(null == rigidbody)
                    rigidbody = GetComponent<Rigidbody>();
                return rigidbody;
            }
        }
        public Sphere Sphere
        {
            get
            {
                return new Sphere(transform.position, r);
            }
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.4f, 0.9f, 0.4f);
            Gizmos.DrawWireSphere(transform.position, r);

        }
        
        IEnumerator CleanCheckedSphere()
        {
            yield return new WaitForSeconds(0.1f);
            hasCheckedSphere = null;
        }



    }
}


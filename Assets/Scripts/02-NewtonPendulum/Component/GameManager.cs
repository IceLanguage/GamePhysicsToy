using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LinHowe_Algorithm;
using CollisionDetection;

namespace LinHowe_GamePhysics.NewtonPendulum
{
    public class GameManager : MonoBehaviour
    {
        //球体包围体
        private SphereComponent[] spheres;

        //球体们的初始坐标
        private Vector3[] initialSpheresPos;

        //单摆线支点坐标
        private Vector3[] sphereCentersPos;

        //球体们的速度
        private Vector3[] spheresVelocity;

        //球体们的质量
        private float[] mass;

        //单摆线的长度
        private const float LineLength = 8;

        //重力
        private const float gravity = 9.8f;

        //计算小球的位置
        private static System.Func<float, Vector3, Vector3> GetNowPosFromAngle = (angle, centerPos) =>
          {
              
              if (float.IsNaN(angle))
                  return new Vector3(centerPos.x, centerPos.y - LineLength, centerPos.z);
              float x = Mathf.Sin(angle * Mathf.Deg2Rad) * LineLength;
              float y = Mathf.Cos(angle * Mathf.Deg2Rad) * LineLength;
              
              return new Vector3(centerPos.x - x, centerPos.y - y, centerPos.z);
          };
        private void Awake()
        {
            spheres = FindObjectsOfType<SphereComponent>();
            initialSpheresPos = new Vector3[spheres.Length];
            sphereCentersPos = new Vector3[spheres.Length];
            spheresVelocity = new Vector3[spheres.Length];
            mass = new float[spheres.Length];
            for (int i = 0; i < spheres.Length; ++i)
            {
                mass[i] = 1f;
                initialSpheresPos[i] = spheres[i].transform.position;
                sphereCentersPos[i] = new Vector3(
                    initialSpheresPos[i].x,
                    initialSpheresPos[i].y + LineLength,
                    initialSpheresPos[i].z
                    );
            }                
        }

        private void Update()
        {
            //检测碰撞
            CollisionDetection();


            //更新牛顿摆
            UpdateNewtonPendulum();
        }
        //IEnumerator UpdatePendulum()
        //{
        //    yield return new WaitForSeconds(3.5f);
        //    UpdateNewtonPendulum();
        //}
        //检测碰撞
        private void CollisionDetection()
        {
            for (int i = 0; i < spheres.Length; ++i)
            {
                for (int j = 0; j < spheres.Length; ++j)
                {
                    if (i == j) continue;
                    if (spheres[i].HasCheckedSphere == spheres[j]) continue;
                    if (IntersectionTest.Check_Sphere_Sphere(spheres[i].Sphere, spheres[j].Sphere))
                    {

                    }
                }
            }
        }
        //更新牛顿摆
        private void UpdateNewtonPendulum()
        {
            for (int i = 0; i < spheres.Length; ++i)
            {
                
                Vector3 center = sphereCentersPos[i];
                Vector3 spherePos = spheres[i].transform.position;

                float angle = Vector3.SignedAngle(spherePos - center, Vector3.down,Vector3.forward);
                Vector3 curspeed = spheresVelocity[i];
                float SqrSpeed = Mathf.Pow(curspeed.x,2)+ Mathf.Pow(curspeed.y, 2);

                Vector3 newspeed = curspeed;
                Vector3 newpos = spherePos;

                if (0!=angle||Vector3.zero!=curspeed)
                {                   
                    if(angle>0|| curspeed.x>0)
                    {
                        //向右移动
                        float h = (curspeed.y + gravity*Time.deltaTime/2) * Time.deltaTime;
                        float yoffset = h + center.y - spherePos.y;
                        if(yoffset>LineLength)
                        { 
                            h = 0;
                            yoffset = h + center.y - spherePos.y;
                            newspeed.y = - newspeed.y;
                        }
                        
                        float xoffset = Mathf.Sqrt(Mathf.Pow(LineLength, 2) - Mathf.Pow(yoffset, 2));
                        if (angle<0)
                            newpos.x = center.x - xoffset;
                        else
                            newpos.x = center.x + xoffset;
                        newpos.y = center.y - yoffset;
                        float newangle = Vector3.Angle(newpos - center, Vector3.down);

                        float speed = Mathf.Sqrt(SqrSpeed + gravity * h * 2);
                        if(yoffset!=8)
                        {
                            newspeed.x = Mathf.Cos(newangle * Mathf.Rad2Deg) * speed;
                            newspeed.y = Mathf.Sin(newangle * Mathf.Rad2Deg) * speed;
                            if (angle < 0)
                                newspeed.y = -newspeed.y;
                        }
                        else
                        {
                            newspeed.x = speed;
                            newspeed.y = 0;
                        }
                    }
                    else if (angle < 0 || curspeed.x < 0)
                    {
                        //向右移动
                        float h = (curspeed.y + gravity * Time.deltaTime / 2) * Time.deltaTime;
                        float yoffset = h + center.y - spherePos.y;
                        if (yoffset > LineLength)
                        {
                            h = 0;
                            yoffset = h + center.y - spherePos.y;
                            newspeed.y = -newspeed.y;
                        }

                        float xoffset = Mathf.Sqrt(Mathf.Pow(LineLength, 2) - Mathf.Pow(yoffset, 2));
                        if (angle < 0)
                            newpos.x = center.x - xoffset;
                        else
                            newpos.x = center.x + xoffset;
                        newpos.y = center.y - yoffset;
                        float newangle = Vector3.Angle(newpos - center, Vector3.down);

                        float speed = Mathf.Sqrt(SqrSpeed + gravity * h * 2);
                        if (yoffset != 8)
                        {
                            newspeed.x = -Mathf.Cos(newangle * Mathf.Rad2Deg) * speed;
                            newspeed.y = Mathf.Sin(newangle * Mathf.Rad2Deg) * speed;
                            if (angle < 0)
                                newspeed.y = -newspeed.y;
                        }
                        else
                        {
                            newspeed.x = -speed;
                            newspeed.y = 0;
                        }
                    }

                }

                spheresVelocity[i] = newspeed;
                spheres[i].transform.position = newpos;

            }
        }

        [ContextMenu("初始化牛顿摆")]
        public void Init()
        {
            for (int i = 0; i < spheres.Length; ++i)
            {
                spheres[i].transform.position = initialSpheresPos[i];
                spheresVelocity[i] = Vector3.zero; ;
            }
        }

        [ContextMenu("让牛顿摆摆动起来")]
        public void AddForce()
        {
            Init();
            float angle = Random.Range(5f, 15f);
            Vector3 v = GetNowPosFromAngle(angle, sphereCentersPos[0]);
            spheres[0].transform.position = v;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LinHowe_GamePhysics.ParabolicMovement
{
    /// <summary>
    /// 匕首控制
    /// </summary>
    public class DaggerController : MonoBehaviour
    {
        //投掷角度
        private float ThrowAngle;

        //投掷速度
        private float ThrowSpeed;

        //速度矢量
        private Vector3 Velocity = Vector3.zero;

        //重力
        private const float gravity = -9.8f;

        //质量
        private float mass;

        //绘制抛物线的摄像头
        private DrawCurve drawCurve;

        
        //随机投掷匕首
        public void RandomThrowingDagger()
        {
            //设置摄像头绘制轨迹的对象
            drawCurve = Camera.main.GetComponent<DrawCurve>();
            drawCurve.Dagger = gameObject;

            //初始化投掷角度，速度，匕首质量
            ThrowAngle = Random.Range(10f, 80f);
            ThrowSpeed = Random.Range(4f, 10f);
            mass = Random.Range(0.4f, 2f);
            
            //计算初始速度矢量
            float cos = Mathf.Cos(ThrowAngle * Mathf.Deg2Rad);
            float sin = Mathf.Sin(ThrowAngle * Mathf.Deg2Rad);
            Velocity = new Vector3(ThrowSpeed * cos, ThrowSpeed * sin, 0);

            //利用协程更新匕首状态
            StartCoroutine(UpdateCorrespondence(0.15f));

        }
        
        /// <summary>
        /// 向目标投掷匕首
        /// </summary>
        public void ThrowingDaggerForAim(Vector3 aim,float speed)
        {
            //设置摄像头绘制轨迹的对象
            drawCurve = Camera.main.GetComponent<DrawCurve>();
            drawCurve.Dagger = gameObject;

            //初始化匕首质量,速度
            mass = Random.Range(0.4f, 2f);
            ThrowSpeed = speed;

            //计算投掷角度
            if(mass * gravity * (aim.y - transform.position.y)
                > mass / 2 * Mathf.Pow(speed,2))
            {
                ThrowAngle = 90;
            }   
            else ThrowAngle = GetAngle(aim, speed);

            //计算初始速度矢量
            float cos = Mathf.Cos(ThrowAngle * Mathf.Deg2Rad);
            float sin = Mathf.Sin(ThrowAngle * Mathf.Deg2Rad);
            Velocity = new Vector3(ThrowSpeed * cos, ThrowSpeed * sin, 0);

            //利用协程更新匕首状态
            StartCoroutine(UpdateCorrespondence(0.15f));
        }

        //计算投掷角度
        private float GetAngle(Vector3 aim, float speed)
        {
           
            
            float x = aim.x - transform.position.x;
            float y = aim.y - transform.position.y;

            //计算某角度时的近似值，越小越近似
            System.Func<float, float> GetDifference = Angle =>
            {
                float t = x / speed / Mathf.Cos(Angle * Mathf.Deg2Rad);
                float left = speed * Mathf.Sin(Angle * Mathf.Deg2Rad) * t - gravity * Mathf.Pow(t, 2) / 2;
                float right = y;
                
                return Mathf.Abs(left - right);
               
            };
            float angle = 1;
            float mindis = GetDifference(1);
            for (float i = 2f;i< 89;i+=0.001f)
            {
                float dis = GetDifference(i);
                if (dis < mindis)
                {
                    mindis = dis;
                    angle = i;
                }
            }
            return angle;
        }

        //利用协程更新匕首状态
        IEnumerator UpdateCorrespondence(float t)
        {
            ChangeAngle();
            Movement();
            UpdateVelocity();
            yield return new WaitForSeconds(t);
            StartCoroutine(UpdateCorrespondence(t));
        }

     
        //运动
        private void Movement()
        {
            transform.position = new Vector3(
                transform.position.x + Velocity.x * Time.deltaTime,
                transform.position.y + Velocity.y * Time.deltaTime,
                transform.position.z + Velocity.z * Time.deltaTime
                );
        }

        //更新速度
        private void UpdateVelocity()
        {
            float a = gravity / mass;
            Velocity = new Vector3(Velocity.x, Velocity.y + a * Time.deltaTime, Velocity.z);
        }

        //更改匕首角度
        private void ChangeAngle()
        {
            float Angle = (float)System.Math.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(90-Angle, 90, 180));

        }
    }
}


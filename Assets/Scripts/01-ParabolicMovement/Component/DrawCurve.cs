using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LinHowe_Algorithm;

namespace LinHowe_GamePhysics.ParabolicMovement
{
    /// <summary>
    /// 绘制轨迹曲线
    /// </summary>
    public class DrawCurve : MonoBehaviour
    {
        private GameObject dagger;
        public GameObject Dagger
        {
            get
            {
                return dagger;
            }
            set
            {
                MovementPoints.Clear();
                dagger = value;
               
            }
        }
        public Material material;
        private Queue<Vector3> MovementPoints = new Queue<Vector3>();
        private void Update()
        {
            
            if (null == Dagger) return;

            //获取轨迹点
            Vector2 v = Camera.main.WorldToScreenPoint(Dagger.transform.position);
            Vector3 newv = new Vector3(v.x, v.y, 0);
            if (0 == MovementPoints.Count || newv != MovementPoints.Last())
                MovementPoints.Enqueue(newv);
        }

        //绘制匕首运动轨迹
        void OnPostRender()
        {
           
            GL.PushMatrix(); //保存当前Matirx  
            material.SetPass(0); //刷新当前材质  
            GL.LoadPixelMatrix();//设置pixelMatrix  

            
            int i = 0;

            
            List<Vector3> list = new List<Vector3>();

            //曲线精度
            int count = 1000;
            //计算贝塞尔曲线
            var bezierCurve1 = new BezierCurve(MovementPoints.ToArray(), count);
            list = bezierCurve1.Curve;
           
            //绘制曲线
            for (; i < count - 1; ++i)
            {
                Vector3 v = list[i];
                GL.Begin(GL.LINES);
                GL.Vertex(v);
                GL.Vertex(list[i + 1]);
                GL.End();


            }

            
            GL.PopMatrix();//读取之前的Matrix  
        }
    }

}
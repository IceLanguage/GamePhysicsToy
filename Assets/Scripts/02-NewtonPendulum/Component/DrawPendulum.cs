using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LinHowe_GamePhysics.NewtonPendulum
{
    public class DrawPendulum : MonoBehaviour
    {
        public Material material;

        //单摆线点
        public GameObject[] Points;

        //小球
        public GameObject[] Balls;

        private System.Func<Vector3, Vector3> GetScreenPoint = x =>
          {
              Vector2 v = Camera.main.WorldToScreenPoint(x);
              return new Vector3(v.x, v.y, 0);
          };
        void OnPostRender()
        {

            GL.PushMatrix(); //保存当前Matirx  
            material.SetPass(0); //刷新当前材质  
            GL.LoadPixelMatrix();//设置pixelMatrix  

            int len = Mathf.Min(Points.Length, Balls.Length);
            for(int i =0;i<len;++i)
            {
                GL.Begin(GL.LINES);
                GL.Vertex(GetScreenPoint(Points[i].transform.position));
                GL.Vertex(GetScreenPoint(Balls[i].transform.position));
                GL.End();
            }
            GL.PopMatrix();//读取之前的Matrix  
        }
    }
}

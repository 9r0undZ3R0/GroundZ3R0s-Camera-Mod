using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Libraries
{
    /// <summary>
    /// A library that allows you to manage GameObjects
    /// </summary>
    internal class ObjLIB
    {
        public static GameObject CreateObj(string name, PrimitiveType shape, string shader, bool collidable, Vector3 size, Vector3 position, Color color)
        {
            var gameObj = GameObject.CreatePrimitive(shape);
            gameObj.name = name;
            gameObj.GetComponent<Renderer>().material.shader = Shader.Find(shader);
            gameObj.GetComponent<Renderer>().material.color = color;
            if (!collidable)
            {
                GameObject.Destroy(gameObj.GetComponent<Collider>());
            }
            gameObj.transform.localScale = size;
            gameObj.transform.position = position;
                return gameObj;
        }
        public static GameObject FindObj(string name)
        {
            var obj = GameObject.Find(name);
                return obj;
        }
        public static void DestroyObj(GameObject obj)
        {
            GameObject.Destroy(obj);
        }
        public static void DestroyObj(string name)
        {
            var obj = GameObject.Find(name);
            GameObject.Destroy(obj);
        }
        public static List<Vector3> GetValues(GameObject obj)
        {
            var resultpos = obj.transform.position;
            var resultscale = obj.transform.localScale;
            var resulteulers = obj.transform.eulerAngles;
            List<Vector3> results = new List<Vector3>();
            results.Add(resultpos);
            results.Add(resultscale);
            results.Add(resulteulers);
                return results;
        }
        public static List<Vector3> GetValues(string name)
        {
            var obj = GameObject.Find(name);
            var resultpos = obj.transform.position;
            var resultscale = obj.transform.localScale;
            var resulteulers = obj.transform.eulerAngles;
            List<Vector3> results = new List<Vector3>();
            results.Add(resultpos);
            results.Add(resultscale);
            results.Add(resulteulers);
                return results;
        }
    }
}

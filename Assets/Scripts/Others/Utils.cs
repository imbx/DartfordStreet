using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoxScripts {
    public class TransformData {
        public Vector3 position { get; }
        public Vector3 eulerAngles { get; private set; }

        public TransformData (Transform copy) {
            position = copy.position;
            eulerAngles = copy.eulerAngles;
        }

        public TransformData (Vector3 pos, Vector3 rot) {
            position = pos;
            eulerAngles = rot;
        }

        public Vector3 LerpDistance(TransformData other, float timer)
        {
            return Vector3.Lerp(position, other.position, timer);
        }

        public Vector3 LerpAngle(TransformData other, float timer)
        {
            return new Vector3(
                Mathf.LerpAngle(eulerAngles.x, other.eulerAngles.x, timer),
                Mathf.LerpAngle(eulerAngles.y, other.eulerAngles.y, timer),
                Mathf.LerpAngle(eulerAngles.z, other.eulerAngles.z, timer));
        }

        public void SetEuler(Vector3 newRot)
        {
            eulerAngles = newRot;
        }

        
    }

    public class NotebookPage {
        public int reqID { get; }
        public string text { get; }
        public NotebookPage (string dText)
        {
            reqID = -1;
            text = dText;
        }

        public NotebookPage (int rqId, string dText)
        {
            reqID = rqId;
            text = dText;
        }
    }

    public class DiaryController : MonoBehaviour
    {

    }

    public enum NotebookType{
        Diary,
        Notes,
        None
    }

    public enum RadioSound{
        Beep,
        Peep,
        None
    }

    public enum PinSelect{
        Red,
        Blue,
        None
    }

    public enum GameState {
        TITLESCREEN,
        LOADGAME,
        PLAYING,
        INTERACTING,
        ENDINTERACTING,
        LOOKITEM,
        ENDLOOKITEM,
        TARGETINGPICTURE,
        TARGETING,
        MOVINGPICTURE,
        OPENNOTEBOOK,
        CLOSENOTEBOOK,
        ENDGAME,
        MOVINGCAMERA
    }

    public enum TextPosition
    {
        TOPLEFT,
        TOP,
        TOPRIGHT,
        BOTTOMLEFT,
        BOTTOM,
        BOTTOMRIGHT,
        NONE
    }

    public enum AchievementType
    {
        Note,
        Key,
        Picture,
        Diary,
        OilLamp,
        OilLampUpgrade,
        Tool,
        None
    }

    [Serializable]
    public class Dialogue
    {
        public int id;
        public SerializableVector2 anchoredPosition;
        public SerializableVector2 size;
        public string dialogueText;
        public bool isAnchoredAtTop;
        public bool isAnchoredAtCenter;
        public float lifeTime;
    }

     [Serializable]
    public class Interactions
    {
        public string tag;
        public string interactionText;
    }


    [Serializable]
    public class SerializableVector2
    {
        public float x;
        public float y;
        public Vector2 Vector2
        {
            get
            {
                return new Vector2(x, y);
            }
            set
            {
                x = value.x;
                y = value.y;
            }
        }

        public SerializableVector2() { x = y = 0; }
        public SerializableVector2(Vector2 vector2) : this(vector2.x, vector2.y) {}
        public SerializableVector2 (float _x, float _y)
        {
            x = _x;
            y = _y;
        }
    }
    [Serializable]
    public class SerializableVector3
    {
        public float x;
        public float y;
        public float z;
        public Vector3 Vector3
        {
            get
            {
                return new Vector3(x, y, z);
            }
            set
            {
                x = value.x;
                y = value.y;
                z = value.z;
            }
        }

        public SerializableVector3() { x = y = z = 0; }
        public SerializableVector3(Vector3 vector3) : this(vector3.x, vector3.y, vector3.z) {}
        public SerializableVector3 (float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
    }

    public class Line
    {
        public float x1 { get; set; }
        public float y1 { get; set; }

        public float x2 { get; set; }
        public float y2 { get; set; }

        public Line() { x1 = y1 = x2 = y2 = 0; }
        public Line(Vector2 point1, Vector2 point2) : this (point1.x, point1.y, point2.x, point2.y) {}
        public Line(Vector3 point1, Vector3 point2) : this (point1.x, point1.y, point2.x, point2.y) {}
        public Line (float _x1, float _y1, float _x2, float _y2)
        {
            x1 = _x1;
            y1 = _y1;
            x2 = _x2;
            y2 = _y2;
        }
    }

    // https://stackoverflow.com/questions/4543506/algorithm-for-intersection-of-2-lines

    public static class LineIntersection
    {
        public static bool HasIntersection(Line lineA, Line lineB, out Vector2 vec, float tolerance = 0.0001f)
        {
            vec = Vector2.zero;

            double x1 = lineA.x1, y1 = lineA.y1;
            double x2 = lineA.x2, y2 = lineA.y2;

            double x3 = lineB.x1, y3 = lineB.y1;
            double x4 = lineB.x2, y4 = lineB.y2;
            
            if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance && Math.Abs(x1 - x3) < tolerance) return false;
            if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance && Math.Abs(y1 - y3) < tolerance) return false;
            if (Math.Abs(x1 - x2) < tolerance && Math.Abs(x3 - x4) < tolerance) return false;
            if (Math.Abs(y1 - y2) < tolerance && Math.Abs(y3 - y4) < tolerance) return false;

            double x, y;

            if (Math.Abs(x1 - x2) < tolerance)
            {
                double m2 = (y4 - y3) / (x4 - x3);
                double c2 = -m2 * x3 + y3;
                x = x1;
                y = c2 + m2 * x1;
            }
            else if (Math.Abs(x3 - x4) < tolerance)
            {
                double m1 = (y2 - y1) / (x2 - x1);
                double c1 = -m1 * x1 + y1;
                x = x3;
                y = c1 + m1 * x3;
            }
            else
            {
                double m1 = (y2 - y1) / (x2 - x1);
                double c1 = -m1 * x1 + y1;

                double m2 = (y4 - y3) / (x4 - x3);
                double c2 = -m2 * x3 + y3;

                x = (c1 - c2) / (m2 - m1);
                y = c2 + m2 * x;

                if (!(Math.Abs(-m1 * x + y - c1) < tolerance
                    && Math.Abs(-m2 * x + y - c2) < tolerance))
                    {
                        return false;
                    }
            }
            if (IsInsideLine(lineA, x, y) &&
                IsInsideLine(lineB, x, y))
                {
                    vec =  new Vector2 { x = (float) x, y = (float) y };
                    return true;
                }
                return false;
        }
        private static bool IsInsideLine(Line line, double x, double y)
        {
            return (x > line.x1 && x < line.x2
                        || x > line.x2 && x < line.x1)
                && (y > line.y1 && y < line.y2
                        || y > line.y2 && y < line.y1);
        }
    }

    // https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }

    // https://forum.unity.com/threads/change-gameobject-layer-at-run-time-wont-apply-to-child.10091/
    
     
    public static class IListExtensions {
        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> ts) {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i) {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
    }

    public static class BoxUtils{

        public static int ConvertTo01 (int value)
        {
            if(value == 0) return 0;
            if(value > 0) return 1;
            return -1;
        }
        public static void SetLayerRecursively(
            GameObject obj,
            int newLayer)
        {
            if (null == obj)
                return;
        
            obj.layer = newLayer;
        
            foreach (Transform child in obj.transform)
            {
                if (null == child)
                {
                    continue;
                }
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
        public static void SetTagRecursively(
            GameObject obj,
            string newTag)
        {
            if (null == obj)
                return;
        
            obj.tag = newTag;
        
            foreach (Transform child in obj.transform)
            {
                if (null == child)
                {
                    continue;
                }
                SetTagRecursively(child.gameObject, newTag);
            }
        }
    }
}

using DG.Tweening;
using UnityEngine;

namespace Extensions
{
    public static class Extensionss
    {
        /// <summary>
        /// Method to set specific elements of vector
        /// </summary>
        public static Vector3 With(this Vector3 v, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(
                x ?? v.x,
                y ?? v.y,
                z ?? v.z
            );
        }

        /// <summary>
        /// Method to set specific elements of vector
        /// </summary>
        public static Vector2 With(this Vector2 v, float? x = null, float? y = null)
        {
            return new Vector2(
                x ?? v.x,
                y ?? v.y
            );
        }

        /// <summary>
        /// Equals to new Vector2(v.x, v.z)
        /// </summary>
        public static Vector2 ToXZ(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }

        /// <summary>
        /// Equals to new Vector3(v.x, y, v.y)
        /// </summary>
        public static Vector3 FromXZ(this Vector2 v, float y = 0)
        {
            return new Vector3(v.x, y, v.y);
        }

        /// <summary>
        /// Inverse lerp for vectors
        /// </summary>
        public static float InverseLerp(Vector3 a, Vector3 b, Vector3 v)
        {
            Vector3 ab = b - a;
            Vector3 av = v - a;
            return Vector3.Dot(av, ab) / Vector3.Dot(ab, ab);
        }

        /// <summary>
        /// Lerps between v.x and v.y
        /// </summary>
        public static float LerpInside(this Vector2 v, float t)
        {
            return Mathf.Lerp(v.x, v.y, t);
        }

        public static Tween Wait(float dur)
        {
            return DOTween.To(() => 0, _ => { }, 0, dur);
        }
    }
}
using UnityEngine;

namespace Helpers.Extensions
{
    public static class RigidbodyExtensions
    {
        public static void ResetVelocity(this Rigidbody rb)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        public static void SetDrag(this Rigidbody rb, int value)
        {
            rb.drag = value;
            rb.angularDrag = value;
        }
    }
}
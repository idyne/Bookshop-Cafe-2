using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames
{
    public class CameraFollow : MonoBehaviour
    {
        public static CameraFollow Instance = null;
        public bool UseFixedUpdate = false;
        public Transform Target = null;
        public Vector3 Offset = Vector3.zero;
        public Vector3 rotation = Vector3.zero;
        public Vector3 Speed = Vector3.one;
        [SerializeField] private bool freezeX = false;
        [SerializeField] private bool freezeY = false;
        [SerializeField] private bool freezeZ = false;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
            {
                DestroyImmediate(gameObject);
                return;
            }
        }

        private void FixedUpdate()
        {
            if (UseFixedUpdate && Target)
                Follow();
        }

        private void LateUpdate()
        {
            if (!UseFixedUpdate && Target)
                Follow();
        }

        private void Follow()
        {
            Vector3 pos = Target.position + Offset;
            if (freezeX)
                pos.x = transform.position.x;
            if (freezeY)
                pos.y = transform.position.y;
            if (freezeZ)
                pos.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, new Vector3(pos.x, transform.position.y, transform.position.z), Speed.x * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, pos.y, transform.position.z), Speed.y * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, pos.z), Speed.z * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), Time.deltaTime * 7);
        }

        public void TakePosition()
        {
            Vector3 pos = Target.position + Offset;
            if (freezeX)
                pos.x = transform.position.x;
            if (freezeY)
                pos.y = transform.position.y;
            if (freezeZ)
                pos.z = transform.position.z;
            transform.position = pos;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }

}

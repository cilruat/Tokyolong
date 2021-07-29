using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Festival
{

    public class TargetTracker : MonoBehaviour
    {

        protected Transform tr;

        public Transform target;
        public Vector3 margin;
        public float trackingSpeed = 10f;

        void Awake()
        {
            tr = GetComponent<Transform>();
        }

        void Update()
        {
            if (target == null)
                return;

            Tracking();
        }

        public virtual void SetTarget(Transform _target, Vector3 _pos, Vector3 _margin)
        {
            target = _target;
            tr.position = _pos;
            margin = _margin;
        }

        protected virtual void Tracking()
        {

        }
    }
}
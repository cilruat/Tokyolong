using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Festival
{

    public class UITargetTracker : TargetTracker
    {
        [SerializeField]
        private bool isWorldTarget = true;

        protected override void Tracking()
        {
            if (isWorldTarget)
            {
                tr.position = Vector3.Lerp(tr.position, Camera.main.WorldToScreenPoint(target.position + margin), Time.deltaTime * trackingSpeed);
            }
            else
            {
                tr.position = Vector3.Lerp(tr.position, target.position + margin, Time.deltaTime * trackingSpeed);
            }
        }

    }
}
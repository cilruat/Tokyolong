using UnityEngine;
using System;

namespace StackGameTemplate
{
	/// <summary>
	/// This class fades out the color of an object over time
	/// </summary>
	public class STKFadeOut:MonoBehaviour
	{
        // The mesh renderer which will be faded out
        internal MeshRenderer meshRenderer;

        // The default transparency we start from. 1 means totally opaque, 0 means totally transparent
        internal float transparency = 1;

        // How quickly the color fades out
        public float fadeSpeed = 3;

        void Start()
        {
            // Cache the mesh renderer for quicker access
            meshRenderer = GetComponent<MeshRenderer>();
        }

        void Update()
        {
            // Animate the fading out of the color
            if ( meshRenderer.materials[0]) meshRenderer.material.color = new Color(1,1,1, transparency);
            if ( meshRenderer.materials[1] )    meshRenderer.materials[1].color = new Color(1, 1, 1, transparency);

            // Count down the fade out time
            transparency -= Time.deltaTime * fadeSpeed;
        }
	}
}
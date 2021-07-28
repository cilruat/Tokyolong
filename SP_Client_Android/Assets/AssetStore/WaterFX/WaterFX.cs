//
//  Created by Olivier VENERI on 05/04/2016.
//  Copyright (c) 2016 FenschValleyGames. All rights reserved.
//

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

[ExecuteInEditMode]
public class WaterFX : MonoBehaviour {
	public enum QualityLevel
	{
		E_MAX = 1,
		E_BEST = 2,
		E_MEDIUM = 4,
		E_LOW = 8
	}


	
	public Camera 		m_targetCamera	= null;
	public LayerMask 	m_cullingMask		= -1;

	public string 		m_sortingLayer = "Default";
	public int 			m_sortingOrderInLayer = 0;

	public QualityLevel m_qualityLevel	= QualityLevel.E_BEST;
	public float 		m_yPosition 	= 0.0f;
	public float 		m_zOffset		= 0.0f;

	[Range(0.0f,1.0f)]
	public float 		m_distorsionAmount	= 0.1f;
	public bool			m_perspectiveLink	= false;
	public bool			m_pixelated			= false;

	[Range(-50.0f,50.0f)]
	public float 		m_waterSpeed1		= 0.4f;

	[Range(-50.0f,50.0f)]
	public float 		m_waterSpeed2				= 0.2f;
	public Texture2D 	m_displacementMap1			= null;
	public Texture2D 	m_displacementMap2			= null;

	[Range(0.0f,1.0f)]
	public float 		m_transparency	= 1.0f;
	public Color 		m_waterTint		= Color.white;

	RenderTexture		m_waterPlane 	= null;
	FilterMode			m_displacementFilterMode	= FilterMode.Bilinear;
	[Range(0.0f,1.0f)]
	public float 		m_waterHeight = 0.5f;
	

	float 				m_borderIncreaseCoef = 1.2f;
	float 				m_orthographicWidth = 0.0f;
	GameObject			m_waterObject 		= null;
	public bool 		m_horizontalFlip 	= false;
	public bool 		m_verticalFlip 		= true;
	Camera 				m_cameraComponent	= null;
	// Screen quad stuff
	MeshFilter			m_screenQuadMeshFilter		= null;
	Mesh				m_screenQuadMesh			= null;
	MeshRenderer		m_screenQuadMeshRenderer 	= null;
	Material 			m_screenQuadMat 			= null;	
	
	Color				m_screenQuadColor 				= Color.white;
	Vector3[]			m_screenQuadVertices 			= new Vector3[4];
	Vector3[]			m_screenQuadNormals				= new Vector3[4];
	Vector2[]			m_screenQuadUVs 				= new Vector2[4];
	int[]				m_screenQuadVtxIndices			= new int[6];
	Color[]				m_screenQuadVtxColors			= new Color[4];
	
	// Use this for initialization
	void Start () {
		m_cameraComponent = GetComponent<Camera>();
		if(m_cameraComponent == null)
		{
			m_cameraComponent 	= gameObject.AddComponent<Camera>();
		}
		m_cameraComponent.hideFlags = HideFlags.HideInInspector;
		gameObject.transform.hideFlags = HideFlags.HideInInspector;
		
		if(m_targetCamera != null)
		{
			if(!m_targetCamera.orthographic)
			{
				Debug.LogWarning("Camera added but should be orthographic to work as intended.", gameObject);
			}
			
			// Get or create the water plane game object
			m_waterObject = GameObject.Find("WaterPlane");
			if(m_waterObject == null)
			{
				m_waterObject = new GameObject("WaterPlane");
			}
			m_waterObject.hideFlags = HideFlags.HideInHierarchy;
			m_waterObject.transform.parent = gameObject.transform;
			
			// Create the render target that will be used to render the water surface
			int width = (int)m_targetCamera.pixelWidth;
			int height = (int)m_targetCamera.pixelHeight;
			m_waterPlane = new RenderTexture(width/(int)m_qualityLevel, height/(int)m_qualityLevel, 16, RenderTextureFormat.ARGB32);
			m_waterPlane.Create();
			
			Vector3 oldPosition = m_cameraComponent.transform.position;
			m_cameraComponent.CopyFrom(m_targetCamera);
			m_cameraComponent.transform.position = oldPosition;
			m_cameraComponent.targetTexture = m_waterPlane;
			m_cameraComponent.cullingMask = m_cullingMask;
			
			m_cameraComponent.orthographicSize =  m_targetCamera.orthographicSize * m_borderIncreaseCoef; // increase water camera a little bit to avoid glitches at the borders
			// Create screen quad stuff
			m_screenQuadMeshFilter	= m_waterObject.GetComponent<MeshFilter>();
			if(m_screenQuadMeshFilter == null)
			{
				m_screenQuadMeshFilter 	= m_waterObject.AddComponent<MeshFilter>();
			}
			m_screenQuadMeshFilter.hideFlags = HideFlags.HideInInspector;
			
			m_screenQuadMesh 				= new Mesh();
			m_screenQuadMesh.MarkDynamic();
			m_screenQuadMeshFilter.sharedMesh	= m_screenQuadMesh;
			
			
			m_screenQuadMeshRenderer = m_waterObject.GetComponent<MeshRenderer>();
			if(m_screenQuadMeshRenderer == null)
			{
				m_screenQuadMeshRenderer = m_waterObject.AddComponent<MeshRenderer>();
			}
			m_screenQuadMeshRenderer.hideFlags = HideFlags.HideInInspector;
			m_screenQuadMeshRenderer.sortingLayerName = m_sortingLayer;
			m_screenQuadMeshRenderer.sortingOrder = m_sortingOrderInLayer;
			
			// Create the screen quad material
			m_screenQuadMat = new Material(Shader.Find("Unlit/WaterShader"));
			m_screenQuadMat.hideFlags = HideFlags.HideInInspector;
			m_screenQuadMeshRenderer.sharedMaterial 	= m_screenQuadMat;
			m_screenQuadMat.mainTexture = m_waterPlane;
			
			if(m_displacementMap1!=null)
			{
				m_screenQuadMat.SetTexture("_DispMap1",m_displacementMap1);
			}
			else
			{
				Debug.LogError("Water effect added but no displacement map 1 set.", gameObject);
			}
			
			if(m_displacementMap2!=null)
			{
				m_screenQuadMat.SetTexture("_DispMap2",m_displacementMap2);
			}
			else
			{
				Debug.LogError("Water effect added but no displacement map 2 set.", gameObject);
			}
			
			UpdateScreenQuad();
		}
		else
		{
			Debug.LogError("Water effect added but no target camera set.", gameObject);
		}
		
	}
	
	void UpdateScreenQuad()
	{
		if(m_cameraComponent == null || m_targetCamera == null || m_screenQuadVertices == null || m_screenQuadNormals == null || m_screenQuadUVs == null || m_screenQuadVtxIndices == null || m_screenQuadVtxColors == null || m_screenQuadMesh == null)
		{
			return;
		}
		
		if(m_pixelated)
		{
			m_displacementFilterMode = FilterMode.Point;
		}
		else
		{
			m_displacementFilterMode = FilterMode.Bilinear;
		}
		
		if(m_displacementMap1!=null)
		{
			m_displacementMap1.filterMode = m_displacementFilterMode;
		}
		
		if(m_displacementMap2!=null)
		{
			m_displacementMap2.filterMode = m_displacementFilterMode;
		}

		m_cameraComponent.orthographicSize =  m_targetCamera.orthographicSize * m_borderIncreaseCoef; 
		m_cameraComponent.transform.position = new Vector3(m_targetCamera.transform.position.x,m_yPosition,m_targetCamera.transform.position.z+m_targetCamera.nearClipPlane);
		m_waterObject.transform.position = new Vector3(m_cameraComponent.transform.position.x,m_yPosition,m_cameraComponent.transform.position.z+m_zOffset);
		float halfOrthographicWidth = m_cameraComponent.orthographicSize *  m_cameraComponent.aspect;
		m_orthographicWidth = halfOrthographicWidth * 2.0f;
		float halfOrthographicHeight = m_cameraComponent.orthographicSize; 
		
		// upper left
		m_screenQuadVertices[0].x = - halfOrthographicWidth ;
		m_screenQuadVertices[0].y = halfOrthographicHeight;
		m_screenQuadVertices[0].z = 0;
		// upper right
		m_screenQuadVertices[1].x = halfOrthographicWidth ;
		m_screenQuadVertices[1].y = halfOrthographicHeight;
		m_screenQuadVertices[1].z = 0;
		// bottom right
		m_screenQuadVertices[2].x = halfOrthographicWidth ;
		m_screenQuadVertices[2].y = - halfOrthographicHeight;
		m_screenQuadVertices[2].z = 0;
		// bottom left
		m_screenQuadVertices[3].x = - halfOrthographicWidth ;
		m_screenQuadVertices[3].y = - halfOrthographicHeight;
		m_screenQuadVertices[3].z = 0;
		// upper left
		m_screenQuadNormals[0].x = 0.0f;
		m_screenQuadNormals[0].y = 0.0f;
		m_screenQuadNormals[0].z = -1.0f;
		// upper right
		m_screenQuadNormals[1].x = 0.0f;
		m_screenQuadNormals[1].y = 0.0f;
		m_screenQuadNormals[1].z = -1.0f;
		// bottom right
		m_screenQuadNormals[2].x = 0.0f;
		m_screenQuadNormals[2].y = 0.0f;
		m_screenQuadNormals[2].z = -1.0f;
		// bottom left
		m_screenQuadNormals[3].x = 0.0f;
		m_screenQuadNormals[3].y = 0.0f;
		m_screenQuadNormals[3].z = -1.0f;
		//
		// Set the UVs
		float UVMin = 0.0f;
		float UVMax = 1.0f;
		if(m_verticalFlip)
		{
			if(m_horizontalFlip)
			{
				// upper left
				m_screenQuadUVs[0].x = UVMax;
				m_screenQuadUVs[0].y = UVMin; 
				// upper right
				m_screenQuadUVs[1].x = UVMin;
				m_screenQuadUVs[1].y = UVMin;
				// bottom right
				m_screenQuadUVs[2].x = UVMin;
				m_screenQuadUVs[2].y = UVMax;
				// bottom left
				m_screenQuadUVs[3].x = UVMax;
				m_screenQuadUVs[3].y = UVMax;
			}
			else
			{
				// upper left
				m_screenQuadUVs[0].x = UVMin;
				m_screenQuadUVs[0].y = UVMin; 
				// upper right
				m_screenQuadUVs[1].x = UVMax;
				m_screenQuadUVs[1].y = UVMin;
				// bottom right
				m_screenQuadUVs[2].x = UVMax;
				m_screenQuadUVs[2].y = UVMax;
				// bottom left
				m_screenQuadUVs[3].x = UVMin;
				m_screenQuadUVs[3].y = UVMax;
			}	
		}
		else
		{
			if(m_horizontalFlip)
			{
				// upper left
				m_screenQuadUVs[0].x = UVMax;
				m_screenQuadUVs[0].y = UVMax; 
				// upper right
				m_screenQuadUVs[1].x = UVMin;
				m_screenQuadUVs[1].y = UVMax;
				// bottom right
				m_screenQuadUVs[2].x = UVMin;
				m_screenQuadUVs[2].y = UVMin;
				// bottom left
				m_screenQuadUVs[3].x = UVMax;
				m_screenQuadUVs[3].y = UVMin;
			}
			else
			{
				// upper left
				m_screenQuadUVs[0].x = UVMin;
				m_screenQuadUVs[0].y = UVMax; 
				// upper right
				m_screenQuadUVs[1].x = UVMax;
				m_screenQuadUVs[1].y = UVMax;
				// bottom right
				m_screenQuadUVs[2].x = UVMax;
				m_screenQuadUVs[2].y = UVMin;
				// bottom left
				m_screenQuadUVs[3].x = UVMin;
				m_screenQuadUVs[3].y = UVMin;
			}
		}
		
		//
		m_screenQuadVtxIndices[0]	= 0;
		m_screenQuadVtxIndices[1]	= 1;
		m_screenQuadVtxIndices[2]	= 3;
		m_screenQuadVtxIndices[3]	= 1;
		m_screenQuadVtxIndices[4]	= 2;
		m_screenQuadVtxIndices[5]	= 3;
		// 
		m_screenQuadVtxColors[0] 	= m_screenQuadColor;
		m_screenQuadVtxColors[1] 	= m_screenQuadColor;
		m_screenQuadVtxColors[2] 	= m_screenQuadColor;
		m_screenQuadVtxColors[3] 	= m_screenQuadColor;
		
		//
		m_screenQuadMesh.vertices	= m_screenQuadVertices;
		m_screenQuadMesh.colors		= m_screenQuadVtxColors;
		//m_screenQuadMesh.normals	= m_screenQuadNormals;
		m_screenQuadMesh.uv			= m_screenQuadUVs;
		m_screenQuadMesh.triangles	= m_screenQuadVtxIndices;
	}

	void EditorUpdate()
	{
		if(m_screenQuadMeshRenderer != null)
		{
			m_screenQuadMeshRenderer.sortingLayerName = m_sortingLayer;
			m_screenQuadMeshRenderer.sortingOrder = m_sortingOrderInLayer;
		}
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		EditorUpdate();
		#endif
		UpdateScreenQuad();
		
		if(m_screenQuadMat != null && m_targetCamera != null)
		{
			// Update shader properties
			m_screenQuadMat.SetFloat("_ReflDistort", m_distorsionAmount);
			m_screenQuadMat.SetColor("_WaterTint", m_waterTint);
			m_screenQuadMat.SetFloat("_WaterSpeed1", m_waterSpeed1);
			m_screenQuadMat.SetFloat("_WaterSpeed2", m_waterSpeed2);
			m_screenQuadMat.SetFloat("_WaterHeight", 1.0f-m_waterHeight);
			m_screenQuadMat.SetFloat("_xPositionOffset", 1.0f+((m_targetCamera.transform.position.x)/m_orthographicWidth)/2.0f);
			m_screenQuadMat.SetFloat("_transparency",m_transparency);
			m_screenQuadMat.SetFloat("_distPerspectiveLink", m_perspectiveLink ? 0.0f : 1.0f);
		}
	}
	
	#if UNITY_EDITOR
	// Add a menu item to create a Water FX. 
	[MenuItem("Water FX/Create Water FX", false, 10)]
	static void CreateWaterFx(MenuCommand menuCommand) {
		// Create a custom game object
		GameObject go = GameObject.Find("WaterFX");
		if(go != null)
		{
			return;
		}
		
		go = new GameObject("WaterFX");
		go.AddComponent<WaterFX>();
		// Ensure it gets reparented if this was a context click (otherwise does nothing)
		GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
		// Register the creation in the undo system
		Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
		Selection.activeObject = go;
	}
	#endif // UNITY_EDITOR
}

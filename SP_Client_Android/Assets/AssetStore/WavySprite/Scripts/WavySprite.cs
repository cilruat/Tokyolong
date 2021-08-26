using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WavySprite:MonoBehaviour{

	[SerializeField]
	private bool isCanvas=false;
	[SerializeField]
	private MeshRenderer mr;
	[SerializeField]
	private CanvasRenderer cr;
	[SerializeField]
	private MeshFilter mf;
	[SerializeField]
	private Mesh mesh;
	[SerializeField]
	private Material mat;
	[SerializeField]
	private List<Vector3> vertices=new List<Vector3>(200);
	[SerializeField]
	private List<Vector3> uvs=new List<Vector3>(200);
	[SerializeField]
	private List<Color> colors=new List<Color>(200);
	[SerializeField]
	private int[] triangles;
	[SerializeField]
	private int trianglesCount;

	public Texture2D texture;
	[SerializeField]
	private Texture2D _texture;

	public Color tint=Color.white;
	[SerializeField]
	private Color _tint;

	[Range(0,100)]
	public int divisionsX=30;
	[SerializeField]
	private int _divisionsX;

	[Range(0,100)]
	public int divisionsY=30;
	[SerializeField]
	private int _divisionsY;

	[System.Serializable]
	public enum pivotPositions{Center,Top,Right,Bottom,Left}
	public pivotPositions pivotPosition=pivotPositions.Bottom;
	[SerializeField]
	private pivotPositions _pivotPosition=(pivotPositions)(-1);
	[SerializeField]
	public bool manualPivot=false;
	
	public Vector2 pivotOffset=new Vector2(-0.5f,0);
	[SerializeField]
	private Vector2 _pivotOffset=new Vector2(-0.5f,0);

	public enum waveDirections{Vertical,Horizontal};
	public waveDirections waveDirection=waveDirections.Vertical;
	[SerializeField]
	private waveDirections? _waveDirection=null;

	public enum objSides{Top,Right,Bottom,Left,TopBottom,LeftRight,None}
	public objSides staticSide=objSides.Bottom;
	[SerializeField]
	private objSides? _staticSide=null;

	[Range(-100,100)]
	public float waveFrequency=10f;
	[SerializeField]
	private float _waveFrequency;

	[Range(0f,10f)]
	public float waveForce=0.03f;
	[SerializeField]
	private float _waveForce;

	[Range(-100f,100f)]
	public float waveSpeed=1f;
	[SerializeField]
	private float _waveSpeed;

	[Range(-100f,100f)]
	public float textureSpeed=0f;
	[SerializeField]
	private float _textureSpeed;

	[SerializeField]
	private float meshWidth=1f,meshHeight=1f;

	public bool lit=false;
	[SerializeField]
	private bool _lit;

	public int sortingLayer=0;
	[SerializeField]
	private int _sortingLayer;

	[SerializeField]
	public int orderInLayer=0;
	[SerializeField]
	private int _orderInLayer=0;

	private bool forceUpdate=false;

	private void Awake(){
		isCanvas=isChildOfCanvas(transform);
		CreateComponents();
	}

	[ContextMenu("WavySprite online manual...")]
	void OpenURLOnlineManual(){
		Application.OpenURL("http://ax23w4.com/devlog/wavysprite");
	}

	[ContextMenu("Rate this asset on the Asset Store...")]
	void OpenURLRateThisAsset(){
		Application.OpenURL("http://u3d.as/PWu");
	}

	[ContextMenu("Other assets by Andrii Sudyn...")]
	void OpenURLOtherAssets(){
		Application.OpenURL("https://assetstore.unity.com/publishers/26071");
	}

	public void CreateComponents(){
		if(isCanvas && cr==null){
			#if UNITY_EDITOR
				DestroyImmediate(GetComponent<MeshFilter>());
				DestroyImmediate(GetComponent<MeshRenderer>());
			#else
				Destroy(GetComponent<MeshFilter>());
				Destroy(GetComponent<MeshRenderer>());
			#endif
			if(GetComponent<CanvasRenderer>()==null) gameObject.AddComponent<CanvasRenderer>();
			cr=GetComponent<CanvasRenderer>();
		}else if(!isCanvas && (mf==null || mr==null)){
			#if UNITY_EDITOR
				DestroyImmediate(GetComponent<CanvasRenderer>());
			#else
				Destroy(GetComponent<CanvasRenderer>());
			#endif
			if(GetComponent<MeshFilter>()==null) gameObject.AddComponent<MeshFilter>();
			if(GetComponent<MeshRenderer>()==null) gameObject.AddComponent<MeshRenderer>();
			mf=GetComponent<MeshFilter>();
			mr=GetComponent<MeshRenderer>();
		}
		forceUpdate=true;
	}

	void OnDrawGizmos(){
		#if UNITY_EDITOR
		if(!Application.isPlaying){
			UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
			UnityEditor.SceneView.RepaintAll();
		}
		#endif
	}

	void Update(){
		#if UNITY_EDITOR
		if(transform.hasChanged){
			if(isCanvas!=isChildOfCanvas(transform)){
				isCanvas=!isCanvas;
				CreateComponents();
				#if UNITY_EDITOR
					DestroyImmediate(mat);
				#else
					Destroy(mat);
				#endif
			}
		}
		#endif
		if(
			_texture!=texture || 
			_tint!=tint ||
			_divisionsX!=divisionsX || 
			_divisionsY!=divisionsY || 
			_pivotPosition!=pivotPosition || 
			_waveDirection!=waveDirection || 
			_staticSide!=staticSide || 
			_waveFrequency!=waveFrequency || 
			_waveForce!=waveForce ||
			_waveSpeed!=waveSpeed ||
			_textureSpeed!=textureSpeed ||
			_lit!=lit ||
			_sortingLayer!=sortingLayer ||
			_orderInLayer!=orderInLayer ||
			mat==null ||
			forceUpdate
		){
			//If lit setting has changed, destroy material forcing to recreate it
			if(_lit!=lit){
				#if UNITY_EDITOR
					DestroyImmediate(mat);
				#else
					Destroy(mat);
				#endif
				_lit=lit;
			}
			//If there's no material, we create it and assign it to the component we're using
			if(mat==null){
				if(!lit) mat=new Material(Shader.Find("Custom/WavySpriteUnlit")){name="WavySpriteUnlitMaterial"};
				else mat=new Material(Shader.Find("Custom/WavySpriteLit")){name="WavySpriteLitMaterial"};
				forceUpdate=true;
			}
			//Take care of all material settings
			if(_waveDirection!=waveDirection || forceUpdate){
				mat.SetFloat("_WaveDirection",waveDirection==waveDirections.Vertical?0:1);
				_waveDirection=waveDirection;
			}
			if(_staticSide!=staticSide || forceUpdate){
				if(staticSide==objSides.Top) mat.SetFloat("_StaticSide",1);
				if(staticSide==objSides.Right) mat.SetFloat("_StaticSide",2);
				if(staticSide==objSides.Bottom) mat.SetFloat("_StaticSide",3);
				if(staticSide==objSides.Left) mat.SetFloat("_StaticSide",4);
				if(staticSide==objSides.TopBottom) mat.SetFloat("_StaticSide",5);
				if(staticSide==objSides.LeftRight) mat.SetFloat("_StaticSide",6);
				if(staticSide==objSides.None) mat.SetFloat("_StaticSide",0);
				_staticSide=staticSide;
			}
			if(_texture!=texture || forceUpdate){
				mat.SetTexture("_MainTex",texture);
				if(texture!=null){
					if(texture.width>texture.height){
						meshWidth=1f;
						meshHeight=(float)texture.height/(float)texture.width;
					}else{
						meshWidth=(float)texture.width/(float)texture.height;
						meshHeight=1f;
					}
				}else{
					meshWidth=1f;
					meshHeight=1f;
				}
				_texture=texture;
			}
			mat.SetColor("_Color",tint);
			mat.SetFloat("_WaveFrequency",waveFrequency);
			mat.SetFloat("_WaveForce",waveForce);
			mat.SetFloat("_WaveSpeed",waveSpeed);
			mat.SetFloat("_TextureSpeed",textureSpeed);
			_waveFrequency=waveFrequency;
			_waveForce=waveForce;
			_waveSpeed=waveSpeed;
			_textureSpeed=textureSpeed;
			if(forceUpdate){
				if(isCanvas){
					cr.SetMaterial(mat,null);
				}else{
					mr.sharedMaterial=mat;
				}
			}
			_tint=tint;
			if(_sortingLayer!=sortingLayer || _orderInLayer!=orderInLayer || forceUpdate){
				if(mr!=null) {
					mr.sortingLayerID=sortingLayer;
					mr.sortingOrder=orderInLayer;
				}
				_sortingLayer=sortingLayer;
				_orderInLayer=orderInLayer;
			}
			_divisionsX=divisionsX;
			_divisionsY=divisionsY;

			if(_pivotPosition!=pivotPosition || forceUpdate){
				if(pivotPosition==pivotPositions.Center) pivotOffset=new Vector2(-0.5f,-0.5f);
				if(pivotPosition==pivotPositions.Top) pivotOffset=new Vector2(-0.5f,-1f);
				if(pivotPosition==pivotPositions.Right) pivotOffset=new Vector2(-1f,-0.5f);
				if(pivotPosition==pivotPositions.Bottom) pivotOffset=new Vector2(-0.5f,0f);
				if(pivotPosition==pivotPositions.Left) pivotOffset=new Vector2(0f,-0.5f);
				Vector2 pivotDelta=_pivotOffset-pivotOffset;
				Vector2 meshSize=transform.TransformVector(new Vector3(meshWidth,meshHeight,0));
				transform.position+=new Vector3(pivotDelta.x*meshSize.x,pivotDelta.y*meshSize.y,0f);
				//Update object's settings
				manualPivot=false;
				_pivotPosition=pivotPosition;
				_pivotOffset=pivotOffset;
				/*
				#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(this);
				#endif
				*/
			}

			forceUpdate=false;
			GenerateMesh();
		}
	}

	public void SetOffset(Vector2 newOffset){
		pivotPosition=(pivotPositions)(-1);
		pivotOffset-=new Vector2(newOffset.x/meshWidth,newOffset.y/meshHeight);
		forceUpdate=true;
		GenerateMesh();
	}

	void GenerateMesh(){
		int pointsX=divisionsX+2;
		int pointsY=divisionsY+2;
		int verticeNum=0;
		int squareNum=-1;
		vertices.Clear();
		uvs.Clear();
		colors.Clear();
		triangles=new int[(((divisionsX+1)*(divisionsY+1))*2)*3];
		for(int y=0;y<pointsY;y++){
			for(int x=0;x<pointsX;x++){
				vertices.Add(new Vector3(
					(((float)x/(float)(pointsX-1))+pivotOffset.x)*meshWidth,
					(((float)y/(float)(pointsY-1))+pivotOffset.y)*meshHeight,
					0f
				));
				uvs.Add(new Vector3(
					((float)x/(float)(pointsX-1)),
					((float)y/(float)(pointsY-1)),
					0f
				));
				if(x>0 && y>0){
					verticeNum=x+(y*pointsX);
					squareNum++;
					triangles[squareNum*6]=verticeNum-pointsX-1;
					triangles[squareNum*6+1]=verticeNum-1;
					triangles[squareNum*6+2]=verticeNum;
					triangles[squareNum*6+3]=verticeNum;
					triangles[squareNum*6+4]=verticeNum-pointsX;
					triangles[squareNum*6+5]=verticeNum-pointsX-1;
				}
			}
		}
		if(mesh==null){
			mesh=new Mesh{name="WavySpriteMesh"};
		}else{
			mesh.Clear();
		}
		mesh.SetVertices(vertices);
		mesh.SetColors(colors);
		mesh.SetUVs(0,uvs);
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		mesh.SetTriangles(triangles,0);
		trianglesCount=triangles.Length/3;
		if(isCanvas){
			cr.SetMesh(mesh);
		}else{
			mf.sharedMesh=mesh;
		}
	}

	public int triangleCount{
		get{return trianglesCount;}
	}

	private bool isChildOfCanvas(Transform t){
		if(t.GetComponent<Canvas>()!=null){
			return true;
		}else if(t.parent!=null){
			return isChildOfCanvas(t.parent);
		}else{
			return false;
		}
	}

}

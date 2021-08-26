using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System;

[CustomEditor(typeof(WavySprite))]
public class WavySpriteEditor:Editor{

	WavySprite script;

	private Plane objectPlane; //Mostly to position controls using plane's normal

	private bool pivotMoveMode=false; //Tells the script if it's in a mode when you can drag pivot around
	private bool draggingPivot=false; //True when the pivot is being in the process of dragging
	private Vector3 pivotStart; //Starting position of the pivot
	private Tool rememberTool; //Remember Unity's last selected tool

	[MenuItem("GameObject/2D Object/WavySprite")]
	static void Create(){
		GameObject go=new GameObject();
		go.AddComponent<WavySprite>();
		go.name="WavySprite";
		SceneView sc=SceneView.lastActiveSceneView!=null?SceneView.lastActiveSceneView:SceneView.sceneViews[0] as SceneView;
		go.transform.position=new Vector3(sc.pivot.x,sc.pivot.y-0.5f,0f);
		if(Selection.activeGameObject!=null) go.transform.parent=Selection.activeGameObject.transform;
		Selection.activeGameObject=go;
		WavySprite tScript=go.GetComponent<WavySprite>();
		tScript.CreateComponents();
		UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(tScript.GetComponent<MeshFilter>(),false);
		UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(tScript.GetComponent<MeshRenderer>(),false);
		UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(tScript.GetComponent<CanvasRenderer>(),false);
		UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(tScript.GetComponent<PolygonCollider2D>(),false);
	}

	void Awake(){
		script=(WavySprite)target;
	}

	private void OnDestroy(){
		PivotMoveModeOff();
	}

	public override void OnInspectorGUI(){

		//Texture field
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel(new GUIContent("Sprite","An image to use as a texture"));
		Texture2D texture=(Texture2D)EditorGUILayout.ObjectField(script.texture,typeof(Texture2D),false);
		if(script.texture!=texture){
			Undo.RecordObject(script,"Change sprite");
			script.texture=texture;
			EditorUtility.SetDirty(script);
		}
		EditorGUILayout.EndHorizontal();

		//Tint field
		Color tint=EditorGUILayout.ColorField(new GUIContent("Color","Tint the sprite using a color. Use white for no tint."),script.tint);
		if(script.tint!=tint){
			Undo.RecordObject(script,"Change color");
			script.tint=tint;
			EditorUtility.SetDirty(script);
		}

		//Edit X divisions
		int divisionsX=EditorGUILayout.IntSlider(new GUIContent("Divisions X","Number of vertical divisions"),script.divisionsX,0,100);
		if(divisionsX!=script.divisionsX){
			Undo.RecordObject(script,"Change divisions X");
			script.divisionsX=divisionsX;
			EditorUtility.SetDirty(script);
		}

		//Edit X divisions
		int divisionsY=EditorGUILayout.IntSlider(new GUIContent("Divisions Y","Number of horizontal divisions"),script.divisionsY,0,100);
		if(divisionsY!=script.divisionsY){
			Undo.RecordObject(script,"Change divisions Y");
			script.divisionsY=divisionsY;
			EditorUtility.SetDirty(script);
		}









		//Storing padding for toolbar buttons so we can restore it later
		RectOffset oPaddingButtonLeft=GUI.skin.GetStyle("ButtonLeft").padding;
		RectOffset oPaddingButtonMid=GUI.skin.GetStyle("ButtonMid").padding;
		RectOffset oPaddingButtonRight=GUI.skin.GetStyle("ButtonRight").padding;
		//Changing padding for toolbar buttons to fit more stuff
		GUI.skin.GetStyle("ButtonLeft").padding=new RectOffset(0,0,3,3);
		GUI.skin.GetStyle("ButtonMid").padding=new RectOffset(0,0,3,3);
		GUI.skin.GetStyle("ButtonRight").padding=new RectOffset(0,0,3,3);
		
		//Pivot type
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel(new GUIContent("Pivot","Move and configure object's pivot"));

		//Move pivot manually
		GUIStyle pivotButtonStyle=new GUIStyle(GUI.skin.button);
		pivotButtonStyle.padding=new RectOffset(3,3,3,3);
		pivotButtonStyle.stretchWidth=false;
		pivotButtonStyle.fixedWidth=21;
		pivotButtonStyle.fixedHeight=GUI.skin.GetStyle("ButtonMid").CalcHeight(new GUIContent("Text"),10f);

		bool newPivotMoveMode=GUILayout.Toggle(pivotMoveMode,new GUIContent((Texture)Resources.Load("Icons/movePivot"),"Move pivot manually"),pivotButtonStyle);
		if(newPivotMoveMode!=pivotMoveMode){ 
			pivotMoveMode=newPivotMoveMode;
			if(pivotMoveMode){
				PivotMoveModeOn();
			}else{
				PivotMoveModeOff();
			}
		}

		//Pivot position
		GUIStyle pivotTypeButtonStyle=new GUIStyle(GUI.skin.button);
		pivotTypeButtonStyle.padding=new RectOffset(3,3,3,3);
		pivotTypeButtonStyle.fixedHeight=GUI.skin.GetStyle("ButtonMid").CalcHeight(new GUIContent("Text"),10f);
		int pivotPosition=GUILayout.Toolbar(script.manualPivot?-1:(int)script.pivotPosition,EnumToGUI<WavySprite.pivotPositions>("Icons/pivot"),pivotTypeButtonStyle);
		if((script.manualPivot && pivotPosition!=-1) || (!script.manualPivot && pivotPosition!=(int)script.pivotPosition)){
			PivotMoveModeOff();
			script.manualPivot=false;
			script.pivotPosition=(WavySprite.pivotPositions)pivotPosition;
			EditorUtility.SetDirty(script);
		}
		EditorGUILayout.EndHorizontal();

		//Restoring original button padding
		GUI.skin.GetStyle("ButtonLeft").padding=oPaddingButtonLeft;
		GUI.skin.GetStyle("ButtonMid").padding=oPaddingButtonMid;
		GUI.skin.GetStyle("ButtonRight").padding=oPaddingButtonRight;

		//Show triangle count
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(EditorGUIUtility.labelWidth);
		GUILayout.Box(new GUIContent("Triangle count: "+script.triangleCount.ToString()),EditorStyles.helpBox);
		EditorGUILayout.EndHorizontal();

		//Wave direction
		WavySprite.waveDirections waveDirection=(WavySprite.waveDirections)EditorGUILayout.EnumPopup(new GUIContent("Wave direction","Direction of wave movement"),script.waveDirection);
		if(waveDirection!=script.waveDirection){
			Undo.RecordObject(script,"Change wave direction");
			script.waveDirection=waveDirection;
			EditorUtility.SetDirty(script);
		}

		//Static side
		WavySprite.objSides staticSide=(WavySprite.objSides)EditorGUILayout.EnumPopup(new GUIContent("Static side","Which side to keep unmoving"),script.staticSide);
		if(staticSide!=script.staticSide){
			Undo.RecordObject(script,"Change static side");
			script.staticSide=staticSide;
			EditorUtility.SetDirty(script);
		}

		//Wave frequency
		float waveFrequency=EditorGUILayout.Slider(new GUIContent("Wave frequency","How many waves to fit into a unit of space"),script.waveFrequency,-100f,100f);
		if(waveFrequency!=script.waveFrequency){
			Undo.RecordObject(script,"Changed wave frequency");
			script.waveFrequency=waveFrequency;
			EditorUtility.SetDirty(script);
		}

		//Wave force (size)
		float waveForce=EditorGUILayout.Slider(new GUIContent("Wave size","How big is the difference between wave's high and low"),script.waveForce,0f,100f);
		if(waveForce!=script.waveForce){
			Undo.RecordObject(script,"Changed wave size");
			script.waveForce=waveForce;
			EditorUtility.SetDirty(script);
		}

		//Wave speed
		float waveSpeed=EditorGUILayout.Slider(new GUIContent("Wave speed","How fast the waves will move"),script.waveSpeed,-100f,100f);
		if(waveSpeed!=script.waveSpeed){
			Undo.RecordObject(script,"Changed wave speed");
			script.waveSpeed=waveSpeed;
			EditorUtility.SetDirty(script);
		}

		GUILayout.Space(10);

		//Texture speed
		float textureSpeed=EditorGUILayout.Slider(new GUIContent("Texture speed","How fast to move the texture. Remember to set Wrap mode to Repeat for your image"),script.textureSpeed,-100f,100f);
		if(textureSpeed!=script.textureSpeed){
			Undo.RecordObject(script,"Changed texture speed");
			script.textureSpeed=textureSpeed;
			EditorUtility.SetDirty(script);
		}
		
		GUILayout.Space(10);

		//Allow to switch between lit and unlit material
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel(new GUIContent("Sprite material","The type of material to use for this object"));
		GUIContent[] buttonsMaterialType=new GUIContent[]{new GUIContent("Unlit"),new GUIContent("Lit")};
		int switchState=GUILayout.Toolbar(script.lit?1:0,buttonsMaterialType);
		if(switchState!=(script.lit?1:0)){
			GUI.FocusControl(null);
			script.lit=(switchState==0?false:true);
		}
		EditorGUILayout.EndHorizontal();

		//Get sorting layers
		int[] layerIDs=GetSortingLayerUniqueIDs();
		string[] layerNames=GetSortingLayerNames();
		//Get selected sorting layer
		int selected=-1;
		for(int i=0;i<layerIDs.Length;i++){
			if(layerIDs[i]==script.sortingLayer){
				selected=i;
			}
		}
		//Select Default layer if no other is selected
		if(selected==-1){
			for(int i=0;i<layerIDs.Length;i++){
				if(layerIDs[i]==0){
					selected=i;
				}
			}
		}
		//Sorting layer dropdown
		EditorGUI.BeginChangeCheck();
		selected=EditorGUILayout.Popup("Sorting Layer",selected,layerNames);
		if(EditorGUI.EndChangeCheck()){
			Undo.RecordObject(script,"Change sorting layer");
			script.sortingLayer=layerIDs[selected];
			EditorUtility.SetDirty(script);
		}
		//Order in layer field
		EditorGUI.BeginChangeCheck();
		int order=EditorGUILayout.IntField("Order in Layer",script.orderInLayer);
		if(EditorGUI.EndChangeCheck()){
			Undo.RecordObject(script,"Change order in layer");
			script.orderInLayer=order;
			EditorUtility.SetDirty(script);
		}
	}

	private void PivotMoveModeOn(){
		script.manualPivot=true;
		rememberTool=Tools.current;
		Tools.current=Tool.None;
		pivotStart=script.transform.position;
		pivotMoveMode=true;
	}

	private void PivotMoveModeOff(){ 
		if(rememberTool==Tool.None) rememberTool=Tool.Move;
		if(Tools.current==Tool.None) Tools.current=rememberTool;
		pivotMoveMode=false;
	}

    void OnSceneGUI(){
		Tools.pivotMode=PivotMode.Pivot;
		EventType et=Event.current.type; //Need to save this because it can be changed to Used by other functions
		//Create an object plane
		objectPlane=new Plane(
			script.transform.TransformPoint(new Vector3(0,0,0)),
			script.transform.TransformPoint(new Vector3(0,1,0)),
			script.transform.TransformPoint(new Vector3(1,0,0))
		);

		if(Tools.current==Tool.None && pivotMoveMode && script.isActiveAndEnabled){
			Handles.color=Color.red;
			Handles.DrawSolidDisc(pivotStart,objectPlane.normal,HandleUtility.GetHandleSize(pivotStart)*0.03f);
			Handles.color=Color.white;
			EditorGUI.BeginChangeCheck();
			pivotStart=Handles.FreeMoveHandle(pivotStart,Quaternion.identity,HandleUtility.GetHandleSize(pivotStart)*0.1f,Vector3.zero,Handles.CircleHandleCap);
			bool changed=EditorGUI.EndChangeCheck();
			if(changed && draggingPivot==false){ 
				draggingPivot=true;
				
			}
			//React to drag stop
			if(et==EventType.MouseUp && draggingPivot){
				draggingPivot=false;
				//Transform the point
				Vector2 newPivot=script.transform.InverseTransformPoint(pivotStart);
				//Difference between projected and real pivots converted to local scale
				Vector2 diff=newPivot-(Vector2)script.transform.InverseTransformPoint((Vector2)script.transform.position);
				//To record full state we need to use RegisterFullObjectHierarchyUndo
				Undo.RegisterFullObjectHierarchyUndo(script,"Moving pivot");
				//Set new pivot
				script.SetOffset(newPivot);
				//For undo
				EditorUtility.SetDirty(script);
			}
		}
		SceneView.RepaintAll();
	}

	//Get the sorting layer IDs
	public int[] GetSortingLayerUniqueIDs() {
		Type internalEditorUtilityType=typeof(InternalEditorUtility);
		PropertyInfo sortingLayerUniqueIDsProperty=internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs",BindingFlags.Static|BindingFlags.NonPublic);
		return (int[])sortingLayerUniqueIDsProperty.GetValue(null,new object[0]);
	}

	//Get the sorting layer names
	public string[] GetSortingLayerNames(){
		Type internalEditorUtilityType=typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty=internalEditorUtilityType.GetProperty("sortingLayerNames",BindingFlags.Static|BindingFlags.NonPublic);
		return (string[])sortingLayersProperty.GetValue(null,new object[0]);
	}

	//Convert any enum to array of GUIContent
	GUIContent[] EnumToGUI<K>(){ 
		if(typeof(K).BaseType!=typeof(Enum)) throw new InvalidCastException();
		string[] strings=Enum.GetNames(typeof(K));
		GUIContent[] buttons=new GUIContent[strings.Length];
		for(int i=0;i<buttons.Length;i++){
			buttons[i]=new GUIContent(strings[i]);
		}
		return buttons;
	}

	//Convert any enum to array of GUIContent with images and tooltips, given a path to images
	GUIContent[] EnumToGUI<K>(string resourcePrefix){
		if(typeof(K).BaseType!=typeof(Enum)) throw new InvalidCastException();
		string[] strings=Enum.GetNames(typeof(K));
		GUIContent[] buttons=new GUIContent[strings.Length];
		for(int i=0;i<buttons.Length;i++){
			buttons[i]=new GUIContent((Texture)Resources.Load(resourcePrefix+strings[i]),strings[i]);
		}
		return buttons;
	}

}

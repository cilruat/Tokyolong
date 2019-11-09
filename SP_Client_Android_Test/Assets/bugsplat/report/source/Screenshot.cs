using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;

namespace BugSplat {

    /// <summary>
    /// Class that handles the capture and loading of screenshots.
    /// </summary>
    public class Screenshot : MonoBehaviour {

        #region static
    
        /// <summary>
        /// Captures a screenshot and calls a callback on the file is ready.
        /// </summary>
        /// <param name="p_callback"></param>
        /// <param name="p_scale"></param>
        static public void Capture(Action<byte[]> p_callback,int p_scale=1) {
            Init();
            m_queue.Add(p_callback);
            ScreenCapture.CaptureScreenshot("screenshot-component.png",p_scale);            
        }

        /// <summary>
        /// List of requests to handle.
        /// </summary>
        static List<Action<byte[]>> m_queue { get { return __queue==null ? (__queue = new List<Action<byte[]>>()) : __queue; } }         
        static List<Action<byte[]>> __queue;

        /// <summary>
        /// Reference to the manager component.
        /// </summary>
        static Screenshot m_manager;

        /// <summary>
        /// Creates the polling component.
        /// </summary>
        static private void Init() {
            if(m_manager) return;
            m_manager = GameObject.FindObjectOfType<Screenshot>();
            if(m_manager) return;
            GameObject g = new GameObject("$screenshot-manager");
            g.hideFlags  = HideFlags.HideInHierarchy;
            m_manager = g.AddComponent<Screenshot>();
        }

        #endregion
        
        /// <summary>
        /// Check for the existence of screenshots.
        /// </summary>
        private void Poll() {
            string path = (!Application.isMobilePlatform) ? Application.dataPath : Application.persistentDataPath;
            path = path.Replace('\\','/');
            if(Application.isEditor) path = path.Substring(0,path.LastIndexOf('/'));
             if (Application.platform == RuntimePlatform.OSXPlayer) path = Application.dataPath + "/Resources/Data/";
            string screenshot_path = Path.Combine (path,"screenshot-component.png");            
            screenshot_path = screenshot_path.Replace('\\','/');
            if(File.Exists(screenshot_path)) {
                Debug.Log("Screenshot> screenshot["+screenshot_path+"]");
                byte[] d = null; 
                #if !UNITY_WEBPLAYER
                d = File.ReadAllBytes(screenshot_path);
                #else
                Debug.LogWarning("Screenshot> This feature isn't available on WebPlayer builds.");
                #endif
                for(int i=0;i<m_queue.Count;i++) m_queue[i](d);
                File.Delete(screenshot_path);
                m_queue.Clear();
            }
        }

        /// <summary>
        /// Updates the screenshot polling.
        /// </summary>
        private void Update () {
            Poll();	
	    }
    }

}
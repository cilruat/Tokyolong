#if (UNITY_4 || UNITY_5 || UNITY_6 || UNITY_2017 || UNITY_2018)
#define IS_UNITY
#endif

#if IS_UNITY

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BugSplat
{

    #region struct UnityLog

    /// <summary>
    /// Class that describes a Unity's log output.
    /// </summary>
    [System.Serializable]
    public struct UnityLog  {
   
        /// <summary>
        /// Log message.
        /// </summary>
        public string condition;

        /// <summary>
        /// Log stacktrace
        /// </summary>
        public string stacktrace;

        /// <summary>
        /// Log type (message, warning, error,...)
        /// </summary>
        public LogType type;     
       
        /// <summary>
        /// Returns a flag indicating if the log 'v' is equal to this log.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="p_ignore_type"></param>
        /// <returns></returns>
        public bool Equal(UnityLog v,bool p_ignore_type) {                
            if(condition != v.condition)   return false;
            if(stacktrace != v.stacktrace) return false;
            if(!p_ignore_type) if(type != v.type) return false;
            return true;
        }

        /// <summary>
        /// Returns a flag indicating if the log 'v' is equal to this log.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool Equal(UnityLog v) { return Equal(v,false); }
    
    }
    
    #endregion

    /// <summary>
    /// Class that extends the BaseReporter to handle Unity related features and callbacks.
    /// It will offer the basic reporting functionalities not dependents on Unity features.
    /// </summary>
	[System.Serializable]
	public class Reporter : BaseReporter {

        /// <summary>
        /// Flag that indicates that exceptions will capture a screenshot.
        /// </summary>
        [Tooltip("Flag that tells this reporter will capture screenshots.")]
        public bool generateScreenshot = true;

        /// <summary>
        /// Flag that indicates that exceptions will send the complete log until it.
        /// </summary>
        [Tooltip("Flag that tells this reporter will generate a log file.")]
        public bool generateLogFile = true;

        /// <summary>
        /// Flag that tells the reporter to issue a Exception Dialog when an error occurs.
        /// </summary>
        [Tooltip("Flag that tells this reporter will open a prompt window.")]
        public bool prompt;

        /// <summary>
        /// Flag that indicate if Unity's log entries will be stored.
        /// </summary>
        [Tooltip("Flag that allow storing Unity logs for later uploading.")]
        public bool logStore;

        /// <summary>
        /// List of captured logs.
        /// </summary>
        [Tooltip("List of generated Unity logs.")]
        public List<UnityLog> logs;

        /// <summary>
        /// Max limit of logs
        /// </summary>
        [Tooltip("Max number of logs to be stored.")]
        public int logMax = 100;

        /// <summary>
        /// Returns the concatenated log strings.
        /// </summary>
        public string logString {
            get {
                if(m_logString==null) m_logString = new StringBuilder();                
                for(int i=0;i<logs.Count;i++) {
                    string s = "["+logs[i].type.ToString().ToLower()+"] "+logs[i].condition+"\n"+logs[i].stacktrace+"\n";
                    m_logString.Append(s);
                }
                string res = m_logString.ToString();
                //Reset builder
                m_logString.Length=0;
                return res;
            }
        }

        /// <summary>
        /// Ignored reports count (for debugging).
        /// </summary>
        [Tooltip("Reports ignored for being repeated.")]
        public int ignored;

        /// <summary>
        /// Sent reports count (for debugging).
        /// </summary>
        [Tooltip("Sent reports.")]
        public int count;

        /// <summary>
        /// Last generated log.
        /// </summary>
        private UnityLog m_lastLog;

        /// <summary>
        /// Handles the concatenation of log strings.
        /// </summary>
        private StringBuilder m_logString;

        /// <summary>
        /// Initialize the Reporter to 
        /// </summary>
        public void Initialize(GameObject gameObject) {

            logs        = new List<UnityLog>();
            m_logString = new StringBuilder();
            m_lastLog   = new UnityLog();
            m_gameObject = gameObject;

            if(string.IsNullOrEmpty(database))
            {
                Debug.LogError("BugSplat> Error! Please specify a database in the 'Reporter' section of your script's properties to enable crash reporting!");
                return;
            }

            if (string.IsNullOrEmpty(app))
            {
                Debug.LogWarning("BugSplat> Warning! Application name not found. Please specify a name in the 'Reporter' section of your script's properties.");
            }

            if (string.IsNullOrEmpty(version))
            {
                Debug.LogWarning("BugSplat> Warning! Application version not found. Please specify a version in the 'Reporter' section of your script's properties.");
            }

            Log("BugSplat> Reporter Initialize database["+database+"] app["+app+"] version["+version+"]");
            
            //Register the reporter into Unity's logging callback.
            Application.logMessageReceived += OnUnityLog;
        }
        
        /// <summary>
        /// Sends a report.
        /// </summary>
        /// <param name="p_report"></param>
        /// <param name="p_prompt"></param>
        public void Send(Report p_report,bool p_prompt=false) {
            Report r = p_report;
            if(r==null) return;
            GenerateLogFile(r);
            GenerateScreenshot(r,delegate(Report p_result) {
                if(p_prompt) {
                    CallModal(p_result);
                }
                else {
                    base.Send(p_result);
                }                
            });            
        }

        /// <summary>
        /// Handler for unity related logs.
        /// </summary>
        /// <param name="p_condition"></param>
        /// <param name="p_stacktrace"></param>
        /// <param name="p_type"></param>
        protected void OnUnityLog(string p_condition,string p_stacktrace,LogType p_type) {
        
            UnityLog log = new UnityLog();
            log.condition  = p_condition;
            log.stacktrace = p_stacktrace;
            log.type       = p_type;
            
            //If log storing enabled
            if(logStore) {
                logs.Add(log);            
                //If log entries exceeds 'max' start removing.
                if(logs.Count>logMax) logs.RemoveAt(0);
            }
            
            //If the same as last one, ignore.
            if(log.Equal(m_lastLog)) { 
                ignored++;                 
                return; 
            }
            
            //If not error ignore
			if (log.type != LogType.Exception &&
			   log.type != LogType.Error &&
			   log.type != LogType.Assert)
				return;

            //Disable detection to avoid recursion loops
            Application.logMessageReceived -= OnUnityLog;
            
            count++;

            m_lastLog.stacktrace = p_stacktrace;
            m_lastLog.condition  = p_condition;
            m_lastLog.type       = p_type;
            
            Report r = new Report();
            
            //if(Debug.isDebugBuild)
            //{
                r.stacktrace = p_condition+"\n"+p_stacktrace;
            //}
            //else
            //{
            //    r.stacktrace = p_condition+"\n"+new System.Diagnostics.StackTrace(true).ToString();
            //}
            
            GenerateLogFile(r);
                        
            GenerateScreenshot(r,delegate(Report p_result) {

                if(prompt) CallModal(p_result); else base.Send(p_result);

                //Enable detection again
                Application.logMessageReceived += OnUnityLog;
            });
            
        }

        /// <summary>
        /// Calls a feedback modal and issue a report.
        /// </summary>
        /// <param name="p_report"></param>
        private void CallModal(Report p_report) {
            Report r = p_report;
            ReportModal m = Resources.Load<ReportModal>("prefabs/bugsplat-report-modal");
            if(!m) { Debug.LogWarning("Report> Modal not found!"); return;  }
            m = UnityEngine.Object.Instantiate<ReportModal>(m);
            m.name = "bugsplat-report-modal";
            m.Open(delegate (ReportModal p_modal) {
                    
                if(!p_modal) { Debug.LogWarning("Report> Modal is null!"); return; }
                r.userEmail   = p_modal.userEmail;
                r.description = p_modal.description;
                base.Send(r);
            });
        }

        /// <summary>
        /// Creates the log file and set it on the report.
        /// </summary>
        private void GenerateLogFile(Report p_report) {
            Report r = p_report;
            if(!generateLogFile) return;
            Report.FileData fd = new Report.FileData();
            fd.name = "UnityCallstack.txt";
            fd.data = System.Text.Encoding.UTF8.GetBytes (logString); 
            r.files.Add(fd);
        }

        /// <summary>
        /// Creates the screenshot and calls a callback when it is done.
        /// </summary>
        private void GenerateScreenshot(Report p_report,Action<Report> p_on_complete) {
            Report r = p_report;
            if(!generateScreenshot) { if(p_on_complete!=null) p_on_complete(r); return; }            
            //Captures the screenshot and then send the report.
            Screenshot.Capture(delegate(byte[] p_file) {
                Report.FileData fd = new Report.FileData();
                fd.name = "UnityScreenshot.png";
                fd.data = p_file;
                r.files.Add(fd);
                if(p_on_complete!=null) p_on_complete(r);
            });
        }

    }

  }

#endif
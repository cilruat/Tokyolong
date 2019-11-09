using System;
using System.Collections.Generic;
using UnityEngine;

namespace BugSplat
{
    /// <summary>
    /// Class that implements a BugSplat reporter instance.
    /// It will offer the basic reporting functionalities not dependents on Unity features.
    /// </summary>
    [System.Serializable]
    public class BaseReporter
    {
        /// <summary>
        /// Database from the user account.
        /// </summary>
        public string database = "Fred";

        /// <summary>
        /// Application Name.
        /// </summary>
        public string app = "MyUnityCrasher";

        /// <summary>
        /// App Key.
        /// </summary>
        public string key = "1.0";

        /// <summary>
        /// App Version.
        /// </summary>
        public string version = "1.0";

        /// <summary>
        /// Flag that tells the instance to not generate log information itself.
        /// </summary>
        public bool quiet = false;

        /// <summary>
        /// Previously sent report.
        /// </summary>
        public Report previous;

        /// <summary>
        /// Public setter for callback that is executed after report is done uploading
        /// </summary>
        protected Action<bool, string> m_callback = (success, message) => { };
        public void SetCallback(Action<bool, string> callback)
        {
            this.m_callback = callback;
        }

        /// <summary>
        /// GameObject so BugSplat can use the Unity API for coroutines
        /// </summary>
        protected GameObject m_gameObject;

        /// <summary>
        /// Buffer of reports to be sent.
        /// </summary>
        private List<Report> m_buffer { get { return __buffer == null ? (__buffer = new List<Report>()) : __buffer; } }
        private List<Report> __buffer;

        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="p_message"></param>
        public void Log(object p_message)
        {
            if (quiet) return;
            #if UNITY_WINRT_10_0
            Debug.Log(p_message);
            #else
            Console.WriteLine(p_message);
            #endif
        }

        /// <summary>
        /// Queues a report to be sent in the next update.
        /// </summary>
        /// <param name="p_report"></param>
        public virtual void Send(Report p_report)
        {
            //If some of the report's data are empty, fill them with the Reporter's data
            if (string.IsNullOrEmpty(p_report.app)) p_report.app = app;
            if (string.IsNullOrEmpty(p_report.database)) p_report.database = database;
            if (string.IsNullOrEmpty(p_report.key)) p_report.key = key;
            if (string.IsNullOrEmpty(p_report.version)) p_report.version = version;

            m_buffer.Add(p_report);
            if (m_buffer.Count == 1) PollUpload();
        }

        /// <summary>
        /// Sends a new Report using the exception data.
        /// </summary>
        /// <param name="p_error"></param>
		public virtual void Send(Exception p_error)
        {
            Report r = Report.FromException(p_error);
            Send(r);
        }

        /// <summary>
        /// Actually sends the Report.
        /// </summary>
        /// <param name="p_report"></param>
        private void Upload(Report p_report)
        {
            var crashReport = p_report;
            previous = crashReport;

            try
            {
                if (m_gameObject == null)
                {
                    throw new Exception("Please inject the Game Object from your script when calling Reporter.Initialize to enable crash reporting.");
                }
                
                var bugSplatApiClient = (BugSplatApiClient)m_gameObject.GetComponent(typeof(BugSplatApiClient));
                if (bugSplatApiClient == null)
                {
                    m_gameObject.AddComponent(typeof(BugSplatApiClient));
                    bugSplatApiClient = (BugSplatApiClient)m_gameObject.GetComponent(typeof(BugSplatApiClient));
                }

                bugSplatApiClient.Post(crashReport, Log, m_callback);
            }
            catch(Exception ex)
            {
                Log(ex);
            }

            PollUpload();
        }

        /// <summary>
        /// Check the buffer for new uploads.
        /// </summary>
        private void PollUpload()
        {
            if (m_buffer.Count <= 0) return;
            Report r = m_buffer[0];
            m_buffer.RemoveAt(0);
            Upload(r);
        }
    }

}

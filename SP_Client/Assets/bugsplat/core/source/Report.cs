using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace BugSplat
{
    #region class Report

    /// <summary>
    /// Class that describes a report structure with all data possible to send.
    /// </summary>
    [System.Serializable]
    public class Report
    {

        #region class FileData

        /// <summary>
        /// Class that implements the payload that will be uploaded to BugSplat servers.
        /// </summary>
        [System.Serializable]
        public class FileData
        {

            /// <summary>
            /// File name.
            /// </summary>
            public string name;

            /// <summary>
            /// File data.
            /// </summary>
            public byte[] data;

            /// <summary>
            /// Encodes the file data to Base64.
            /// </summary>
            /// <returns></returns>
            public string Encode()
            {
                return Convert.ToBase64String(data == null ? (new byte[0]) : data);
            }

        }

        #endregion

        #region static 

        /// <summary>
        /// Creates a new Report instance using an Exception.
        /// </summary>
        /// <param name="p_error"></param>
        /// <returns></returns>
        static public Report FromException(Exception p_error)
        {
            Exception err = (Exception)p_error;
            string stacktrace = err.StackTrace;
            if (string.IsNullOrEmpty(stacktrace)) stacktrace = Environment.StackTrace;
            string msg = err.Message + "\n" + stacktrace;
            Report r = new Report();
            r.stacktrace = msg;
            return r;
        }

        #endregion

        /// <summary>
        /// Database name.
        /// </summary>
        public string database;

        /// <summary>
        /// App Name
        /// </summary>
        public string app;

        /// <summary>
        /// App Version
        /// </summary>
        public string version;

        /// <summary>
        /// Exception Stacktrace.
        /// </summary>
        public string stacktrace;

        /// <summary>
        /// Key
        /// </summary>
        public string key;

        /// <summary>
        /// List of files to be uploaded.
        /// </summary>
        public List<FileData> files;

        /// <summary>
        /// User Name
        /// </summary>
        public string userName;

        /// <summary>
        /// User Email
        /// </summary>
        public string userEmail;

        /// <summary>
        /// Exception Description.
        /// </summary>
        public string description;

        /// <summary>
        /// CTOR.
        /// </summary>
        public Report()
        {
            database = "";
            app = "";
            version = "";
            key = "";
            files = new List<FileData>();
            userName = "";
            userEmail = "";
            description = "";
        }

        /// <summary>
        /// Returns a table with key-value pairs of the report data.
        /// </summary>
        /// <returns>Form data to be sent by POST.</returns>

        /// <summary>
        /// Returns a table with key-value pairs of the report data.
        /// </summary>
        /// <returns>Form data to be sent by POST.</returns>
        public List<IMultipartFormSection> GetForm()
        {
            List<IMultipartFormSection> form = new List<IMultipartFormSection>();

            form.Add(new MultipartFormDataSection("database", database));
            form.Add(new MultipartFormDataSection("appName", app));
            form.Add(new MultipartFormDataSection("appVersion", version));
            form.Add(new MultipartFormDataSection("appKey", key));
            form.Add(new MultipartFormDataSection("callstack", stacktrace));
            AddToFormIfNotNullOrEmpty(form, "user", userEmail);
            AddToFormIfNotNullOrEmpty(form, "usercomments", description);

            var fileCounter = 1;
            foreach (var file in files)
            {
                if (file.data == null || file.data.Length <= 0) continue;
                form.Add(new MultipartFormDataSection("fileName" + fileCounter, file.name));
                form.Add(new MultipartFormDataSection("optFile" + fileCounter, file.Encode()));
                fileCounter++;
            }

            return form;
        }

        private void AddToFormIfNotNullOrEmpty(List<IMultipartFormSection> form, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                form.Add(new MultipartFormDataSection(key, value));
            }
        }
    }

    #endregion

}

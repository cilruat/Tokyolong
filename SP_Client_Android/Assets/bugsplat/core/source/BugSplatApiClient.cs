using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace BugSplat
{
    /// <summary>
    /// Make asynchronous web requests to BugSplat
    /// </summary>
    public class BugSplatApiClient : MonoBehaviour
    {
        /// <summary>
        /// Send report to BugSplat asynchronously
        /// </summary>
        /// <param name="crashReport">Crash report to post</param>
		public void Post(Report crashReport, Action<string> log, Action<bool, string> callback)
        {
            var app = crashReport.app;
            var version = crashReport.version;
            var stackTrace = crashReport.stacktrace;
            var key = crashReport.key;
            var url = GetPostUrl(crashReport.database);
            var formParams = crashReport.GetForm();

            log("BugSplatApiClient> Post\nurl[" + url + "]\nappName[" + app + "]\nappVersion[" + version + "] \n--callstack--\n" + stackTrace + "\nappKey[" + key + "]\nurl[" + url + "]");
			StartCoroutine(CreateUnityPostRequest(url, formParams, log, callback));
        }

		private IEnumerator CreateUnityPostRequest(string url, List<IMultipartFormSection> formParams, Action<string> log, Action<bool, string> callback)
        {
            using (var webRequest = UnityWebRequest.Post(url, formParams))
            {
				yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
					var error = webRequest.error;
                    log("BugSplatApiClient> Error posting crash report to " + url + "!");
                    log(error);
					callback(false, error);
                }
                else
                {
					var response = webRequest.downloadHandler.text;
                    log("BugSplatApiClient> Upload complete!");
					log(response);
					callback(true, response);
                }
            }
        }

        private string GetPostUrl(string database)
        {
            return "https://" + database.ToLowerInvariant() + ".bugsplat.com/post/unity/";
        }
    }
}

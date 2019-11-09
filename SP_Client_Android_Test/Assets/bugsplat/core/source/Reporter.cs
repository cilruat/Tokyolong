#if (UNITY_4 || UNITY_5 || UNITY_6 || UNITY_2017 || UNITY_2018)
#define IS_UNITY
#endif

#if !IS_UNITY

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace BugSplat {

    /// <summary>
    /// Class that extends the BaseReporter to handle non Unity scenarios.
    /// </summary>
    [System.Serializable]
    public class Reporter : BaseReporter {
        
        /// <summary>
        /// CTOR.
        /// </summary>
        public Reporter() {
            AppDomain.CurrentDomain.UnhandledException += OnException;
        }        
        
        /// <summary>
        /// Handles exceptions globally.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnException(object sender,UnhandledExceptionEventArgs e) {
            Exception err = (Exception) e.ExceptionObject;                       
            Send(err);
        }

    }

  }

  #endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackGameTemplate
{
    /// <summary>
    /// Sets a time of day, with a light, a skybox material, and a fog color
    /// </summary>
    public class STKTimesOfDay : MonoBehaviour
    {
        [Tooltip("A list of all the times of day we can set")]
        public TimeOfDay[] timesOfDay;

        // The index of the current time of day
        static int timeOfDayIndex = 0;

        [Tooltip("Change to the next time of day when the scene starts")]
        public bool changeOnStart = false;

        public void Start()
        {
            // Change to the next time of day when the scene starts
            if (changeOnStart == true) NextTimeOfDay();
        }

        [System.Serializable]
        public class TimeOfDay
        {
            public Light light;

            public Color fogColor;
        }

        /// <summary>
        /// Changes to the next time of day in the list
        /// </summary>
        public void NextTimeOfDay()
        {
            // Go through all times of day, and turn them off
            foreach (TimeOfDay timeOfDay in timesOfDay)
            {
                timeOfDay.light.enabled = false;
            }

            // Cycle through the times of day index, and start from 0 if we reach the end of the list
            if (timeOfDayIndex < timesOfDay.Length - 1) timeOfDayIndex++;
            else timeOfDayIndex = 0;

            // Enable the light object associated with this time of day
            timesOfDay[timeOfDayIndex].light.enabled = true;
            
            // Set the fog color
            RenderSettings.fogColor = timesOfDay[timeOfDayIndex].fogColor;

            // Set the camera background color to be the same as the fog
            Camera.main.backgroundColor = timesOfDay[timeOfDayIndex].fogColor;
        }
    }
}

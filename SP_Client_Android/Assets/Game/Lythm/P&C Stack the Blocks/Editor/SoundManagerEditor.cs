using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace PnCCasualGameKit
{
    [CustomEditor(typeof(PnCSoundManger), true)]
    public class SoundManagerEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            PnCSoundManger soundManager = (PnCSoundManger)target;

            EditorGUILayout.LabelField("");
            EditorGUILayout.HelpBox("After assigning Audio clips, Hit this button to get an enum of audioclip names created/updated. Makes method calls easy.\n ** NOTE : Make sure the name is a valid enum name **\n" +
                                    " Example - SoundManager.Instance.playSound(AudioClips.perfect);", MessageType.Info);

            if (GUILayout.Button("Generate Audioclip Names Enum"))
            {
                GenerateAudioClipsEnum();
            }
        }

        /// <summary>
        /// Creates the audioClips enum at the soundManager's location
        /// </summary>
        public void GenerateAudioClipsEnum()
        {
            //Get the list of audios
            PnCSoundManger soundManager = (PnCSoundManger)target;
            List<Audio> audios = soundManager.audios;

            //Get the script's path
            MonoScript thisScript = MonoScript.FromMonoBehaviour(soundManager);
            string ScriptFilePath = AssetDatabase.GetAssetPath(thisScript);

            //Create a path for the enum file
            string enumName = "AudioClips";
            string filePathAndName = ScriptFilePath.Replace(thisScript.name + ".cs", "") + "/" + enumName + ".cs";

            //Wrire the enum at above path
            using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
            {
                streamWriter.WriteLine("public enum " + enumName);
                streamWriter.WriteLine("{");
                for (int i = 0; i < audios.Count; i++)
                {
                    streamWriter.WriteLine("\t" + audios[i].audioName + ",");
                }
                streamWriter.WriteLine("}");
            }
            AssetDatabase.Refresh();

            Debug.Log("Audioclips  enum created/updated at " + ScriptFilePath);

        }
    }
}

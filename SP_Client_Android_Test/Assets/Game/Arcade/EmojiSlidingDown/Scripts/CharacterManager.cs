using UnityEngine;
using System.Collections;

namespace Emoji
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance;

        public static readonly string CURRENT_CHARACTER_KEY = "SGLIB_CURRENT_CHARACTER";

        public int CurrentCharacterIndex
        {
            get
            {
                return PlayerPrefs.GetInt(CURRENT_CHARACTER_KEY, 0);
            }
            set
            {
                PlayerPrefs.SetInt(CURRENT_CHARACTER_KEY, value);
                PlayerPrefs.Save();
            }
        }

        public Character[] characters;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);

            }
        }
    }
}
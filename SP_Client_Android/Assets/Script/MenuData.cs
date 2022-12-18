using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuData 
{
    public static bool loaded = false;

    public int menuID;
    public int category;
    public string menuName;
    public int price;
	public bool show;

    public static Dictionary<int, List<MenuData>> dictMainMenu = new Dictionary<int, List<MenuData>>();
    public static Dictionary<int, MenuData> dictMenu = new Dictionary<int, MenuData>();

	public MenuData(int menuID, int category, string menuName, int price, bool show)
    {
        this.menuID = menuID;
        this.category = category;
        this.menuName = menuName;
        this.price = price;
		this.show = show;
    }

    public static void Load()
    {
        if (loaded)
            return;

#if UNITY_ANDROID
        string path = Application.streamingAssetsPath + "/Menu.csv";

#else
        string path = Application.dataPath;
        int lastIdx = path.LastIndexOf(@"/");
        path = path.Substring(0, lastIdx) + @"\Info\Menu.csv";
#endif

        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        data = CSVReader.Read(path);

        for (int i = 0; i < data.Count; i++)
        {
            int menuID = -1;
            int category = -1;
            string menuName = string.Empty;
            int price = -1;
            bool show = true;

            Dictionary<string, object> _child = data[i];
            foreach(KeyValuePair<string, object> dic in _child)
            {
                object _value = dic.Value;
                switch (dic.Key)
                {
                    case "Menu":
                        menuID = int.Parse(_value.ToString());
                        break;
                    case "Category":
                        category = int.Parse(_value.ToString());
                        break;
                    case "MenuName":
                        menuName = _value.ToString();
                        break;
                    case "Price":
                        price = int.Parse(_value.ToString());
                        break;
                    case "Show":
                        if (string.IsNullOrEmpty(_value.ToString()) == false)
                            show = bool.Parse(_value.ToString());
                        break;
                }
            }

			MenuData menuData = new MenuData(menuID, category, menuName, price, show);

            Debug.Log("menuID: " + menuID);

            dictMenu.Add(menuID, menuData);

            if (dictMainMenu.ContainsKey(category) == false)
                dictMainMenu.Add(category, new List<MenuData>());

            dictMainMenu[category].Add(menuData);
        }

        loaded = true;
    }

    public static MenuData Get(int i)
    {
        MenuData data = null;
        if (dictMenu.TryGetValue(i, out data) == false)
            return null;

        return data;
    }
}

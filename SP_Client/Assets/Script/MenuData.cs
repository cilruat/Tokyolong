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

        string path = Application.dataPath;
        int lastIdx = path.LastIndexOf(@"/");
        path = path.Substring(0, lastIdx) + @"\Info\Menu.csv";

        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        data = CSVReader.Read(path);

        for (int i = 0; i < data.Count; i++)
        {
            int menuID = int.Parse(data[i]["Menu"].ToString());
            int category = int.Parse(data[i]["Category"].ToString());
            string menuName = data[i]["MenuName"].ToString();
            int price = int.Parse(data[i]["Price"].ToString());

			bool show = true;
			if (string.IsNullOrEmpty (data [i] ["Show"].ToString ()) == false)
				show = bool.Parse (data [i] ["Show"].ToString ());

			MenuData menuData = new MenuData(menuID, category, menuName, price, show);

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

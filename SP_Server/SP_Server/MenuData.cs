using System.Collections;
using System.Collections.Generic;

public class MenuData
{
    public static bool loaded = false;

    public int menuID;
    public int category;
    public string menuName;
    public int price;

    public static Dictionary<int, List<MenuData>> dictMainMenu = new Dictionary<int, List<MenuData>>();
    public static Dictionary<int, MenuData> dictMenu = new Dictionary<int, MenuData>();
    public static List<int> listMenuPrice = new List<int>();

    public MenuData(int menuID, int category, string menuName, int price)
    {
        this.menuID = menuID;
        this.category = category;
        this.menuName = menuName;
        this.price = price;
    }

    public static void Load()
    {
        if (loaded)
            return;

        string path = "Data\\Menu.csv";

        dictMainMenu.Clear();
        dictMenu.Clear();
        listMenuPrice.Clear();
        listMenuPrice.Add(0);

        List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        data = CSVReader.Read(path);

        for (int i = 0; i < data.Count; i++)
        {
            int menuID = int.Parse(data[i]["Menu"].ToString());
            int category = int.Parse(data[i]["Category"].ToString());
            string menuName = data[i]["MenuName"].ToString();
            int price = int.Parse(data[i]["Price"].ToString());

            MenuData menuData = new MenuData(menuID, category, menuName, price);

            dictMenu.Add(menuID, menuData);

            if (dictMainMenu.ContainsKey(category) == false)
                dictMainMenu.Add(category, new List<MenuData>());

            dictMainMenu[category].Add(menuData);
            listMenuPrice.Add(price);
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

    public static int GetMenuPrice(int i)
    {
        if (listMenuPrice.Count < i)
            return 0;

        return listMenuPrice[i];
    }
}

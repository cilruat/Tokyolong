using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System.Text;
public class ScriptingSymbolsHandler {

    /// <summary>
    /// Determine whether the given namespace is already exist in the project
    /// </summary>
    /// <param name="nameSpace"></param>
    /// <param name="assemblyName"></param>
    /// <returns></returns>
    public static bool NamespaceExists(string nameSpace, string assemblyName = null)
    {
        Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

        foreach (Assembly asm in assemblies)
        {
            // The assembly must match the given one if any.
            if (!string.IsNullOrEmpty(assemblyName) && !asm.GetName().Name.Equals(assemblyName))
            {
                continue;
            }

            System.Type[] types = asm.GetTypes();
            foreach (System.Type t in types)
            {
                // The namespace must match the given one if any. Note that the type may not have a namespace at all.
                // Must be a class and of course class name must match the given one.
                if (!string.IsNullOrEmpty(t.Namespace) && t.Namespace.Equals(nameSpace))
                {
                    return true;
                }
            }
        }

        return false;
    }


    /// <summary>
    /// Add the given scripting symboyl array to the current build platform
    /// </summary>
    /// <param name="symbols"></param>
    /// <param name="platform"></param>
    public static void AddDefined_ScriptingSymbol(string[] symbols, BuildTargetGroup platform)
    {
        string symbolStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(platform);
        List<string> currentSymbols = new List<string>(symbolStr.Split(';'));
        int added = 0;

        foreach (string symbol in symbols)
        {
            if (!currentSymbols.Contains(symbol))
            {
                currentSymbols.Add(symbol);
                added++;
            }
        }

        if (added > 0)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < currentSymbols.Count; i++)
            {
                sb.Append(currentSymbols[i]);
                if (i < currentSymbols.Count - 1)
                    sb.Append(";");
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(platform, sb.ToString());
        }
    }

}

using System.Reflection;
using UnityEngine;

public class VEMAssemblyManager : Singleton<VEMAssemblyManager>
{
    public Assembly CSharp_Assembly;

    public Assembly VEM_CarryOut_Assembly;

    private bool isEditor;

    public void Init(bool isEditor)
    {
        this.isEditor = isEditor;
        LoadAssembly("Assembly-CSharp.dll", out CSharp_Assembly);
        LoadVEMSimAssembly("VEM_CarryOut.dll", out VEM_CarryOut_Assembly);
    }

    private void LoadAssembly(string assemblyName, out Assembly assembly)
    {
        if (isEditor)
        {
            assembly = Assembly.LoadFile(Application.dataPath.Replace("Assets", "") + "Library\\ScriptAssemblies\\" + assemblyName);
            if (assembly == null)
            {
                Debug.Log("Load Assembly Error! AssemblyName == " + assemblyName);
            }
        }
        else
        {
            assembly = Assembly.LoadFile(Application.dataPath + "\\Managed\\" + assemblyName);
            if (assembly == null)
            {
                Debug.Log("Load Assembly Error! AssemblyName == " + assemblyName);
            }
        }
    }

    private void LoadVEMSimAssembly(string assemblyName, out Assembly assembly)
    {
        if (isEditor)
        {
            assembly = Assembly.LoadFile(Application.dataPath + "\\Plugins\\VEMSimCoreDLL\\" + assemblyName);
            if (assembly == null)
            {
                Debug.Log("Load Assembly Error! AssemblyName == " + assemblyName);
            }
        }
        else
        {
            assembly = Assembly.LoadFile(Application.dataPath + "\\Managed\\" + assemblyName);
            if (assembly == null)
            {
                Debug.Log("Load Assembly Error! AssemblyName == " + assemblyName);
            }
        }
    }
}
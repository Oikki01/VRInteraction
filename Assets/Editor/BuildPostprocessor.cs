using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace VEMSim
{
    public class BuildPostprocessor
    {
        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
            {
                //复制解密动态库
                string filePath = Application.dataPath + @"\Plugins\VEMSimDecode\mono-2.0-bdwgc.dll";
                FileInfo fileInfo = new FileInfo(pathToBuiltProject);
                string folderPath = String.Format(@"{0}\{1}\{2}", fileInfo.Directory.FullName, "MonoBleedingEdge", @"EmbedRuntime\mono-2.0-bdwgc.dll");
                FileUtil.ReplaceFile(filePath, folderPath);

                // 复制battleSim 目录
                string battleSimPath = Application.dataPath.Replace("Assets", "") + @"BattleSim";
                string exeBattleSimPath = String.Format(@"{0}\{1}", fileInfo.Directory.FullName, "BattleSim");
                FileUtil.ReplaceDirectory(battleSimPath, exeBattleSimPath);
            }
        }
    }
}
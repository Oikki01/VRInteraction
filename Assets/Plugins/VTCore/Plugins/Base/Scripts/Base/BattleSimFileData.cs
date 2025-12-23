/****************************************************
*文件名：.cs
*文件功能描述： 
*创建者：曹志海
*创建时间：2021/0608/
*参考文档：
*修改记录： 用来读取平台工具读取数据
****************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using HuaRuXR.Resource;
using HuaRuXR.Base;
using System.Xml;
using HuaRuXR.Serialize;
using System;
using HuaRuXR.DataTable;

public class BattleSimFileData : MonoBehaviour
{
    public static BattleSimFileData Instance;
    private void Awake()
    {
        Instance = this;
    }
    /// <summary>
    /// 序列化资源器
    /// </summary>
    private PackageVersionListSerializer packageVersionListSerializer;
    /// <summary>
    ///获取到的场景资源名称
    /// </summary>
    private  List<string> m_BSimCoreVersiones = new List<string>();
    /// <summary>
    /// 获取地形资源数据
    /// </summary>SX
    public  List<string> GetBSimCoreVersiones
    {
        get { return m_BSimCoreVersiones; }
    }
    /// <summary>
    /// 获取到的GIS资源名称
    /// </summary>
    private  List<string> m_BSimCoreGISVersiones = new List<string>();
    /// <summary>
    /// 获取GIS地形资源数据
    /// </summary>
    public  List<string> BSimCoreGISVersiones
    {
        get { return m_BSimCoreGISVersiones; }
    }


    void Start()
    {
        AssetName();
        string[] pathList = ResourceEntry.Resources.SimPathResource.PathListData.OtherList;
        if (pathList != null)
        {
            for (int i = 0; i < pathList.Length; i++)
            {
                GetAssetPathORAssetName(BSimCoreEntry.BattleSimPath + pathList[i]);
            }
        }
        GetGISPathName(BSimCoreEntry.BattleSimPath + @"/AssetsBundle/Scene/GIS_Scene");
        InitExcleListData();
        ReadAllTable();
    }
    #region 加载Excel表
    private List<ExcelTableInfo> m_TableDict;
    void InitExcleListData()
    {
        string m_DefaultPath = BSimCoreEntry.BattleSimPath + "/DataTable/ExcelTable/";
        ReadTableFile(m_DefaultPath + "TableConfig.config");
    }
    /// <summary>
    /// 保存读表的配置文件
    /// </summary>
    /// <param name="fullName"></param>
    public void ReadTableFile(string fullName)
    {
        if (FileHelper.Exists(fullName))
        {
            m_TableDict = new List<ExcelTableInfo>();
            XmlDocument xmlFile = HuaRuXR.Utilities.XMLHelper.ReaderXmlDocument(fullName);
            XmlNode rootNode = xmlFile.SelectSingleNode("RootNode");

            XmlNode tempNode = rootNode.SelectSingleNode("表格");
            if (tempNode != null)
            {
                for (int i = 0; i < tempNode.ChildNodes.Count; i++)
                {
                    XmlNode childNode = tempNode.ChildNodes[i];
                    BSVariant variant = new BSVariant(childNode);
                    ExcelTableInfo info = variant.Value<ExcelTableInfo>();
                    info.Init();
                    m_TableDict.Add(info);
                }

                tempNode = null;
            }
        }
        else
        {
            Log.Error("缺少基础数据表  ：" + fullName);
        }

    }
    /// <summary>
    /// 遍历枚举类型，将要读取的表添加到读取表的字典中
    /// 后续可根据实际情况修改
    /// </summary>
    public void ReadAllTable()
    {
        for (int i = 0; i < m_TableDict.Count; i++)
        {
            if (m_TableDict[i] != null)
            {
                DataTableEntry.DataTable.CreateDataTable(m_TableDict[i].SType, m_TableDict[i].EnumName.ToString(), m_TableDict[i].TablePath);
            }
        }
        CoreEntry.Event.Fire((uint)AllDataLoadSuccessEventArgs.EventId,AllDataLoadSuccessEventArgs.Create(true));
    }
    #endregion

    /// <summary>
    /// 获取场景资源路径及名称
    /// </summary>
    private void GetAssetPathORAssetName(string path)
    {
        if (!Directory.Exists(path))
        {
            Debug.LogError(path + "当前路径为空");
            return;
        }
        string[] directory = Directory.GetDirectories(path);

        string[] files = Directory.GetFiles(path);
        for (int i = 0; i < files.Length; i++)
        {
            FileInfo fi = new FileInfo(files[i]);
            if (fi.Name == "BSimCoreVersion.dat")
            {
                FileStream fileStream = new FileStream(fi.FullName, FileMode.Open);
                PackageVersionList versionList = packageVersionListSerializer.Deserialize(fileStream);
                foreach (var item in versionList.GetAssets())
                {
                    m_BSimCoreVersiones.Add(item.Name);
                }
            }
        }
        if (directory.Length > 0)
        {
            for (int i = 0; i < directory.Length; i++)
            {
                GetAssetPathORAssetName(directory[i]);
            }
        }
    }
    /// <summary>
    /// 获取GIS场景资源路径及名称
    /// </summary>
    /// <param name="path"></param>
    private void GetGISPathName(string path)
    {
        if (!Directory.Exists(path))
        {
           // Debug.LogError(path + "当前路径为空");
            return;
        }
        string[] directory = Directory.GetDirectories(path);
        for (int i = 0; i < directory.Length; i++)
        {
            m_BSimCoreGISVersiones.Add(directory[i]);
        }
    }


    private void AssetName()
    {
        packageVersionListSerializer = new PackageVersionListSerializer();
        packageVersionListSerializer.RegisterDeserializeCallback(0, BuiltinVersionListSerializer.PackageVersionListDeserializeCallback_V0);
        packageVersionListSerializer.RegisterDeserializeCallback(1, BuiltinVersionListSerializer.PackageVersionListDeserializeCallback_V1);
        packageVersionListSerializer.RegisterDeserializeCallback(2, BuiltinVersionListSerializer.PackageVersionListDeserializeCallback_V2);


    }

    
}

namespace HuaRuXR.DataTable
{
    /// <summary>
    /// 表格对应的枚举和数据类型
    /// </summary>
    public class ExcelTableInfo
    {
        /// <summary>
        /// 枚举类型
        /// </summary>
        private string m_EnumName;
        /// <summary>
        /// 枚举类型的字符串名称
        /// </summary>
        protected string m_EnumNameStr;

        /// <summary>
        /// 表的数据类型
        /// </summary>
        protected System.Type m_Type;
        /// <summary>
        /// 表数据类型的名称
        /// </summary>
        protected string m_TypeName;
        /// <summary>
        /// 表文件的路径
        /// </summary>
        protected string m_TablePath;

        /// <summary>
        /// 表的数据类型
        /// </summary>
        public System.Type SType
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
            }
        }

        /// <summary>
        /// 表数据类型的名称
        /// </summary>
        public string TypeName
        {
            get
            {
                return m_TypeName;
            }
        }
        /// <summary>
        /// 表文件的路径
        /// </summary>
        public string TablePath
        {
            get
            {
                return m_TablePath;
            }
        }
        /// <summary>
        /// 枚举名称
        /// </summary>
        public string EnumName { get => m_EnumName; set => m_EnumName = value; }
        /// <summary>
        /// 
        /// </summary>
        public ExcelTableInfo()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="t">表的数据类型</param>
        /// <param name="enumName">数据表的枚举</param>
        /// <param name="tableName">表文件的名称</param>
        /// <param name="tPath">表文件的路径</param>
        /// <param name="priority">加载的优先级，目前都是0</param>
        /// <param name="uData">自定义的数据</param>
        public ExcelTableInfo(string enumName, System.Type t, string tPath)
        {
            m_Type = t;
            m_TypeName = t.FullName;
            m_TablePath = tPath;
            m_EnumName = enumName;
            m_EnumNameStr = enumName.ToString();
        }
        /// <summary>
        /// 字符串转换成枚举
        /// </summary>
        public void Init()
        {
            m_Type = BSAssemblyManager.Instance.GetType(TypeName);
            EnumName = m_EnumNameStr;
        }
    }
}
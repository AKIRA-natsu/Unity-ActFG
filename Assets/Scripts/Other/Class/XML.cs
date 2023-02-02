using System;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

public class XML {
    /// <summary>
    /// 路径
    /// </summary>
    /// <value></value>
    private string path;
    /// <summary>
    /// xml 文件
    /// </summary>
    /// <returns></returns>
    private XmlDocument xml = new XmlDocument();
    /// <summary>
    /// <para>只读</para>
    /// <para>true下安卓平台修改路径</para>
    /// </summary>
    private bool @readonly;

    /// <summary>
    /// 默认存储StreamingAssets
    /// </summary>
    /// <param name="path"></param>
    /// <param name="readonly">true下 Android | IOS 平台修改路径</param>
    public XML(string path, bool @readonly = true) {
        this.@readonly = @readonly;

        if (!@readonly && (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)) {
            this.path = Path.Combine(Application.persistentDataPath, path);
        } else {
            this.path = Path.Combine(Application.streamingAssetsPath, path);
        }
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="create"></param>
    public void Create(Action<XmlDocument> create) {
        // 设置编码
        XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration("1.0", "UTF-8", "");
        
        // 具体内容
        create.Invoke(xml);
        
        xml.Save(path);
        $"XML: 创建{path}成功".Colorful(Color.cyan).Log();
    }

    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="read"></param>
    public void Read(Action<XmlDocument> read) {
        read.Invoke(xml);
        $"XML: 读取{path}成功".Colorful(Color.cyan).Log();
    }

    /// <summary>
    /// 添加/更新/删除
    /// </summary>
    /// <param name="update"></param>
    public void Update(Action<XmlDocument> update) {
        update.Invoke(xml);

        xml.Save(path);
        $"XML: 更新{path}成功".Colorful(Color.cyan).Log();
    }

    /// <summary>
    /// 删除xml文件
    /// </summary>
    public void Delete() {
        if (!Exist())
            return;
        
        File.Delete(path);
        xml = new XmlDocument();
    }

    /// <summary>
    /// <para>检查是否存在文件</para>
    /// <para>Android | IOS 使用UnityWebRequest</para>
    /// <para>来源：https://answers.unity.com/questions/1247609/android-streamingasset-problems-with-xml-files.html</para>
    /// </summary>
    /// <returns></returns>
    public bool Exist() {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (@readonly) {
            UnityWebRequest request = UnityWebRequest.Get(path);
            var operation = request.SendWebRequest();
            while (!operation.isDone) {}
            if (request.result == (UnityWebRequest.Result.ConnectionError | UnityWebRequest.Result.ProtocolError))
                return false;
            
            xml.LoadXml(request.downloadHandler.text);
        } else {
#endif
        if (!File.Exists(path))
            return false;

        xml.Load(path);
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        }
#endif
        return true;
    }
}
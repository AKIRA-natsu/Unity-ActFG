using System;
using System.IO;
using System.Xml;
using UnityEngine;

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

    public XML(string path) => this.path = path;

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="create"></param>
    public void Create(Action<XmlDocument> create) {
        if (Exist()) {
            $"XML: 文件已存在 => {path}".Colorful(Color.cyan).Log();
            return;
        }
        
        // 设置编码
        XmlDeclaration xmlDeclaration = xml.CreateXmlDeclaration("1.0", "UTF-8", "");
        
        // 具体内容
        create.Invoke(xml);
        
        xml.Save(path);
        $"XML: 创建{path}成功".Colorful(Color.cyan).Log();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="add"></param>
    public void Add(Action<XmlDocument> add) {
        if (!Exist())
            return;
        
        // 具体内容
        add.Invoke(xml);

        xml.Save(path);
        $"XML: 添加{path}成功".Colorful(Color.cyan).Log();
    }

    /// <summary>
    /// 读取
    /// </summary>
    /// <param name="read"></param>
    public void Read(Action<XmlDocument> read) {
        if (!Exist())
            return;
        
        read.Invoke(xml);
        $"XML: 读取{path}成功".Colorful(Color.cyan).Log();
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="update"></param>
    public void UpdateXML(Action<XmlDocument> update) {
        if (!Exist())
            return;
        
        update.Invoke(xml);

        xml.Save(path);
        $"XML: 更新{path}成功".Colorful(Color.cyan).Log();
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="delete"></param>
    public void DeleteXML(Action<XmlDocument> delete) {
        if (!Exist())
            return;
        
        delete.Invoke(xml);

        xml.Save(path);
        $"XML: 删除{path}成功".Colorful(Color.cyan).Log();
    }

    /// <summary>
    /// 删除xml文件
    /// </summary>
    public void DeleteXML() {
        if (!Exist())
            return;
        
        File.Delete(path);
        xml = new XmlDocument();
    }

    /// <summary>
    /// 检查是否存在文件
    /// </summary>
    /// <returns></returns>
    private bool Exist() {
        if (!File.Exists(path))
            return false;

        xml.Load(path);
        return true;
    }
}
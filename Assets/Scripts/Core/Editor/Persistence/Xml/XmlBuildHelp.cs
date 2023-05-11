using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEditor;

/// <summary>
/// <para>打包前后回调</para>
/// <para>File FileInfo</para>
/// <para>Directory DirectoryInfo</para>
/// <para>Async I/O</para>
/// </summary>
public class XmlBuildHelp : IPreprocessBuildWithReport, IPostprocessBuildWithReport {
    public int callbackOrder => 0;

    /// <summary>
    /// 打包临时文件夹
    /// </summary>
    public readonly static string TempFolder = Path.Combine(Application.streamingAssetsPath, "~TempXml");

    public void OnPostprocessBuild(BuildReport report) {
        ResumeFiles();
    }

    public void OnPreprocessBuild(BuildReport report) {
        var xml = XMLWindow.xml;
        if (!xml.Exist())
            return;
        List<string> fileNames = new();
        xml.Read(x => {
            var data = x.SelectSingleNode("BuildData");
            var nodes = data.ChildNodes;
            foreach (XmlElement node in nodes) {
                if (Convert.ToBoolean(node.GetAttribute("Filter"))) {
                    fileNames.Add(node.Name.Replace("_", " "));
                }
            }
        });
        Directory.CreateDirectory(TempFolder);
        foreach (var file in fileNames) {
            var metaFileName = file + ".meta";
            var curXmlPath = Path.Combine(Application.streamingAssetsPath, file);
            var curMetaPath = Path.Combine(Application.streamingAssetsPath, metaFileName);
            var newXmlPath = Path.Combine(TempFolder, file);
            var newMetaPath = Path.Combine(TempFolder, metaFileName);
            FileInfo xmlFile = new FileInfo(curXmlPath);
            FileInfo metaFile = new FileInfo(curMetaPath);
            xmlFile.MoveTo(newXmlPath);
            metaFile.MoveTo(newMetaPath);
        }
    }

    /// <summary>
    /// 从StreamingAssets的~TempXml文件夹恢复文件
    /// </summary>
    public static void ResumeFiles() {
        DirectoryInfo direction = new DirectoryInfo(TempFolder);
        if (!direction.Exists)
            return;
        var files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
        foreach (var file in files) {
            var originPath = file.FullName.Replace($"\\~TempXml", "");
            file.MoveTo(originPath);
        }
        direction.Delete();
        File.Delete(TempFolder + ".meta");
        AssetDatabase.Refresh();
    }
}
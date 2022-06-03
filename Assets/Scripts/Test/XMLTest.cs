using System.Xml;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMLTest : MonoBehaviour {
    public XML xml = new XML(Application.streamingAssetsPath + "/my.xml");

    private void Update() {
        if (Input.GetKeyDown(KeyCode.C))
            CreateXML();
        if (Input.GetKeyDown(KeyCode.A))
            AddXML();
        if (Input.GetKeyDown(KeyCode.R))
            ReadXML();
        if (Input.GetKeyDown(KeyCode.U))
            UpdateXML();
        if (Input.GetKeyDown(KeyCode.D))
            DeleteXML();
    }

    public void CreateXML() {
        xml.Create((x) => {
            XmlElement root = x.CreateElement("Data");
            XmlElement info = x.CreateElement("Info");
            info.SetAttribute("Name", "小王");
            info.SetAttribute("Age", "28");
            info.SetAttribute("Phone", "12345678");
            root.AppendChild(info);
            x.AppendChild(root);
        });
    }

    public void AddXML() {
        xml.Add((x) => {
            XmlNode root = x.SelectSingleNode("Data");
            XmlElement info = x.CreateElement("Info");
            info.SetAttribute("Name", "小李");
            info.SetAttribute("Age", "25");
            info.SetAttribute("Phone", "87654321");
            root.AppendChild(info);
            x.AppendChild(root);
        });
    }

    public void ReadXML() {
        xml.Read((x) => {
            XmlNodeList nodeList = x.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement xe in nodeList) {
                xe.GetAttribute("Name").Log();
                xe.GetAttribute("Age").Log();
                xe.GetAttribute("Phone").Log();
            }
        });
    }

    public void UpdateXML() {
        xml.UpdateXML((x) => {
            XmlNodeList nodeList = x.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement xe in nodeList) {
                if (xe.GetAttribute("Name").Equals("小王")) {
                    xe.SetAttribute("Name", "大王");
                    xe.SetAttribute("Age", "31");
                    break;
                }
            }
        });
    }

    public void DeleteXML() {
        xml.DeleteXML((x) => {
            XmlNodeList nodeList = x.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement xe in nodeList) {
                if (xe.GetAttribute("Name").Equals("小李")) {
                    xe.RemoveAttribute("Phone");
                }
            }
        });
    }
}

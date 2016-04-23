using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace RubikCube
{
    class SaveGame
    {
        private XmlDocument doc;
        private string fileName;
        private string grandFather;
        private bool didFail;
        public SaveGame(string FileName,string GrandFather)
        {
            try
            {
                doc = new XmlDocument();
                fileName = FileName;
                Path.GetFullPath(fileName);
                doc.Load(fileName);
                grandFather = GrandFather;
                EndingProt();
                didFail = false;
            }
            catch (Exception)
            {
                didFail = true;
                throw;
            }

        }

        public void AddBool(string boolName, string boolValue)
        {
            if (!didFail)
            {
                doc.Load(fileName);
                XmlElement boolElement = doc.CreateElement("bool");
                XmlElement name = doc.CreateElement("name");
                XmlElement value = doc.CreateElement("value");
                name.InnerText = boolName;
                value.InnerText = boolValue;
                boolElement.AppendChild(name);
                boolElement.AppendChild(value);
                doc.GetElementsByTagName(grandFather)[0].AppendChild(boolElement);
                EndingProt();
            }
        }

        public void SaveLogialCubeState(int[,,] state)
        {
            if (!didFail)
            {
                doc.Load(fileName);
                XmlElement intElement = doc.CreateElement("");
            }
        }

        public List<Tuple<string, string>> LoadBools()
        {
            List<Tuple<string, string>> result = new List<Tuple<string, string>>();
            if (!didFail)
            {
                doc.Load(fileName);
                XmlNodeList nodeList = doc.GetElementsByTagName("bool");
                for (int i = 0; i < nodeList.Count; i++)
                {
                    result.Add(new Tuple<string, string>(nodeList[i].ChildNodes[0].InnerText,
                        nodeList[i].ChildNodes[1].InnerText));
                }
            }
            return result;
        }

        private void EndingProt()
        {
            if (!didFail)
            {
                FileStream stream = new FileStream(fileName, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite);
                doc.Save(stream);
                stream.Close();
            }
        }
    }
}

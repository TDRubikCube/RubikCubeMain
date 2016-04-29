using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace RubikCube
{
    /// <summary>
    /// Saves some elements of the game into the computer of the user
    /// </summary>
    class SaveGame
    {
        private readonly XmlDocument doc; //An XML Document
        private readonly string fileName; //The name of the file
        private readonly string grandFather; //Contains all Vars from the same type
        private readonly bool didFail; //Did the save fail
        /// <summary>
        /// Constract SaveGame
        /// Tries to save the game
        /// </summary>
        /// <param name="FileName">The name of the file</param>
        /// <param name="GrandFather">The GrandFather</param>
        public SaveGame(string FileName,string GrandFather)
        {
            try //Tries to create a save file
            {
                doc = new XmlDocument(); //Sets dox as a XML file
                fileName = FileName; //Sets the name of the file in fileName
                Path.GetFullPath(fileName); //Gets the path of the file
                doc.Load(fileName); //Loads the file to the dox
                grandFather = GrandFather; //Sets the grandFather
                EndingProt();
                didFail = false; //Notifies the game that the save did not fail
            }
            catch (Exception) //If the game fails saving
            {
                didFail = true; //Notifies the game that the save failed
            }

        }
        /// <summary>
        /// Edits the XML document
        /// </summary>
        /// <param name="boolName">The name of the bool</param>
        /// <param name="boolValue">The value of the bool</param>
        public void AddBool(string boolName, string boolValue)
        {
            if (!didFail) //If the save did not fail
            {
                doc.Load(fileName); //Loads the file into the doc
                XmlElement boolElement = doc.CreateElement("bool");
                XmlElement name = doc.CreateElement("name"); //Creates a new element in the doc named "name"
                XmlElement value = doc.CreateElement("value")//Creates a new element in the doc named "value"
                name.InnerText = boolName;
                value.InnerText = boolValue;
                boolElement.AppendChild(name);
                boolElement.AppendChild(value);
                doc.GetElementsByTagName(grandFather)[0].AppendChild(boolElement);
                EndingProt();
            }
        }

        /// <summary>
        /// Saves the logical state of the cube
        /// </summary>
        /// <param name="state">The current logical state of the cube</param>
        public void SaveLogialCubeState(int[,,] state)
        {
            if (!didFail)//If the save didn't fail
            {
                doc.Load(fileName); //Loads the file from the doc
                XmlElement intElement = doc.CreateElement("");
            }
        }
        /// <summary>
        /// Loads bools
        /// </summary>
        /// <returns></returns>
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

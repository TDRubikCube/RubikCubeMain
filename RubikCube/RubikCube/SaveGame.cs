using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace RubikCube
{
    class SaveGame
    {
        //create a new xmlDoc, which will be the save file
        private readonly XmlDocument doc;

        //the name of the save file
        private readonly string fileName;

        //the root which binds the bools together under it
        private readonly string grandFather;

        //a flag which represents whether the initializing failed or not
        private readonly bool didFail;

        /// <summary>
        /// Saving the cube state and other information of the user's preferences
        /// </summary>
        /// <param name="FileName">the name of the save file (full path)</param>
        /// <param name="GrandFather">the name of the grandfather root</param>
        public SaveGame(string FileName,string GrandFather)
        {
            //this is int "try" since its expreimental
            try
            {
                //create a new xmlDoc
                doc = new XmlDocument();

                //get the file name
                fileName = FileName;

                //get the path to the file
                Path.GetFullPath(fileName);

                //load the file into the doc
                doc.Load(fileName);

                //set the grandfather
                grandFather = GrandFather;

                //save and close
                EndingProt();

                //since it reached the end, mark it as false
                didFail = false;
            }
            catch (Exception)
            {
                //since it failed mark as true
                didFail = true;
            }

        }

        /// <summary>
        /// add a new bool to save in the file
        /// </summary>
        /// <param name="boolName">name of the bool</param>
        /// <param name="boolValue">the bool's value</param>
        public void AddBool(string boolName, string boolValue)
        {
            if (!didFail)
            {
                //load the save file
                doc.Load(fileName);

                //create a new element of "bool"
                XmlElement boolElement = doc.CreateElement("bool");

                //create a new element of "name"
                XmlElement name = doc.CreateElement("name");

                //create a new element of "value"
                XmlElement value = doc.CreateElement("value");

                //set bool name as the one given
                name.InnerText = boolName;

                //set the bool value as the one given
                value.InnerText = boolValue;

                //set the name as a child of the bool
                boolElement.AppendChild(name);

                //set the value as a child of the bool
                boolElement.AppendChild(value);

                //set the bool as the child of the grandfather
                doc.GetElementsByTagName(grandFather)[0].AppendChild(boolElement);

                //save and close
                EndingProt();
            }
        }

        /// <summary>
        /// meant to save the state of the cube
        /// </summary>
        /// <param name="state">state to save</param>
        public void SaveLogialCubeState(int[,,] state)
        {
            //it should be noted that this is a work in progress 

            if (!didFail)
            {
                //load the file
                doc.Load(fileName);
            }
        }

        /// <summary>
        /// load the bools from the save file
        /// </summary>
        /// <returns></returns>
        public List<Tuple<string, string>> LoadBools()
        {
            //create the return value
            List<Tuple<string, string>> result = new List<Tuple<string, string>>();
            if (!didFail)
            {
                //load the file
                doc.Load(fileName);

                //make a list of nodes, which contains all the elements named bool
                XmlNodeList nodeList = doc.GetElementsByTagName("bool");

                for (int i = 0; i < nodeList.Count; i++)
                {
                    //add each bool to the list of result
                    result.Add(new Tuple<string, string>(nodeList[i].ChildNodes[0].InnerText, // the name of the bool
                        nodeList[i].ChildNodes[1].InnerText)); // the value of the bool
                }
            }
            return result;
        }

        /// <summary>
        /// save the file with the new changes
        /// </summary>
        private void EndingProt()
        {
            if (!didFail)
            {
                //open the stream
                FileStream stream = new FileStream(fileName, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite);

                //save
                doc.Save(stream);

                //close the stream
                stream.Close();
            }
        }
    }
}
﻿/*
 *GRASP(Geo-referential Real-time Acquisition Statistics Platform) Reporting Tool <http://www.brainsen.com>
 * Developed by Brains Engineering s.r.l (marco.giorgi@brainsen.com)
 * This file is part of GRASP Reporting Tool.  
 *  GRASP Reporting Tool is free software: you can redistribute it and/or modify it
 *  under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or (at
 *  your option) any later version.  
 *  GRASP Reporting Tool is distributed in the hope that it will be useful, but
 *  WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser  General Public License for more details.  
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with GRASP Reporting Tool. 
 *  If not, see <http://www.gnu.org/licenses/>
 */

using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms.VisualStyles;
using System.Xml;
/// <summary>
/// Used to receive HTTP Requests from Mobile, fetch them and then save data in the Response Files folder.
/// </summary>
public partial class MobileConnection : System.Web.UI.Page
{

    private string FixBase64ForImage(string Image)
    {
        System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);

        sbText.Replace("\r\n", String.Empty);

        sbText.Replace(" ", String.Empty);

        sbText.Replace('-', '+');

        sbText.Replace('_', '/');

        sbText.Replace(@"\/", "/");

        return sbText.ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string senderF = "";
            string data = "";
            string formResponseName = string.Empty;

            System.Collections.Specialized.NameValueCollection postedValues = Request.Form;
            senderF = postedValues[0];
            data = Server.HtmlDecode(postedValues[1]);

            if (data == "test")
            {
                handleTestRequest(senderF);
            }
            else if (Request.QueryString["call"] != null)
            {
                string parameter = Request.QueryString["call"].ToString();
                switch (parameter)
                {
                    case "test":
                        handleTestRequest(senderF);
                        break;
                    case "response":
                    case "editedResponse":
                        formResponseName = postedValues[2];
                        string formId = postedValues[3];
                        if (senderF == null || senderF == "")
                        {
                            Response.Clear();
                            Response.ContentType = "text/plain";
                            Response.Write("ERROR:Client phone number not received");
                            break;
                        }
                        bool isEditedResponse = parameter.Equals("editedResponse");
                        handleResponseRequest(data, senderF, formResponseName, formId, isEditedResponse);
                        break;
                    case "sync":
                        if (senderF == null || senderF == "")
                        {
                            Response.Clear();
                            Response.ContentType = "text/plain";
                            Response.Write("ERROR:Client phone number not received");
                            break;
                        }
                        HandleSyncRequest(data, senderF);
                        break;
                    default:
                        Response.Clear();
                        Response.ContentType = "text/plain";
                        Response.Write("Generating a request response generated an error");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            LogUtils.WriteErrorLog(ex.ToString());
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Write(ex.Message);
        }
    }

    private void handleTestRequest(string senderF)
    {
        Response.Clear();
        Response.ContentType = "text/plain";
        Response.Write(User_Credential.checkUserFromNumber(senderF));
    }

    private void handleResponseRequest(string data, string senderF, string formName, string formId, bool isEditedResponse)
    {
        Response.Clear();
        Response.ContentType = "text/plain";
        IncomingProcessor incomProc = new IncomingProcessor();
        string saveFormInstanceResult = incomProc.SaveFileResponse(data, senderF, formName, formId, isEditedResponse);
        Response.Write(saveFormInstanceResult);
    }

    private void HandleSyncRequest(string data, string sender)
    {
        data = data.Replace("\n", "").Replace("\r", "").Replace("  ", "");
        XmlDocument xmlData = Utility.TryParseXml(data);

        if(xmlData != null)
        {
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Write(GetManipulatedFormsXML(xmlData, sender));
        }
        else
        {
            Response.Clear();
            Response.ContentType = "text/plain";
            Response.Write("Generating a request response generated an error");
        }
    }

    /// <summary>
    /// Gets the manipulated list of forms, except of the given form Ids (the mobile sends the current installed form Ids,
    /// so reporting sends the other forms).
    /// </summary>
    /// <param name="formsIDList"></param>
    /// <param name="phone"></param>
    /// <returns></returns>
    private string GetManipulatedFormsXML(XmlDocument formsIDList, string phone)
    {
        try
        {
            List<string> downloadedForms = new List<string>(); //Represents the downloaded forms in the requested mobile.
            XmlNodeList formIDs = formsIDList.GetElementsByTagName("form");

            //Get a list of the downloaded forms ids in user's mobile.
            for (int i = 0; i < formIDs.Count; i++)
            {
                downloadedForms.Add(formIDs[i].InnerXml);
            }

            StringBuilder allFormsXML = new StringBuilder();
            string allFormsXMLResponse = global::Form.GetAllXFormsByPhoneNumber(phone, downloadedForms);

            allFormsXML.Append(allFormsXMLResponse);

            //Add a temporary root, in order to parse it as XML. Will be removed after parse it and modify the xml (remove empty groups).
            allFormsXML.Insert(0, "<TempRoot>");
            allFormsXML.Append("</TempRoot>");

            XmlDocument formsXMLDoc = Utility.TryParseXml(allFormsXML.ToString());

            if (formsXMLDoc != null)
            {
                //Get all groups elements.
                XmlNodeList groupNodes = formsXMLDoc.GetElementsByTagName("group");

                //Removes all empty field-list groups.
                int iCount = 0;
                while (iCount < groupNodes.Count)
                {
                    XmlNode groupNode = groupNodes.Item(iCount);
                    if (groupNode != null
                        && (groupNode.Attributes["appearance"] != null
                           && groupNode.Attributes["appearance"].Value.Equals("field-list")))
                    {
                        if (groupNode.ChildNodes.Count == 0)
                        {
                            groupNodes.Item(iCount).ParentNode.RemoveChild(groupNodes.Item(iCount));
                            iCount--;
                        }
                    }
                    iCount++;
                }

                //Get the xml after cleaning the empty groups.
                allFormsXML.Clear();
                allFormsXML.Append(formsXMLDoc.InnerXml);
                
                //Removes the temporary root.
                allFormsXML.Replace("<TempRoot>", "");
                allFormsXML.Replace("</TempRoot>", "");

                //Replaces some characters.
                allFormsXML.Replace("&", "&amp;")
                    .Replace("\"", "&quot;")
                    .Replace("'", "&apos;")
                    .Replace("<", "&lt;")
                    .Replace(">", "&gt;");
                allFormsXML.Replace("&lt;form&gt;", "<form>&lt;?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?&gt;");
                allFormsXML.Replace("&lt;/form&gt;", "</form>");

                //Insert <forms> root. Send the status of all current downloaded forms with a semicolon with the new finalized forms. Mobile first get the first 
                //section of the current downloaded forms and check their statuses, then get the new forms section and download them.
                allFormsXML.Insert(0, "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><forms>");
                allFormsXML.Append("</forms>");
            }

            return allFormsXML.ToString();
        }
        catch (Exception ex)
        {
            LogUtils.WriteErrorLog(ex.ToString());
            return GetEmptyXmlFormsList();
        }
    }

    private string GetEmptyXmlFormsList()
    {
        return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\"?><forms></forms>";
    }
}
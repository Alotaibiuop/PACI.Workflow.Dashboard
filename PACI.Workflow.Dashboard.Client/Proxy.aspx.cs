using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;

public partial class Proxy : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string ProxyURL = string.Empty;

        try
        {
            ProxyURL = HttpUtility.UrlDecode(Request.QueryString["u"].ToString());
        }

        catch { }

        if (ProxyURL != string.Empty)
        {
            string BodyText = string.Empty;

            HttpWebRequest aHttpWebRequest = (HttpWebRequest)WebRequest.Create(ProxyURL);
            aHttpWebRequest.ContentType = "text/xml; charset=utf-8";
            aHttpWebRequest.Method = "POST";
            aHttpWebRequest.Headers.Add("SOAPAction", Request.Headers["SOAPAction"]);

            MemoryStream BodyStream = new MemoryStream();
            Request.InputStream.CopyTo(BodyStream);
            BodyStream.Position = 0;

            using (StreamReader BodyStreamReader = new StreamReader(BodyStream))
            {
                BodyText = WebUtility.HtmlDecode(BodyStreamReader.ReadToEnd());
            }

            byte[] byteArray = Encoding.UTF8.GetBytes(BodyText);
            aHttpWebRequest.ContentLength = byteArray.Length;

            try
            {
                Stream WebRequestDataStream = aHttpWebRequest.GetRequestStream();
                WebRequestDataStream.Write(byteArray, 0, byteArray.Length);
                WebRequestDataStream.Close();

                HttpWebResponse WebResponse = (HttpWebResponse)aHttpWebRequest.GetResponse();

                if (WebResponse.StatusCode.ToString().ToUpper() == "OK")
                {
                    using (Stream Content = WebResponse.GetResponseStream())
                    {
                        StreamReader ContentReader = new StreamReader(Content);
                        Response.ContentType = WebResponse.ContentType;

                        Response.Write(ContentReader.ReadToEnd());
                    }
                }
            }
            catch (Exception Exception)
            {
                Response.Write(Exception.Message);
            }
        }
    }
}


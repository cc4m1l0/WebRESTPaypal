using Newtonsoft.Json.Linq;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebRESTPaypal.Helpers;

namespace WebRESTPaypal
{
    public partial class Index : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                CreateNewCard();
            }
        }

        private void CreateNewCard()
        {
            lbEstado.Visible = false;
            btnCrearTarjeta.Visible = false;
            // ### Api Context
            // Pass in a `APIContext` object to authenticate 
            // the call and to send a unique request id 
            // (that ensures idempotency). The SDK generates
            // a request id if you do not pass one explicitly. 
            // See [Configuration.cs](/Source/Configuration.html) to know more about APIContext.
            var apiContext = Configuration.GetAPIContext();

            lbMetodo.Text = "Método ejecutado: Create de la clase CreditCard";
            lbTarea.Text = "Tarea: Crear una nueva tarjeta de crédito";
            // A resource representing a credit card that can be used to fund a payment.
            var card = new CreditCard()
            {
                expire_month = 11,
                expire_year = 2018,
                number = "4877274905927862",
                type = "visa",
                cvv2 = "874"
            };
            // Creates the credit card as a resource in the PayPal vault. The response contains an 'id' that you can use to refer to it in the future payments.
            var cd = card.Create(apiContext);

            var linksresponse = cd.links;
            lvLinks.DataSource = linksresponse;
            lvLinks.DataBind();

            lbResponse.Text = cd.ConvertToJson();
        }

        protected void lvLinks_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if(e.CommandName == "Execute")
            {
                string data = e.CommandArgument.ToString();
                if (data != "")
                {
                    CreateNewRequest(data);
                }
            }
        }

        private void CreateNewRequest(string data)
        {
            string[] commandArgs = data.Split(new char[] { ',' });
            Links l = new Links();
            l.href = commandArgs[0];
            l.rel = commandArgs[1];
            l.method = commandArgs[2];

            var apiContext = Configuration.GetAPIContext();


            lbMetodo.Text = "Método ejecutado: " + l.method + " de la clase CreditCard";
            lbTarea.Text = "Tarea: " + l.method.ToLower() + " tarjeta de crédito";

            string url = l.href;
            try
            {
                WebRequest myReq = WebRequest.Create(url);
                myReq.ContentType = "application/json";
                myReq.Method = l.method;
                CredentialCache mycache = new CredentialCache();
                myReq.Headers["Authorization"] = apiContext.AccessToken;
                WebResponse wr = myReq.GetResponse();
                HttpWebResponse httpResponse = (HttpWebResponse)wr;

                if (httpResponse.StatusCode.ToString() == "OK")
                {
                    Stream receiveStream = wr.GetResponseStream();

                    StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                    string content = reader.ReadToEnd();
                    Console.WriteLine(content);
                    var json = "[" + content + "]"; // change this to array
                    var objects = JArray.Parse(json); // parse as array  
                    foreach (JObject o in objects.Children<JObject>())
                    {
                        foreach (JProperty p in o.Properties())
                        {
                            string name = p.Name;
                            string value = p.Value.ToString();
                            Console.Write(name + ": " + value);
                        }
                    }
                    lbResponse.Text = json;
                }

                else if (httpResponse.StatusCode.ToString() == "NoContent")
                {
                    var linksresponse = new List<Links>();
                    lvLinks.DataSource = linksresponse;
                    lvLinks.DataBind();
                    lbResponse.Text = "Tarjeta eliminada exitosamente";
                    lbEstado.Text = "Tarjeta eliminada exitosamente";
                    lbEstado.Visible = true;
                    btnCrearTarjeta.Visible = true;
                }

                else
                {
                    lbResponse.Text = "Ocurrió un error";
                }
            }
            catch (Exception exc)
            {
                string e = exc.Message;
                lbResponse.Text = "Ocurrió un error";
            }
        }

        protected void btnCrearTarjeta_Click(object sender, EventArgs e)
        {
            CreateNewCard();
        }
    }
}
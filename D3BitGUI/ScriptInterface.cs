using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace D3BitGUI
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class ScriptInterface
    {
        private CardForm _cardForm;

        public ScriptInterface(CardForm cardForm)
        {
            _cardForm = cardForm;
        }

        public void Upload(string destin, string itemName, string itemQuality, string itemType, string itemDps, string itemStats) 
        {
            Thread t = new Thread(()=>
                                      {
                                          byte[] data = File.ReadAllBytes(_cardForm.TooltipPath);
                                          if (destin == "d3bit")
                                          {
                                              Dictionary<string, object> postParameters = new Dictionary<string, object>();
                                              postParameters.Add("filename", Path.GetFileName(_cardForm.TooltipPath));
                                              postParameters.Add("fileformat", Path.GetExtension(_cardForm.TooltipPath));
                                              postParameters.Add("xyz", "placeholder");
                                              postParameters.Add("n", itemName);
                                              postParameters.Add("q", itemQuality);
                                              postParameters.Add("d", itemDps);
                                              postParameters.Add("t", itemType);
                                              postParameters.Add("a", itemStats);
                                              postParameters.Add("s", Properties.Settings.Default.Secret.Trim());
                                              postParameters.Add("uploadedfile", data);

                                              // Create request and receive response
                                              string postURL = "http://d3bit.com/ajax/uploaditem/";
                                              string userAgent =
                                                  "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3";
                                              HttpWebResponse webResponse = Util.MultipartFormDataPost(postURL, userAgent, postParameters);
                                              StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                                              string res = responseReader.ReadToEnd();
                                              webResponse.Close();

                                              JObject o = JObject.Parse(res);
                                              if (o["status"] != null && o["status"].ToString() == "success" && o["link"] != null)
                                              {
                                                  _cardForm.UIThread(() => Clipboard.SetText(o["link"].ToString()));
                                                  InvokeScript("Signal", string.Format("Success! Link {0} copied to Clipboard.", o["link"]));

                                                  GUI.Log("Uploaded. {0}", o["link"]);
                                              }
                                              else if (o["msg"] != null)
                                              {
                                                  InvokeScript("Signal", o["msg"]);
                                              }
                                              else
                                              {
                                                  InvokeScript("Signal", "Error Uploading.");
                                              }
                                          }
                                          else if (destin == "imgur")
                                          {
                                              Dictionary<string, object> postParameters = new Dictionary<string, object>();
                                              postParameters.Add("filename", Path.GetFileName(_cardForm.TooltipPath));
                                              postParameters.Add("fileformat", Path.GetExtension(_cardForm.TooltipPath));
                                              postParameters.Add("key", "4c379d346aaf18a942734de377c4cda0");
                                              postParameters.Add("title", "Uploaded from D3Bit.com");
                                              postParameters.Add("caption", string.Format("Name: {4}, Quality: {0} Type: {1} DPS/Armor: {2} Stats: {3}", itemQuality, itemType, itemDps, itemStats, itemName));
                                              postParameters.Add("image", data);

                                              string postURL = "http://api.imgur.com/2/upload.json";
                                              string userAgent =
                                                  "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3";
                                              HttpWebResponse webResponse = Util.MultipartFormDataPost(postURL, userAgent, postParameters);
                                              StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                                              string res = responseReader.ReadToEnd();
                                              webResponse.Close();

                                              JObject o = JObject.Parse(res);
                                              if (o["upload"] != null && o["upload"]["links"] != null && o["upload"]["links"]["imgur_page"] != null)
                                              {
                                                  string link = o["upload"]["links"]["imgur_page"].ToString();
                                                  _cardForm.UIThread(() => Clipboard.SetText(link));
                                                  InvokeScript("Signal", string.Format("Success! Link {0} copied to Clipboard.", link));
                                                  GUI.Log("Uploaded. {0}", link);
                                              }
                                              else
                                              {
                                                  InvokeScript("Signal", "Error Uploading.");
                                              }
                                          }
                                      });
            t.Start();
            
        }

        public void Close()
        {
            _cardForm.Close();
        }

        private object InvokeScript(string name, params object[] args)
        {
            object o = null;
            _cardForm.UIThread(()=>
                                   {
                                       o = _cardForm.browser.Document.InvokeScript(name, args);
                                   });
            return o;
        }

    }
}

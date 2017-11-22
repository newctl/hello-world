using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Runtime.InteropServices;
using SHDocVw;
using mshtml;
using Microsoft.Win32;
using System.IO;

namespace ShowTrueUrl3
{
    [
         ComVisible(true),
         Guid("8a194578-81ea-4850-9911-13ba2d71efbd"),
         ClassInterface(ClassInterfaceType.None)
     ]
    public class BHO : IObjectWithSite
    {
        WebBrowser webBrowser;
        HTMLDocument doc;

        public void OnDocumentComplete(object pDisp, ref object URL)
        {
            try
            {
                doc = (HTMLDocument)webBrowser.Document;
                var body = (HTMLBody)doc.body;
                //string urlInputHTML = @"<input type='text' value='hack" + webBrowser.LocationURL + @" '  />";
                string urlInputHTML = @"<input id='tangle' type='text' value='真实网址是：" + webBrowser.LocationURL+"董董董" + @" ' style='color:red;  width: 800px; height:30px;'>";
                foreach(IHTMLElement ele in body.getElementsByTagName("INPUT"))
                {
                    if (ele.id == "tangle")
                    {
                        ele.outerHTML = "";
                    }
                }
                body.insertAdjacentHTML("afterBegin", urlInputHTML);

               /* foreach (IHTMLInputElement element in doc.getElementsByTagName("INPUT"))
                {
                    System.Windows.Forms.MessageBox.Show(element.value);
                }*/
            }
            catch (Exception e)
            {
               // System.Windows.Forms.MessageBox.Show(e.Message);
            }

        }

        private void clickDaKa()
        {
            Random ra = new Random();
            int rest = 10000;
            if (webBrowser.LocationURL.Contains("150.48.16.61"))
            {
                while (true)
                {
                    if(DateTime.Now.Hour==8 || DateTime.Now.Hour == 12 || DateTime.Now.Hour == 14 || DateTime.Now.Hour == 18)
                    {
                        rest = ra.Next(10, 20);
                        Thread.Sleep(1000*rest*60);
                        string result = "";
                        if (DateTime.Now.Hour == 8 || DateTime.Now.Hour == 14)
                        {
                            result = this.webBrowser.Document.InvokeScript("punck(1)").ToString();
                        }else
                        {
                            result = this.webBrowser.Document.InvokeScript("punck(2)").ToString();
                        }
                        Thread.Sleep(1000 * 3600);
                    }
                    Thread.Sleep(1000 * 60);
                }
            }
        }

        public int SetSite(object site)
        {
            if (site != null)
            {
                webBrowser = (WebBrowser)site;
                webBrowser.DocumentComplete += new DWebBrowserEvents2_DocumentCompleteEventHandler(this.OnDocumentComplete);
            }
            else
            {
                webBrowser.DocumentComplete -= new DWebBrowserEvents2_DocumentCompleteEventHandler(this.OnDocumentComplete);
                webBrowser = null;
            }
            return 0;
        }

        public int GetSite(ref Guid guid, out IntPtr ppvSite)
        {
            IntPtr punk = Marshal.GetIUnknownForObject(webBrowser);
            int hr = Marshal.QueryInterface(punk, ref guid, out ppvSite);
            Marshal.Release(punk);
            return hr;
        }



        public static string BHOKEYNAME = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Browser Helper Objects";

        [ComRegisterFunction]
        public static void RegisterBHO(Type type)
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(BHOKEYNAME, true);
            if (registryKey == null)
                registryKey = Registry.LocalMachine.CreateSubKey(BHOKEYNAME);
            string guid = type.GUID.ToString("B");
            RegistryKey ourKey = registryKey.OpenSubKey(guid);
            if (ourKey == null)
                ourKey = registryKey.CreateSubKey(guid);

            registryKey.Close();
            ourKey.Close();
        }

        [ComUnregisterFunction]
        public static void UnregisterBHO(Type type)
        {
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(BHOKEYNAME, true);
            string guid = type.GUID.ToString("B");

            if (registryKey != null)
                registryKey.DeleteSubKey(guid, false);
        }

    }
}

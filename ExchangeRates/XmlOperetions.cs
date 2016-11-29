using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;

namespace ExchangeRates
{
    class XmlOperetions
    {
        string url = @"http://cb.am/latest.json.php";
        const string path = @"C:\Users\Home\Desktop\ExchangeRates\ExchangeRates\History\";
        const string timeFormat = "ddMMyyyyhhmmss";
        string time;
        XmlDocument xDoc;
        public async Task<XmlDocument> GetXml()
        {
            HttpClient httpclient = new HttpClient();
            string jsonString = await httpclient.GetStringAsync(url);
            xDoc = await Task.Run(() => JsonConvert.DeserializeXmlNode(jsonString, "Currencies"));
            time = DateTime.Now.ToString(timeFormat);
            return xDoc;
        }
        public void Update(string jsonString)
        {

        }
        public string SaveXmlInHistory()
        {
            if (xDoc != null)
            {
                xDoc.Save(path + time);
                xDoc = null;
                return DateTime.ParseExact(time, timeFormat, null).ToString();
            }
            return null;
        }

        public void GetXmlHistory(/*DateTime time*/)
        {
            //XmlDocument xDoc = new XmlDocument();
            //xDoc.LoadXml(path+ "Currencies");
            //var fileList = new DirectoryInfo(path).GetFiles();
            //return fileList;
            //string timeString = time.ToString(timeFormat);
            //foreach (var item in fileList)
            //{
            //    if (timeString == item.Name)
            //    {
            //        var loadDoc = XDocument.Load(path + timeString);
            //    }
            //}
        }
        public IEnumerable GetHistoryDates()
        {
            foreach (var item in Directory.GetFiles(path))
            {
                var s = Path.GetFileName(item);

                yield return DateTime.ParseExact(s, timeFormat, null);
            }

        }

        public void ClearHistory()
        {
            foreach (var item in Directory.GetFiles(path))
            {
                File.Delete(item);
            }


        }

    }
}

using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace FifthElement.LogforceLoadingHybrid.RestService
{
    public class RestService
    {
        public enum MethodType
        {
            POST,
            GET
        }

        private int _timeout = 60000;
        public string BaseUrl { get; private set; }

        public RestService(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public int Timeout {
            set { _timeout = value; }
        }
        public NameValueCollection CustomHeaders { get; set; }

        public string CallRestService(string methodUrl, string requestBody, MethodType methodType, string contentType = "application/json; charset=utf-8")
        {
            StringBuilder returnValue = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream resStream = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(BaseUrl + methodUrl);
                request.Method = methodType.ToString();
                if (CustomHeaders != null)
                    request.Headers.Add(CustomHeaders);

                if (methodType == MethodType.POST)
                {
                    request.ContentType = contentType;
                    byte[] byteData = UTF8Encoding.UTF8.GetBytes(requestBody);
                    request.ContentLength = byteData.Length;
                    using (Stream sw = request.GetRequestStream())
                    {
                        if (byteData.Length > 0)
                            sw.Write(byteData, 0, byteData.Length);
                    }
                }

                request.Timeout = _timeout;
                response = (HttpWebResponse)request.GetResponse();
                resStream = response.GetResponseStream();

                returnValue = new StringBuilder();

                if (resStream != null && resStream.CanRead)
                {
                    using (var sr = new StreamReader(resStream))
                    {
                        const int bufferSize = 4096;
                        var buffer = new char[bufferSize];
                        int byteCount = sr.Read(buffer, 0, buffer.Length);
                        while (byteCount > 0)
                        {
                            returnValue.Append(new string(buffer, 0, byteCount));
                            byteCount = sr.Read(buffer, 0, buffer.Length);
                        }
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (resStream != null)
                {
                    resStream.Close();
                    resStream.Dispose();
                }
            }
            return returnValue.ToString();
        }
    }
}

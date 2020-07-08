using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuinBinAPI
{
    public class JsonBinsClient
    {
        private string endpoint;
        private HttpWebRequest request;

        public JsonBinsClient(string _endpoint)
        {
            endpoint = _endpoint;
        }

        public dynamic CreateBin(string _body, Dictionary<string, string> _headers)
        {
             request = (HttpWebRequest)WebRequest.Create(endpoint);
           // request = HttpWebRequest.Create(endpoint);

            request.Method = "POST";
            foreach (var h in _headers)
            {
                if (h.Key.ToLower() == "content-type") //this is workaround to set up Content type
                    request.ContentType = h.Value;
                else
                    request.Headers[h.Key] = h.Value;
            }
            //work with body
            using (var streamWriter = new System.IO.StreamWriter(request.GetRequestStream())) // httpWebRequest sent request as strim and receive response as a stream
            {
                streamWriter.Write(_body); //we write body into stream 
            }

            return GetResponseWithoutException(); //method send request , parse response and handle exeptions 
        }

        public dynamic ReadBin(string binId, Dictionary<string, string> _headers)
        {
            request = (HttpWebRequest)WebRequest.Create(endpoint + $"/{binId}");
            request.Method = "GET";
            foreach (var h in _headers)
            {
                if (h.Key.ToLower() == "content-type")
                    request.ContentType = h.Value;
                else
                    request.Headers[h.Key] = h.Value;
            }

            return GetResponseWithoutException();
        }

        public dynamic UpdateBin(string binId, string _body, Dictionary<string, string> _headers)
        {
            request = (HttpWebRequest)WebRequest.Create(endpoint + $"/{binId}");
            request.Method = "PUT";
            foreach (var h in _headers)
            {
                if (h.Key.ToLower() == "content-type")
                    request.ContentType = h.Value;
                else
                    request.Headers[h.Key] = h.Value;
            }

            using (var streamWriter = new System.IO.StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(_body);
            }

            return GetResponseWithoutException();
        }

        public dynamic DeleteBin(string binId, Dictionary<string, string> _headers)
        {
            request = (HttpWebRequest)WebRequest.Create(endpoint + $"/{binId}"); //$ allows parameters
            request.Method = "DELETE";
            foreach (var h in _headers)
            {
                if (h.Key.ToLower() == "content-type")
                    request.ContentType = h.Value;
                else
                    request.Headers[h.Key] = h.Value;
            }

            return GetResponseWithoutException();
        }
        //send request with http exeption handling
        private dynamic GetResponseWithoutException() 
        {
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse(); //method to send request
            }
            catch (System.Net.WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }
            using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
            {
                string responseText = reader.ReadToEnd();
                return new //create dynamic obj, to work with this obj in our tests
                {
                    StatusCode = response.StatusCode,
                    Headers = response.Headers,
                    Body = JsonConvert.DeserializeObject(responseText) //convert string to json obj
                };
            }
        }

    }
}

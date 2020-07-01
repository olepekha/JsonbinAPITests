using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using NUnit;

namespace QuinBinAPI
{    
    [TestClass]
    public class BinApiTests
     {
        
        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void AddPrivateBin()
        {
            HttpWebRequest request;
            HttpWebResponse response =null;
  
            string endPoint = "https://api.jsonbin.io/b";
            request = (HttpWebRequest)WebRequest.Create(endPoint); //create http request 
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            request.Headers["secret-key"] = "$2b$10$dwnLF2rKEZIsJ2QzogLaBuZG8BTPBXSwIm8tIoIjpLfyOg9n.T2zm";

            using (var streamWriter = new System.IO.StreamWriter(request.GetRequestStream(), ASCIIEncoding.ASCII))
            {
                string CreateBinBody = JsonConvert.SerializeObject(new
                {
                    sample = "Hellow World"
                 });

                streamWriter.Write(CreateBinBody);
                streamWriter.Flush();
                streamWriter.Close();
            }

            dynamic json;
            
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();
                    json = JsonConvert.DeserializeObject(responseText);
                                 
                }
            } catch(System.Net.WebException ex)
            {
                using (var reader = new System.IO.StreamReader(ex.Response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();
                    json = JsonConvert.DeserializeObject(responseText);
                }
            }

            Assert.IsTrue(json.success.Value);
            Assert.IsTrue(json["private"].Value);
            Assert.AreEqual(json.data.sample.Value, "Hellow World");
            Assert.IsNotNull(json.id.Value);
            Assert.AreEqual(response.StatusCode.ToString(), "OK");

        }

        [TestMethod]
        public void AddPublicBin()
        {
            HttpWebRequest request;
            HttpWebResponse response =null;

            string endPoint = "https://api.jsonbin.io/b";
            request = (HttpWebRequest)WebRequest.Create(endPoint); //create http request 
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            request.Headers["secret-key"] = "$2b$10$dwnLF2rKEZIsJ2QzogLaBuZG8BTPBXSwIm8tIoIjpLfyOg9n.T2zm";
            request.Headers["private"] = "false";

            using (var streamWriter = new System.IO.StreamWriter(request.GetRequestStream(), ASCIIEncoding.ASCII))
            {
                string CreateBinBody = JsonConvert.SerializeObject(new
                {
                    sample = "Hellow World"
                });

                streamWriter.Write(CreateBinBody);
                streamWriter.Flush();
                streamWriter.Close();
            }

            dynamic json;
            
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();

                    json = JsonConvert.DeserializeObject(responseText);
                }
            }
            catch (System.Net.WebException ex)
            {
                using (var reader = new System.IO.StreamReader(ex.Response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();
                    json = JsonConvert.DeserializeObject(responseText);
                }
            }

            Assert.IsTrue(json.success.Value);
            Assert.IsFalse(json["private"].Value);
            Assert.AreEqual(json.data.sample.Value, "Hellow World");
            Assert.IsNotNull(json.id.Value);
            Assert.AreEqual(response.StatusCode.ToString(), "OK");
        }

        [TestMethod]
        public void AddPublicBintoCollection()
        {
            HttpWebRequest request;
            HttpWebResponse response = null;

            string endPoint = "https://api.jsonbin.io/b";
            request = (HttpWebRequest)WebRequest.Create(endPoint); //create http request 
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            request.Headers["secret-key"] = "$2b$10$dwnLF2rKEZIsJ2QzogLaBuZG8BTPBXSwIm8tIoIjpLfyOg9n.T2zm";
            request.Headers["private"] = "false";
            request.Headers["collection-id"] = "5efa1fd57f16b71d48a81da8";
            var CollectionId = request.Headers["collection-id"];
            request.Headers["name"] = "Auto1";
            var binName = request.Headers["name"];

            using (var streamWriter = new System.IO.StreamWriter(request.GetRequestStream(), ASCIIEncoding.ASCII))
            {
                string CreateBinBody = JsonConvert.SerializeObject(new
                {
                    sample = "Hellow World"
                });

                streamWriter.Write(CreateBinBody);
                streamWriter.Flush();
                streamWriter.Close();
            }

            dynamic json;
       
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();

                    json = JsonConvert.DeserializeObject(responseText);
                }
            }
            catch (System.Net.WebException ex)
            {
                using (var reader = new System.IO.StreamReader(ex.Response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();
                    json = JsonConvert.DeserializeObject(responseText);
                }
            }

            Assert.AreEqual(response.StatusCode.ToString(), "OK");
            Assert.IsTrue(json.success.Value);
            Assert.IsFalse(json["private"].Value);
            Assert.AreEqual(json.data.sample.Value, "Hellow World");
            Assert.IsNotNull(json.id.Value);
            Assert.AreEqual(json.collectionID.Value, CollectionId);
            Assert.AreEqual(json.binName.Value, binName);
        }

        [TestMethod]
        public void AddPrivateBintoCollection()
        {
            HttpWebRequest request;
            HttpWebResponse response = null;

            string endPoint = "https://api.jsonbin.io/b";
            request = (HttpWebRequest)WebRequest.Create(endPoint); //create http request 
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            request.Headers["secret-key"] = "$2b$10$dwnLF2rKEZIsJ2QzogLaBuZG8BTPBXSwIm8tIoIjpLfyOg9n.T2zm";
            request.Headers["private"] = "true";
            request.Headers["collection-id"] = "5efa1fd57f16b71d48a81da8";
            var CollectionId = request.Headers["collection-id"];
            request.Headers["name"] = "Auto1";
            var binName = request.Headers["name"];

            using (var streamWriter = new System.IO.StreamWriter(request.GetRequestStream(), ASCIIEncoding.ASCII))
            {
                string CreateBinBody = JsonConvert.SerializeObject(new
                {
                    sample = "Hellow World"
                });

                streamWriter.Write(CreateBinBody);
                streamWriter.Flush();
                streamWriter.Close();
            }

            dynamic json;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();

                    json = JsonConvert.DeserializeObject(responseText);
                }
            }
            catch (System.Net.WebException ex)
            {
                using (var reader = new System.IO.StreamReader(ex.Response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();
                    json = JsonConvert.DeserializeObject(responseText);
                }
            }

            Assert.AreEqual(response.StatusCode.ToString(), "OK");
            Assert.IsTrue(json.success.Value);
            Assert.IsTrue(json["private"].Value);
            Assert.AreEqual(json.data.sample.Value, "Hellow World");
            Assert.IsNotNull(json.id.Value);
            Assert.AreEqual(json.collectionID.Value, CollectionId);
            Assert.AreEqual(json.binName.Value, binName);
        }

        [TestMethod]
        public void AddBinWithInvalidHeaders()
        {
            HttpWebRequest request;
            HttpWebResponse response = null;

            string endPoint = "https://api.jsonbin.io/b";
            request = (HttpWebRequest)WebRequest.Create(endPoint); //create http request 
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";

            using (var streamWriter = new System.IO.StreamWriter(request.GetRequestStream(), ASCIIEncoding.ASCII))
            {
                string CreateBinBody = JsonConvert.SerializeObject(new
                {
                    sample = "Hellow World"
                });

                streamWriter.Write(CreateBinBody);
                streamWriter.Flush();
                streamWriter.Close();
            }

            dynamic json;
           
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();

                    json = JsonConvert.DeserializeObject(responseText);
                }
            }
            catch (System.Net.WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                using (var reader = new System.IO.StreamReader(ex.Response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();
                    json = JsonConvert.DeserializeObject(responseText);                 
           
                }
            }

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);
            Assert.IsFalse(json.success.Value);
            Assert.AreEqual(json.message.Value, "You need to pass a secret-key in the header to Create a Bin");
           
        }



        [TestMethod]
        public void GetPublicBin()
        {
            HttpWebRequest request;
            HttpWebResponse response;
            
            string endPoint = "https://api.jsonbin.io/b/5efcb8cfbb5fbb1d25624675"; // how get id from respons dinamically

            request = (HttpWebRequest)WebRequest.Create(endPoint); //create http request 
            request.Method = "GET";
            
            dynamic json;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();

                    json = JsonConvert.DeserializeObject(responseText);
                }
            }
            catch (System.Net.WebException ex)

            {
                response = (HttpWebResponse)ex.Response;
                using (var reader = new System.IO.StreamReader(ex.Response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                     string responseText = reader.ReadToEnd();
                    json = JsonConvert.DeserializeObject(responseText);
                }
            }
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(json.sample.Value, "Hello World");

        }

        [TestMethod]
        public void GetPrivateBin()
        {
            HttpWebRequest request;
            HttpWebResponse response = null;

            string endPoint = "https://api.jsonbin.io/b/5efcb939bb5fbb1d256246af"; 
            request = (HttpWebRequest)WebRequest.Create(endPoint); //create http request 
            request.Method = "GET";
            request.Headers["secret-key"] = "$2b$10$dwnLF2rKEZIsJ2QzogLaBuZG8BTPBXSwIm8tIoIjpLfyOg9n.T2zm";

            dynamic json;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();

                    json = JsonConvert.DeserializeObject(responseText);
                }
            }
            catch (System.Net.WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                using (var reader = new System.IO.StreamReader(ex.Response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();
                    json = JsonConvert.DeserializeObject(responseText);
                }
            }
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(json.sample.Value, "Hello World");

        }

        [TestMethod]
        public void UpdatePublicBinNoVersioning()
        {
            HttpWebRequest request;
            HttpWebResponse response = null;

            string endPoint = "https://api.jsonbin.io/b/5efcacd4bb5fbb1d25623f04";
            request = (HttpWebRequest)WebRequest.Create(endPoint); //create http request 
            request.Method = "PUT";
            request.ContentType = "application/json; charset=UTF-8";
            request.Headers["secret-key"] = "$2b$10$dwnLF2rKEZIsJ2QzogLaBuZG8BTPBXSwIm8tIoIjpLfyOg9n.T2zm";
            request.Headers["private"] = "true";
            request.Headers["versioning"] = "false";


            using (var streamWriter = new System.IO.StreamWriter(request.GetRequestStream(), ASCIIEncoding.ASCII))
            {
                string CreateBinBody = JsonConvert.SerializeObject(new
                {
                    sample = "NewBin"
                });

                streamWriter.Write(CreateBinBody);
                streamWriter.Flush();
                streamWriter.Close();
            }

            dynamic json;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();

                    json = JsonConvert.DeserializeObject(responseText);
                }
            }
            catch (System.Net.WebException ex)
            {
                using (var reader = new System.IO.StreamReader(ex.Response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();
                    json = JsonConvert.DeserializeObject(responseText);
                }
            }

            Assert.AreEqual(response.StatusCode.ToString(), "OK");
            Assert.IsTrue(json.success.Value);
            Assert.AreEqual(json.data.sample.Value, "NewBin");
            Assert.AreEqual(json.parentId.Value, "5efcacd4bb5fbb1d25623f04");
            Assert.IsNull(json["version"] );
        }

        [TestMethod]
        public void UpdatePublicBinWithVersioning()
        {
            HttpWebRequest request;
            HttpWebResponse response = null;

            string endPoint = "https://api.jsonbin.io/b/5efc4e2a0bab551d2b69eeb1";
            request = (HttpWebRequest)WebRequest.Create(endPoint); //create http request 
            request.Method = "PUT";
            request.ContentType = "application/json; charset=UTF-8";
            request.Headers["secret-key"] = "$2b$10$dwnLF2rKEZIsJ2QzogLaBuZG8BTPBXSwIm8tIoIjpLfyOg9n.T2zm";
            request.Headers["private"] = "true";
            request.Headers["versioning"] = "true";
            var versioning = request.Headers["versioning"];


            using (var streamWriter = new System.IO.StreamWriter(request.GetRequestStream(), ASCIIEncoding.ASCII))
            {
                string CreateBinBody = JsonConvert.SerializeObject(new
                {
                    sample = "NewBin2"
                });

                streamWriter.Write(CreateBinBody);
                streamWriter.Flush();
                streamWriter.Close();
            }

            dynamic json;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();

                    json = JsonConvert.DeserializeObject(responseText);
                }
            }
            catch (System.Net.WebException ex)
            {
                using (var reader = new System.IO.StreamReader(ex.Response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();
                    json = JsonConvert.DeserializeObject(responseText);
                }
            }

            Assert.AreEqual(response.StatusCode.ToString(), "OK");
            Assert.IsTrue(json.success.Value);
            Assert.AreEqual(json.data.sample.Value, "NewBin2");
            Assert.AreEqual(json.parentId.Value, "5efc4e2a0bab551d2b69eeb1");
            Assert.IsNotNull(json.version);
        }


        [TestMethod]
        public void DeleteNewPrivateBin()
        {
            HttpWebRequest request;
            HttpWebResponse response = null;

            string endPoint = "https://api.jsonbin.io/b/5efcb4a77f16b71d48a967f3";
            request = (HttpWebRequest)WebRequest.Create(endPoint); //create http request 
            request.Method = "DELETE";
            request.Headers["secret-key"] = "$2b$10$dwnLF2rKEZIsJ2QzogLaBuZG8BTPBXSwIm8tIoIjpLfyOg9n.T2zm";

            dynamic json;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();

                    json = JsonConvert.DeserializeObject(responseText);
                }
            }
            catch (System.Net.WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
                using (var reader = new System.IO.StreamReader(ex.Response.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    string responseText = reader.ReadToEnd();
                    json = JsonConvert.DeserializeObject(responseText);
                }
            }

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK); 
            Assert.IsTrue(json.success.Value);
            Assert.AreEqual(json.id.Value, "5efcb4a77f16b71d48a967f3");
            Assert.IsTrue(json.message.Value.StartsWith("Bin 5efcb4a77f16b71d48a967f3 is deleted successfully. "));
        }


        [TestCleanup]
        public void Cleanup()
        {
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuinBinAPI
{
    [TestClass]
    public class BinApiTests
    {
        private JsonBinsClient client = new JsonBinsClient("https://api.jsonbin.io/b");
        const string secretKey = "$2b$10$dwnLF2rKEZIsJ2QzogLaBuZG8BTPBXSwIm8tIoIjpLfyOg9n.T2zm";
        const string contentType = "application/json";
        const string defaultBody = "{\"sample\":\"Hello World\"}";
        const string testCollectionId = "5efa1fd57f16b71d48a81da8";


        [TestMethod]
        public void AddPrivateBin()
        {
            // object to string
            string binBody = JsonConvert.SerializeObject(new
            {
                sample = "Hellow World"
            });

            var headers = new Dictionary<string, string>
            {
                { "content-type", contentType },
                { "secret-key", secretKey }
            };

            var response = client.CreateBin(binBody, headers);
            
            Assert.IsTrue(response.Body.success.Value);
            Assert.IsTrue(response.Body["private"].Value);

            Assert.AreEqual(response.Body.data.sample.Value, "Hellow World");
            Assert.IsNotNull(response.Body.id.Value);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void AddPublicBin()
        {
            string binBody = JsonConvert.SerializeObject(new
            {
                sample = "Hello World"
            });

            var headers = new Dictionary<string, string>
            {
                { "content-type", contentType },
                { "secret-key", secretKey },
                { "private", "false"}
            };

            var response = client.CreateBin(binBody, headers);

            Assert.IsTrue(response.Body.success.Value);
            Assert.IsFalse(response.Body["private"].Value);
            Assert.AreEqual(response.Body.data.sample.Value, "Hello World");
            Assert.IsNotNull(response.Body.id.Value);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }
        [TestMethod]
        public void GetPrivateBin()
        {
            //create bin
            var createBinResponse = client.CreateBin(defaultBody, new Dictionary<string, string>
            {
                { "secret-key", secretKey },
                {"content-type", contentType }
            });
            string createdBinId = createBinResponse.Body.id.Value;
            
            var headers = new Dictionary<string, string>
            {
                { "secret-key", secretKey }
            };

            var response = client.ReadBin(createdBinId, headers);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(response.Body.sample.Value, "Hello World");

        }

        [TestMethod]
        public void UpdatePrivateBinNoVersioning()
        {
            //create bin
            var createBinResponse = client.CreateBin(defaultBody, new Dictionary<string, string>
            {
                { "secret-key", secretKey },
                {"content-type", contentType }
            });
            string createdBinId = createBinResponse.Body.id.Value;
            //update bin
            var headers = new Dictionary<string, string>
            {
                { "secret-key", secretKey },
                { "private", "true" },
                { "versioning", "false" },
                { "content-type", contentType }

            };

            string binBody = JsonConvert.SerializeObject(new
            {
                sample = "NewBin"
            });

            var response = client.UpdateBin(createdBinId, binBody, headers);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(response.Body.success.Value);
            Assert.AreEqual(response.Body.data.sample.Value, "NewBin");
            Assert.AreEqual(response.Body.parentId.Value, createdBinId); 
            Assert.IsNull(response.Body["version"]);
        }

        [TestMethod]
        public void UpdatePublicBinNoVersioning()
        {
            //create public bin
            var createBinResponse = client.CreateBin(defaultBody, new Dictionary<string, string>
            {
                { "secret-key", secretKey },
                {"content-type", contentType },
                { "private", "false" }
            });
            string createdBinId = createBinResponse.Body.id.Value;
            //update bin
            var headers = new Dictionary<string, string>
            {
                { "secret-key", secretKey },
                { "private", "false" },
                { "versioning", "false" },
                { "content-type", contentType }

            };

            string binBody = JsonConvert.SerializeObject(new
            {
                sample = "NewBinPublic"
            });

            var response = client.UpdateBin(createdBinId, binBody, headers);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(response.Body.success.Value);
            Assert.AreEqual(response.Body.data.sample.Value, "NewBinPublic");
            Assert.AreEqual(response.Body.parentId.Value, createdBinId);
            Assert.IsNotNull(response.Body["version"]);
            Assert.IsTrue(response.Body["version"].Value > 0);
        }

        [TestMethod]
        public void DeleteNewPrivateBin()
        {
           var createBinResponse = client.CreateBin(defaultBody, new Dictionary<string, string>
            {
                { "secret-key", secretKey },
                {"content-type", contentType },
                { "private", "true"}
            });
            string createdBinId = createBinResponse.Body.id.Value;

            //delete created bin
            var headers = new Dictionary<string, string>
            {
                { "secret-key", secretKey }
            };

            var response = client.DeleteBin(createdBinId, headers);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(response.Body.success.Value);
            Assert.AreEqual(response.Body.id.Value, createdBinId);
            Assert.IsTrue(response.Body.message.Value.StartsWith($"Bin {createdBinId} is deleted successfully. "));
        }

        [TestMethod]
        public void DeleteNewPublicBin()
        {
            var createBinResponse = client.CreateBin(defaultBody, new Dictionary<string, string>
            {
                { "secret-key", secretKey },
                {"content-type", contentType },

            });
            string createdBinId = createBinResponse.Body.id.Value;

            //delete created bin
            var headers = new Dictionary<string, string>
            {
                { "secret-key", secretKey }
            };

            var response = client.DeleteBin(createdBinId, headers);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(response.Body.success.Value);
            Assert.AreEqual(response.Body.id.Value, createdBinId);
            Assert.IsTrue(response.Body.message.Value.StartsWith($"Bin {createdBinId} is deleted successfully. "));
        }


        [TestMethod]
        public void AddPublicBintoCollection()

        {
            
            var headers = new Dictionary<string, string>
            {
                { "content-type", contentType },
                { "secret-key", secretKey },
                { "private", "false"},
                { "collection-id", testCollectionId},
                {"name","Auto1" }
            };
            string binBody = JsonConvert.SerializeObject(new
            {
                sample = "Hello World"
            });

            var response = client.CreateBin(binBody, headers);

            Assert.AreEqual(response.StatusCode.ToString(), "OK");
            Assert.IsTrue(response.Body.success.Value);
            Assert.IsFalse(response.Body["private"].Value); // [] as privat service word
            Assert.AreEqual(response.Body.data.sample.Value, "Hello World");
            Assert.IsNotNull(response.Body.id.Value);
            Assert.AreEqual(response.Body.collectionID.Value, testCollectionId);
            Assert.AreEqual(response.Body.binName.Value, "Auto1");
        }

        [TestMethod]
        public void AddPrivateBintoCollection()

        {
            var headers = new Dictionary<string, string>
            {
                { "content-type", contentType },
                { "secret-key", secretKey },
                { "private", "true"},
                { "collection-id", testCollectionId},
                {"name","Auto1" }
            };
            string binBody = JsonConvert.SerializeObject(new
            {
                sample = "Hello World"
            });

            var response = client.CreateBin(binBody, headers);

            Assert.AreEqual(response.StatusCode.ToString(), "OK");
            Assert.IsTrue(response.Body.success.Value);
            Assert.IsTrue(response.Body["private"].Value); // [] as privat service word
            Assert.AreEqual(response.Body.data.sample.Value, "Hello World");
            Assert.IsNotNull(response.Body.id.Value);
            Assert.AreEqual(response.Body.collectionID.Value, testCollectionId);
            Assert.AreEqual(response.Body.binName.Value, "Auto1");
            
        }

        [TestMethod]
        public void AddBinWithEmptyHeaderSecurity()
        {
            var headers = new Dictionary<string, string>
            {

            {"content-type", contentType }

            };
            string binBody = JsonConvert.SerializeObject(new
            {
                sample = "Hello World"
            });

            var response = client.CreateBin(binBody, headers);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);
            Assert.AreEqual(response.Body.message.Value, "You need to pass a secret-key in the header to Create a Bin");
                       
        }

        [TestMethod]
        public void AddBinWithEmptyHeaderContentType()
        {
            var headers = new Dictionary<string, string>
            {

             { "secret-key", secretKey }

            };
            string binBody = JsonConvert.SerializeObject(new
            {
                sample = "Hello World"
            });

            var response = client.CreateBin(binBody, headers);

            Assert.AreEqual(response.StatusCode, (HttpStatusCode)422);
            Assert.AreEqual(response.Body.message.Value, "Expected content type - application/json");

        }

        [TestMethod]
        public void AddBinWithInvalidHeaderCollectionId()
        {
            var headers = new Dictionary<string, string>
            {

             { "secret-key", secretKey },
             {"content-type", contentType },
             { "collection-id", "invalida1fd57f16b71d48a81da7"},
             {"name","Auto1" }

            };
            string binBody = JsonConvert.SerializeObject(new
            {
                sample = "Hello World"
            });

            var response = client.CreateBin(binBody, headers);

            Assert.AreEqual(response.StatusCode, (HttpStatusCode)422);
            Assert.AreEqual(response.Body.message.Value, "Invalid Collection ID");

        }

        [TestMethod]
        public void AddBinWithInvalidSecretKey()
        {
            var headers = new Dictionary<string, string>
            {

             { "secret-key", "InvalidTest" },
             {"content-type", contentType },
             { "collection-id", testCollectionId},
             {"name","Auto1" }

            };
            string binBody = JsonConvert.SerializeObject(new
            {
                sample = "Hello World"
            });

            var response = client.CreateBin(binBody, headers);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);
            Assert.AreEqual(response.Body.message.Value, "Invalid secret key provided.");

        }


        [TestMethod]
        public void GetPublicBin()
        {
            //create bin
            var createBinResponse = client.CreateBin(defaultBody, new Dictionary<string, string>
            {
                { "secret-key", secretKey },
                {"content-type", contentType },
                { "private", "false"}

            });
            string createdBinId = createBinResponse.Body.id.Value;

            var headers = new Dictionary<string, string>
            {
                {"name", "TestName" }
            };

            var response = client.ReadBin(createdBinId, headers);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(response.Body.sample.Value, "Hello World");

            }

        [TestMethod]
        public void UpdatePrivateBinWithVersioning()
        {
            //create public bin
            var createBinResponse = client.CreateBin(defaultBody, new Dictionary<string, string>
            {
                { "secret-key", secretKey },
                {"content-type", contentType },
                { "private", "true" }
            });
            string createdBinId = createBinResponse.Body.id.Value;
            //update bin
            var headers = new Dictionary<string, string>
            {
                { "secret-key", secretKey },
                { "private", "true" },
                { "versioning", "true" },
                { "content-type", contentType }

            };


            string binBody = JsonConvert.SerializeObject(new
            {
                sample = "NewBinPublic2"
            });

            var response = client.UpdateBin(createdBinId, binBody, headers);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsTrue(response.Body.success.Value);
            Assert.AreEqual(response.Body.data.sample.Value, "NewBinPublic2");
            Assert.AreEqual(response.Body.parentId.Value, createdBinId);
            Assert.IsNotNull(response.Body["version"]);
            Assert.IsTrue(response.Body["version"].Value > 0);
            
        }

        [TestCleanup]
        public void Cleanup()
        {
        }
    }
}

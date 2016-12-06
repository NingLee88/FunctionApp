using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp
{
    public class JsonFileChecker
    {
        public static string CheckFile()
        {
            try {
                List<OutPutModel> outPutList = new List<OutPutModel>();
                OutPutModel model;
                ListPrices listPrice = getListPrice();

                CloudBlobClient blobClient = new CloudBlobClient(new Uri(@"https://globalconnectioncenter.blob.core.windows.net"), new StorageCredentials("globalconnectioncenter", ""));

                //Get a reference to the container.
                CloudBlobContainer container = blobClient.GetContainerReference("pricemapingfiles");

                //List blobs and directories in this container
                var blobs = container.ListBlobs();

                foreach (var blobItem in blobs)
                {
                    string[] array = readBlobFile(blobItem.Uri).Split('\r', '\n');
                    foreach (string line in array)
                    {
                        bool isFind = true;
                        var fields = line.Split('\t');
                        if (fields.Length < 5 || string.IsNullOrEmpty(fields[1]))
                        {
                            continue;
                        }
                        model = getOutPutModel(fields);
                        try
                        {
                            Service service = listPrice.Services[model.serviceName];
                            if (service == null || string.IsNullOrEmpty(service.Name))
                            {
                                isFind = false;
                            }
                            Type type = service.Types.Find(t => t.Name.Equals(model.typeName));
                            if (type == null || string.IsNullOrEmpty(type.Name))
                            {
                                isFind = false;
                            }
                            Feature feature = type.Features.Find(f => f.Name.Equals(model.featureName));
                            if (feature == null || string.IsNullOrEmpty(feature.Name))
                            {
                                isFind = false;
                            }
                            Price price = feature.Sizes.Find(s => s.Name.Equals(model.sizeName));
                            if (price == null || string.IsNullOrEmpty(price.Name))
                            {
                                isFind = false;
                            }
                            if (!isFind)
                            {
                                outPutList.Add(model);
                            }
                        }
                        catch (Exception ex)
                        {
                            outPutList.Add(model);
                        }
                    }
                }
                if (outPutList.Count > 0)
                {
                    sendEmail(outPutList);
                    return string.Format("There are {0} MetersID can't be fined!The details is send to email!",outPutList.Count);
                }
                return "All MetersID is find in the json files!";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private static ListPrices getListPrice()
        {
            using (var webClient = new WebClient { Encoding = Encoding.UTF8 })
            {
                string content = webClient.DownloadString(new Uri(@"https://globalconnectioncenter.blob.core.windows.net/pricelist/azurechinapricelatest.json"));
                return JsonConvert.DeserializeObject<ListPrices>(content);
            }
        }

        private static string readBlobFile(Uri uri)
        {
            using (var webClient = new WebClient { Encoding = Encoding.UTF8 })
            {
                string content = webClient.DownloadString(uri);
                return content;
            }
        }
        
        private static void sendEmail(List<OutPutModel> outPutList)
        {
            StringBuilder sb = new StringBuilder();
            foreach (OutPutModel m in outPutList)
            {
                sb.Append(m.MeterID + ":" + "not find!");
                sb.Append("\r\n");
            }
            Email mail = new Email("happy366days@outlook.com", "");
            mail.SendMail("109880678@qq.com", "Meters Not Find", sb.ToString());
        }

        private static OutPutModel getOutPutModel(string[] fields)
        {
            OutPutModel model = new OutPutModel();
            model.MeterID = fields[0];
            model.serviceName = fields[1];
            model.typeName = fields[2];
            model.featureName = fields[3];
            model.sizeName = fields[4];
            return model;
        }
    }
}

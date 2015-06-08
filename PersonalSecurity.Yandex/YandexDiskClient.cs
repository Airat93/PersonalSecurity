namespace PersonalSecurity.Yandex
{
    using System;
    using System.IO;
    using System.Net;
    using System.Web;
    using PersonalSecurity.DataAccess.Domain;
    using PersonalSecurity.Yandex.Model;
    using PersonalSecurity.Yandex.Utils;
    using Newtonsoft.Json;
    using FileInfo = PersonalSecurity.DataAccess.Domain.FileInfo;

    public class YandexDisk : ICloudApi
    {
        private readonly string _accessToken;

        private static readonly string YandexDiskPath = "test";

        public YandexDisk(string accessToken)
        {
            _accessToken = accessToken;
        }

        public void UploadFile(FileInfo fileInfo, Stream stream, EventHandler<CloudEventArgs> completeCallback)
        {
            if (!IsExistsFolder(fileInfo.FileType))
            {
                var response = CreateFolder(fileInfo.FileType);
                if (response.Error != null)
                {
                    completeCallback.Invoke(this, new CloudEventArgs { ErrorMessage = response.Error });
                }
            }

            var uploadResponse = GetResponseModel(fileInfo.FileType, fileInfo.Name, Method.Upload);

            var fileStream = stream;
            var request = HttpUtilities.CreateRequest(uploadResponse.Href, _accessToken);
            request.Method = uploadResponse.Method;
            request.AllowWriteStreamBuffering = false;
            request.SendChunked = true;
            try
            {
                request.BeginGetRequestStream(
                    getRequestStreamResult =>
                        {
                            var getRequestStreamRequest = (HttpWebRequest)getRequestStreamResult.AsyncState;
                            using (var requestStream = getRequestStreamRequest.EndGetRequestStream(getRequestStreamResult))
                            {
                                const int BufferLength = 4096;
                                var buffer = new byte[BufferLength];
                                var count = fileStream.Read(buffer, 0, BufferLength);
                                while (count > 0)
                                {
                                    requestStream.Write(buffer, 0, count);
                                    count = fileStream.Read(buffer, 0, BufferLength);
                                }

                                fileStream.Dispose();
                            }

                            getRequestStreamRequest.BeginGetResponse(
                                getResponseResult =>
                                {
                                    var getResponseRequest = (HttpWebRequest)getResponseResult.AsyncState;
                                    try
                                    {
                                        using (getResponseRequest.EndGetResponse(getResponseResult))
                                        {
                                            completeCallback.Invoke(this, new CloudEventArgs { FileInfo = fileInfo });
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        completeCallback.Invoke(this, new CloudEventArgs { ErrorMessage = ex.Message });
                                        throw new Exception(ex.Message);
                                    }
                                },
                                getRequestStreamRequest);
                        }, request);
            }
            catch (Exception ex)
            {
                completeCallback.Invoke(this, new CloudEventArgs { ErrorMessage = ex.Message });
            }
        }

        public Stream DownloadFile(string folder, string fileName)
        {
            var uploadResponse = GetResponseModel(folder, fileName, Method.Download);
            var request = HttpUtilities.CreateRequest(uploadResponse.Href, _accessToken);
            request.Method = "GET";
            return request.GetResponse().GetResponseStream();
        }

        private ResponseModel CreateFolder(string folder)
        {
            var url = HttpUtility.UrlEncode(YandexDiskPath + "/" + folder);
            var req = HttpUtilities.CreateRequest(
                "https://cloud-api.yandex.net/v1/disk/resources/?path=" + url,
                _accessToken);
            req.Method = "PUT";

            try
            {
                using (var stream = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    return JsonConvert.DeserializeObject<ResponseModel>(stream.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel { Error = ex.Message };
            }
        }

        private bool IsExistsFolder(string folder)
        {
            var url = HttpUtility.UrlEncode(YandexDiskPath + "/" + folder);
            var req = HttpUtilities.CreateRequest(
                "https://cloud-api.yandex.net/v1/disk/resources/?path=" + url,
                _accessToken);
            req.Method = "GET";

            try
            {
                using (req.GetResponse().GetResponseStream())
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public CloudType Cloud
        {
            get
            {
                return CloudType.Yandex;
            }
        }

        private ResponseModel GetResponseModel(string folder, string fileName, string method)
        {
            var url = HttpUtility.UrlEncode(YandexDiskPath + "/" + folder + "/" + fileName);
            var req = HttpUtilities.CreateRequest(
                "https://cloud-api.yandex.net/v1/disk/resources/" + method + "?path=" + url + (method == Method.Upload ? "&overwrite=true" : ""),
                _accessToken);
            req.Method = "GET";

            try
            {
                using (var stream = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    return JsonConvert.DeserializeObject<ResponseModel>(stream.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                return new ResponseModel { Error = ex.Message };
            }
        }
    }

    public class Method
    {
        public const string Upload = "upload";
        public const string Download = "download";
    }
}

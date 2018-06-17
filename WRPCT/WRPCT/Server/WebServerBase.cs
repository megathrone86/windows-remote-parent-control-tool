using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WRPCT.Server
{
    public abstract class WebServerBase : IDisposable
    {
        TcpListener listener;
        bool working;
        Task listenerTask;

        public WebServerBase()
        {
        }

        public void Start(int port)
        {
            Stop();

            listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
            listener.Start();

            listenerTask = new Task(async () =>
            {
                while (working)
                {
                    var client = await listener.AcceptTcpClientAsync();
                    ProcessClient(client);
                }
            });
            working = true;
            listenerTask.Start();
        }

        public void Stop()
        {
            working = false;
            listenerTask?.Wait();
            listener?.Stop();
        }

        public void Dispose()
        {
            working = false;
            listener?.Stop();
        }

        string GetRequestString(TcpClient client)
        {
            string requestString = "";
            byte[] receiveBuffer = new byte[1024];

            int Count;
            while ((Count = client.GetStream().Read(receiveBuffer, 0, receiveBuffer.Length)) > 0)
            {
                requestString += Encoding.ASCII.GetString(receiveBuffer, 0, Count);
                if (requestString.ToString().IndexOf("\r\n\r\n") >= 0 || requestString.Length > 4096)
                {
                    break;
                }
            }
            return requestString;
        }

        void ProcessClient(TcpClient client)
        {
            var requestString = GetRequestString(client);
            var result = OnRequest(requestString);
            var sendBuffer = Encoding.UTF8.GetBytes(result);
            client.GetStream().Write(sendBuffer, 0, sendBuffer.Length);
            client.Close();
        }

        protected string HtmlResult(string html, HttpStatusCode code = HttpStatusCode.OK)
        {
            var htmlLength = Encoding.UTF8.GetByteCount(html);
            return $"HTTP/1.1 {(int)code} {code}\nContent-type: text/html; charset=utf-8\nContent-Length:{htmlLength}\n\n" + html;
        }

        protected string FileResult(string fileContent, HttpStatusCode code = HttpStatusCode.OK)
        {
            var fileLength = Encoding.UTF8.GetByteCount(fileContent);
            return $"HTTP/1.1 {(int)code} {code}\nContent-type: text; charset=utf-8\nContent-Length:{fileLength}\n\n" + fileContent;
        }

        protected string RedirectResult(string location)
        {
            return $"HTTP/1.1 {(int)HttpStatusCode.MovedPermanently} {HttpStatusCode.MovedPermanently}\nLocation: {location}\n\n";
        }

        protected abstract string OnRequest(string requestString);

        protected Dictionary<string, string> GetParametersFromPost(string requestString)
        {
            var n = requestString.IndexOf("\r\n\r\n");
            if (n < 0)
                n = requestString.IndexOf("\n\n");
            if (n < 0)
                return new Dictionary<string, string>();

            var paramsString = requestString.Substring(n).Trim();
            var result = new Dictionary<string, string>();
            foreach (var paramPairString in paramsString.Split(new char[] { '&' }))
            {
                var ts = paramPairString.Split(new char[] { '=' });
                if (!string.IsNullOrEmpty(ts[0]))
                {
                    result[ts[0]] = string.IsNullOrEmpty(ts[1]) ? "" : ts[1];
                }
            }
            return result;
        }
    }
}

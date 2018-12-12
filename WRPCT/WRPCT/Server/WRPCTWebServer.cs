using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WRPCT.Helpers;
using WRPCT.Services;
using static WRPCT.Helpers.ConfigHelper;

namespace WRPCT.Server
{
    public class WRPCTWebServer : WebServerBase
    {
        Config _params = ConfigHelper.Params;

        public WRPCTWebServer(int port) : base(port)
        {
        }

        protected override string OnRequest(string requestString)
        {
            try
            {
                var requestStringHeader = requestString.Substring(0, Math.Min(100, requestString.Length)).ToUpper();

                //index
                if (requestStringHeader.StartsWith("GET / "))
                    return HtmlResult(GetIndexContent());

                //additional files
                if (requestStringHeader.StartsWith("GET /"))
                {
                    var ts = requestString.Split(new char[] { ' ' });
                    var path = ts[1].Substring(1);
                    return CreateFileResult(path);
                }

                //commands
                if (requestStringHeader.StartsWith("POST /DISALLOWGAMES "))
                {
                    MainService.Instance.ControlService.DisableGames();
                    return RedirectResult("/");
                }
                if (requestStringHeader.StartsWith("POST /ALLOWGAMES "))
                {
                    MainService.Instance.ControlService.EnableGames();
                    return RedirectResult("/");
                }
                if (requestStringHeader.StartsWith("POST /UPDATE "))
                {
                    var requestParams = GetParametersFromPost(requestString);
                    MainService.Instance.ControlService.UpdateTimeCounter(int.Parse(requestParams["amount"]));
                    return RedirectResult("/");
                }
                if (requestStringHeader.StartsWith("POST /GETPROCESSES "))
                {
                    var processes = ProcessesHelper.GetProcesses();
                    return JsonResult(processes.Select(t =>
                        new { name = t.FileName, path = t.FilePath, user = t.UserName }
                    ).ToList());
                }

                //default
                return HtmlResult("", HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return HtmlResult(ex.ToString(), HttpStatusCode.InternalServerError);
            }
        }

        string CreateFileResult(string path)
        {
            var fileContent = ResourceHelper.GetStringFromEmdeddedResource($"WRPCT.Server.{path}");
            if (fileContent == null)
                return HtmlResult("", HttpStatusCode.NotFound);
            return FileResult(fileContent);
        }

        string GetIndexContent()
        {
            string systemName = "TEST SYSTEM";
            string projectUrl = "https://github.com/megathrone86/windows-remote-parent-control-tool";
            string gamesStatus = _params.AllowGames ? "Игры разрешены" : "Игры запрещены";
            string gamesTimeLeft = _params.GamesTimeLeft.ToString();

            var template = ResourceHelper.GetStringFromEmdeddedResource("WRPCT.Server.Index.html");
            template = template.Replace("{%%systemName%%}", systemName);
            template = template.Replace("{%%currentTime%%}", DateTime.Now.ToString());
            template = template.Replace("{%%gamesStatus%%}", gamesStatus);
            template = template.Replace("{%%projectUrl%%}", projectUrl);
            template = template.Replace("{%%gamesTimeLeft%%}", gamesTimeLeft);

            return template;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WRPCT.Helpers;
using WRPCT.Services;

namespace WRPCT.Server
{
    public class WRPCTWebServer : WebServerBase
    {
        protected override string OnRequest(string requestString)
        {
            try
            {
                var requestStringHeader = requestString.Substring(0, Math.Min(100, requestString.Length)).ToUpper();
                if (requestStringHeader.StartsWith("GET / "))
                    return HtmlResult(GetIndexContent());

                if (requestStringHeader.StartsWith("GET /"))
                    return RedirectResult("/");

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
                    MainService.Instance.ControlService.UpdateTimeCounter(int.Parse(requestParams["amount"]), requestParams.ContainsKey("add"));
                    return RedirectResult("/");
                }

                return HtmlResult("", HttpStatusCode.NotFound);
            } catch (Exception ex)
            {
                return HtmlResult(ex.ToString(), HttpStatusCode.InternalServerError);
            }
        }

        string GetIndexContent()
        {
            string systemName = "TEST SYSTEM";
            string projectUrl = "https://github.com/megathrone86/windows-remote-parent-control-tool";
            string gamesStatus = MainService.Instance.ControlService.AllowGames ? "Игры разрешены" : "Игры запрещены";
            string gamesTimeLeft = MainService.Instance.ControlService.GamesTimeLeft.ToString();

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
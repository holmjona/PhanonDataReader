using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;

namespace PhanonDataReader.Controllers {
    public class GetController : Controller {
        string appBaseUrl = "http://phanon.herokuapp.com/";
        string signiUrl = "users/sign_in";
        string moduleUrl = "api/phanon_modules?context_id={course_id}&course_id={course_id}";
        string progressMapUrl = "api/progress_maps/{course_id}?context_id={course_id}&per_page=200&page=0";
        string studentAnsUrl = "api/user_lessons/{lesson_id}?context_id={course_id}&student_id={student_id}";
        string viewAnsUrl = "#/course/{course_id}/user_lesson_review/{lesson_id}/user/{student_id}";
        string studentProjUrl = "api/submissions?context_id={course_id}&project_id={project_id}&student_id={student_id}";
        string viewProjUrl = "#/course/{course_id}/submissions/{project_id}/student/{student_id}";
        Dictionary<string, string> cookies = new Dictionary<string, string>();
        public async Task<IActionResult> Index() {
            string body = "";
            try {
                //WebRequest wr = WebRequest.Create(appBaseUrl);
                //WebResponse resp = wr.GetResponse();
                string url = "api/progress_maps/179?per_page=50&page=0";
            // https://docs.microsoft.com/en-us/dotnet/api/system.net.networkcredential?view=net-5.0
                Uri baseAddy = new Uri(appBaseUrl);
                HttpClientHandler cHandler = new HttpClientHandler();
                
                NetworkCredential nc = new NetworkCredential("phanon-test@jdholmes.net", "yYpjjN5z5AgAVnhGNsq59YEk2KMLirjqW9T3CrMgNShh");

                CredentialCache cc = new CredentialCache();
                cc.Add(new Uri(appBaseUrl), "Basic", nc);
                WebRequest wr = WebRequest.Create(appBaseUrl);
                wr.Credentials = cc;

                HttpWebResponse hwr = (HttpWebResponse)wr.GetResponse();
                System.Diagnostics.Debug.WriteLine(hwr.StatusDescription);
                Stream str = hwr.GetResponseStream();
                StreamReader strRead = new StreamReader(str);
                string strAnswer = strRead.ReadToEnd();
                System.Diagnostics.Debug.WriteLine(strAnswer);
                strRead.Close();
                str.Close();
                hwr.Close();

                cHandler.UseCookies = true;
                cHandler.Credentials = nc;
                
                using (HttpClient httpC = new HttpClient(cHandler)) {
                    using (HttpResponseMessage resp = await httpC.GetAsync(appBaseUrl + "/" + url)) {
                        if (resp.StatusCode == HttpStatusCode.Unauthorized) {
                            body = "boo";
                        }
                        if (resp.StatusCode == HttpStatusCode.OK) {
                            body = await resp.Content.ReadAsStringAsync();

                        }
                    }
                }

                HttpClientHandler clientHandler = new HttpClientHandler() { UseCookies = true };
                HttpClient client = new HttpClient(clientHandler) { BaseAddress = baseAddy };
                HttpResponseMessage respInit = await client.GetAsync(url);
                var heads = respInit.Headers;
                var cooks = heads.GetValues("Cookie");
            } catch (Exception ex) {
                body = "Oops: " + ex.Message;
            }


            return View(body);

        }
    }
}

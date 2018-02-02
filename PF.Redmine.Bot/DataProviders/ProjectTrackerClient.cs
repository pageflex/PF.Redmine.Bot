using PF.Redmine.Bot.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace PF.Redmine.Bot.DataProviders
{
    class ProjectTrackerClient
    {
        IConfiguration _configuration;
        public string ApiKey { get; private set; }
        public ProjectTrackerClient(string apiKey, IConfiguration configuration)
        {
            _configuration = configuration ?? throw new Exception("Configuration Not Found");
            ApiKey = apiKey ?? throw new Exception("No API Key given");
        }
        public Issue GetIssue(int IssueNum)
        {
            var issueUri = String.Format("/issues/{0}.json", IssueNum);

            var respStr = GetResponse(issueUri);

            using (var rdr = new StringReader(respStr))
            using (var jReader = new JsonTextReader(rdr))
            {
                var jObj = JObject.ReadFrom(jReader);

                var i = jObj["issue"];

                var convIssue = JsonConvert.DeserializeObject<Issue>(i.ToString());

                return convIssue;
            }
        }

        public Issues GetPagedIssuesForProject(int ProjectId, int offset, int limit)
        {
            var issuesUri = String.Format("/issues.json?project_id={0}&status_id=open&offset={1}&limit={2}&sort=updated_on:desc", ProjectId, offset, limit);

            var respStr = GetResponse(issuesUri);

            var convIssues = JsonConvert.DeserializeObject<Issues>(respStr);

            return convIssues;
        }

        public Issues GetPagedIssuesForUser(int UserId, int offset, int limit)
        {
            var issuesUri = String.Format("/issues.json?assigned_to_id={0}&offset={1}&limit={2}&sort=updated_on:desc", UserId, offset, limit);

            var respStr = GetResponse(issuesUri);

            var convIssues = JsonConvert.DeserializeObject<Issues>(respStr);

            return convIssues;
        }

        public List<DisplayProject> GetAllProjects()
        {
            var firstPage = GetPagedProjects(0, 100);

            List<DisplayProject> allProjects = (
                from project in firstPage.projects
                select new DisplayProject { id = project.id, name = project.name }).ToList<DisplayProject>();

            var total = firstPage.total_count;

            for (var i = firstPage.offset + 100; i <= total; i = i + 100)
            {
                var page = GetPagedProjects(i, 100);

                if (page.projects != null && page.projects.Count > 0)
                {
                    allProjects.AddRange(
                        (from project in firstPage.projects
                         select new DisplayProject { id = project.id, name = project.name }));
                }
            }

            return allProjects;
        }

        public Projects GetPagedProjects(int offset, int limit)
        {
            var projectUri = String.Format("/projects.json?offset={0}&limit={1}", offset, limit);

            var respStr = GetResponse(projectUri);

            var convProjects = JsonConvert.DeserializeObject<Projects>(respStr);

            return convProjects;
        }

        public Users GetPagedUsers(int offset, int limit)
        {
            var usersUri = String.Format("/users.json?offset={0}&limit={1}", offset, limit);

            var respStr = GetResponse(usersUri);

            var convUsers = JsonConvert.DeserializeObject<Users>(respStr);

            return convUsers;
        }

        public List<DisplayUser> GetAllUsers()
        {
            var firstPage = GetPagedUsers(0, 100);
            var total_count = firstPage.total_count;

            List<DisplayUser> allUsers = (from user in firstPage.users
                                          select new DisplayUser() { id = user.id, login = user.login }).ToList<DisplayUser>();

            for (var i = firstPage.offset + 100; i <= total_count; i = i + 100)
            {
                var page = GetPagedUsers(i, 100);

                if (page.users != null && page.users.Count > 0)
                {
                    allUsers.AddRange(
                        (from user in page.users
                         select new DisplayUser() { id = user.id, login = user.login, name = String.Format("{0} {1}", user.firstname, user.lastname) }
                        ));
                }
            }

            return allUsers;
        }

        #region Private Methods

        public string GetResponse(string requestUri)
        {

            string ret = "";

            var request = System.Net.WebRequest.Create(String.Format("{0}/{1}", GetApiUrl().TrimEnd('/'), requestUri.TrimStart('/')));
            request.ContentType = "application/json";
            request.Headers.Add("X-Redmine-API-Key", GetApiKey());

            var resp = request.GetResponse();

            try
            {

                if (((HttpWebResponse)resp).StatusCode == HttpStatusCode.OK)
                {
                    using (Stream dataStream = resp.GetResponseStream())
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        ret = reader.ReadToEnd();
                        reader.Close();
                        dataStream.Close();
                    }
                }
                else
                {
                    var respex = (HttpWebResponse)resp;

                    throw new Exception(String.Format("Response Exception: {0} | {1}", respex.StatusCode, respex.StatusDescription));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                resp.Close();
            }


            return ret;

        }

        private string GetApiUrl()
        {
            return _configuration["PtUrl"];
        }

        private string GetApiKey()
        {
            //return ConfigurationManager.AppSettings["PtKey"];
            return this.ApiKey;
        }

        #endregion Private Methods

    }
}
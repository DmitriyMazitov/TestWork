using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Npgsql;
using NpgsqlTypes;
using HtmlAgilityPack;

namespace ThirdVariant.DAL
{
    public class GetAndStoreContent
    {
        protected class Content
        {
            internal long VacancyId { get; set; }
            internal string VacancyContent { get; set; }
        }

        protected bool IsDownlodContent = false;
        protected bool IsStoreToDb = false;
        protected List<Content> DownloadedContents;

        protected string UriString;
        protected string PgConnString;
        protected string HhApiUrl;
        protected string VacancyUrl = "";
        protected StringBuilder VacancyApiUrlId = new StringBuilder();
        public GetAndStoreContent(string uriString, string hhApiUrl, string pgConnString)
        {
            UriString = uriString;
            HhApiUrl = hhApiUrl;
            PgConnString = pgConnString;
            DownloadedContents = GetVacancyContent();
        }

        public bool StartProcess()
        {
            if (!ReferenceEquals(DownloadedContents, null) && DownloadedContents.Count != 0)
            {
                IsDownlodContent = true;
                IsStoreToDb = StoreContentToDb();
                if (IsStoreToDb)
                {
                    IsStoreToDb = true;
                    return true;
                }
            }
            else
            {
                IsDownlodContent = false;
                return false;
            }

            return false;
        }

        private List<Content> GetVacancyContent()
        {
            try
            {
                List<Content> downloadedContents = new List<Content>();
                using (var webClient = new WebClient())
                {
                    using (Stream webStream = webClient.OpenRead(UriString))
                    {
                        using (StreamReader sr = new StreamReader(webStream))
                        {
                            HtmlDocument htmlDoc = new HtmlDocument();
                            htmlDoc.Load(sr);

                            List<HtmlNode> divHhNodes = htmlDoc.DocumentNode.Descendants().Where(x =>
                                x.Name == "div" && x.Attributes["class"] != null
                                && x.Attributes["class"].Value.Contains("search-result-item")).ToList();
                            if (divHhNodes.Count != 0)
                            {
                                foreach (var divHhNode in divHhNodes)
                                {
                                    var vacUrl = divHhNode.Descendants().Where(x =>
                                        x.Name == "a" && x.Attributes["class"] != null
                                        && x.Attributes["class"].Value
                                            .Contains("search-result-item__name")).ToList();

                                    foreach (var url in vacUrl)
                                    {
                                        if (!VacancyUrl.Contains(url.Attributes["href"].Value))
                                        {
                                            VacancyUrl = url.Attributes["href"].Value;
                                            var vacancyId = url.Attributes["href"].Value;

                                            int aa = VacancyUrl.LastIndexOf("/", StringComparison.Ordinal) + 1;
                                            int uriLength = VacancyUrl.Length;
                                            int numberLength = uriLength - aa;
                                            string id = vacancyId.Substring(
                                                vacancyId.LastIndexOf("/", StringComparison.Ordinal) + 1, numberLength);

                                            VacancyApiUrlId.Append(HhApiUrl);
                                            VacancyApiUrlId.Append(vacancyId.Substring(
                                                vacancyId.LastIndexOf("/", StringComparison.Ordinal) + 1,
                                                numberLength));

                                            using (WebClient webApiClient = new WebClient())
                                            {
                                                webApiClient.Headers[HttpRequestHeader.UserAgent] = "HhC#Api/1.0 (mazitovdv@mail.ru)";
                                                webApiClient.Headers[HttpRequestHeader.Accept] = "application/json";
                                                string htmlResult = webApiClient.DownloadString(VacancyApiUrlId.ToString());

                                                downloadedContents.Add(new Content
                                                {
                                                    VacancyId = Convert.ToInt64(id),
                                                    VacancyContent = htmlResult
                                                });
                                            }
                                            VacancyApiUrlId.Clear();
                                        }
                                    }
                                }
                            }

                            divHhNodes.Clear();
                            return downloadedContents;
                        }
                    }
                }
            }
            catch (HtmlWebException we)
            {
                Console.WriteLine(we);
                return null;
            }
            catch (JsonException je)
            {
                Console.WriteLine(je);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private bool StoreContentToDb()
        {
            try
            {
                using (NpgsqlConnection pgConnection = new NpgsqlConnection(PgConnString))
                {
                    pgConnection.Open();

                    using (NpgsqlCommand pgComm = new NpgsqlCommand())
                    {
                        pgComm.Connection = pgConnection;
                        pgComm.CommandText = "INSERT INTO vacancy.stored_content_temp(vacancy_id, vacancy_data) VALUES (@vacancyId, @vacancyData)";

                        foreach (var items in DownloadedContents)
                        {
                            pgComm.Parameters.Add(new NpgsqlParameter("vacancyId", NpgsqlDbType.Bigint)
                            {
                                Value = items.VacancyId,
                                Direction = ParameterDirection.Input
                            }
                            );
                            pgComm.Parameters.Add(new NpgsqlParameter("vacancyData", NpgsqlDbType.Jsonb)
                            {
                                Value = items.VacancyContent,
                                Direction = ParameterDirection.Input
                            }
                            );
                            pgComm.ExecuteNonQuery();
                            pgComm.Parameters.Clear();
                        }

                        pgComm.CommandText = "vacancy.fn_merge_content";
                        pgComm.CommandType = CommandType.StoredProcedure;
                        pgComm.ExecuteNonQuery();

                        pgComm.Dispose();
                    }
                    pgConnection.Close();
                    pgConnection.Dispose();
                    return true;
                }
            }
            catch (NpgsqlException ne)
            {
                Console.WriteLine(ne);
                return false;
            }
        }
    }
}

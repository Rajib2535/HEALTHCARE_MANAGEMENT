
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace CORPORATE_DISBURSEMENT_UTILITY
{
    /// <summary>
    /// genertic utility class for extensions
    /// </summary>
    public static class Util
    {
        public static IConfiguration? configuration;
        public static void StaticConfigurationInitialize(IConfiguration Configuration)
        {
            configuration = Configuration;
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string GetBase64StringFromImagePath(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            byte[] imageArray = File.ReadAllBytes(filePath);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            string result = $"data:image/{extension.Replace(".", string.Empty)};base64," + base64ImageRepresentation;
            return result;
        }        
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        public static DataTable ToDataTable(List<Dictionary<string, string>> list)
        {
            DataTable result = new();
            if (list.Count == 0)
                return result;

            var columnNames = list.SelectMany(dict => dict.Keys).Distinct();
            result.Columns.AddRange(columnNames.Select(c => new DataColumn(c)).ToArray());
            foreach (Dictionary<string, string> item in list)
            {
                var row = result.NewRow();
                foreach (var key in item.Keys)
                {
                    row[key] = item[key];
                }

                result.Rows.Add(row);
            }

            return result;
        }
        public static string AddDoubleQuotes(this string value)
        {
            return "\"" + value + "\"";
        }
        public static string XMLStringToJson(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string jsonText = JsonConvert.SerializeXmlNode(doc);
            return jsonText;
        }
        public static string FormatJson(string json)
        {
            dynamic? parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Newtonsoft.Json.Formatting.Indented);
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public static string GetLogPath(string log_path, string log_type)
        {
            String absolute_path = "";
            try
            {
                if (OperatingSystem.IsLinux())
                {
                    absolute_path = $"{log_path}/{log_type}/";
                }
                else
                {
                    absolute_path = $"{log_path}\\{log_type}\\";
                }
                bool exists = Directory.Exists(absolute_path);
                if (!exists)
                    Directory.CreateDirectory(absolute_path);
            }
            catch (Exception ex)
            {

            }
            return absolute_path;
        }
        public async static Task<APIConnectivityStatus> CheckConnection(APIConnectivityStatus connectionStatus, int connectionTimeoutSeconds = 5)
        {
            string elapsed = string.Empty;
            Stopwatch timer = new();
            timer.Start();
            string remarks = string.Empty;
            try
            {
                Task<IPHostEntry>? hostEntryTask = null;
                if (!IPAddress.TryParse(connectionStatus.HostURL, out IPAddress? ipAddress))
                {
                    var hostEntryTimeout = TimeSpan.FromSeconds(5);
                    hostEntryTask = Dns.GetHostEntryAsync(connectionStatus.HostURL ?? string.Empty);
                    if (!hostEntryTask.Wait(hostEntryTimeout))
                    {
                        timer.Stop();
                        elapsed = timer.ElapsedMilliseconds.ToString();
                        remarks = $"Connection failed to: {connectionStatus.HostURL}:{connectionStatus.Port} in: {elapsed} milliseconds. Reason => Host Entry Timeout!";
                        connectionStatus.Remarks = remarks;
                        connectionStatus.Status = false;
                        return connectionStatus;
                    }
                    IPHostEntry hostEntry = hostEntryTask.Result;
                    ipAddress = hostEntry.AddressList[0];
                }
                else
                {
                    ipAddress = IPAddress.Parse(connectionStatus.HostURL);
                }
                IPEndPoint ip = new(ipAddress, Convert.ToInt32(connectionStatus.Port));
                Socket server = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IAsyncResult result = server.BeginConnect(ip, null, null);
                result.AsyncWaitHandle.WaitOne(connectionTimeoutSeconds * 1000, true);
                timer.Stop();
                elapsed = timer.ElapsedMilliseconds.ToString();
                if (!server.Connected)
                {
                    remarks = $"Connection failed to: {connectionStatus.HostURL}:{connectionStatus.Port} in: {elapsed} milliseconds. Reason => Could not connect to server!";
                    connectionStatus.Status = false;
                    server.Close();
                }
                else
                {
                    remarks = string.Format("Connected succesfully to: {0} in: {1} milliseconds", server.RemoteEndPoint?.ToString(), elapsed);
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                    connectionStatus.Status = true;
                }
            }
            catch (Exception ex)
            {
                timer.Stop();
                elapsed = timer.ElapsedMilliseconds.ToString();
                remarks = $"Connection failed to: {connectionStatus.HostURL}:{connectionStatus.Port} in: {elapsed} milliseconds. Reason => {ex.Message}";
                connectionStatus.Status = false;
                
            }
            connectionStatus.Remarks = remarks;
            return connectionStatus;
        }
    }
    public class APIConnectivityStatus
    {
        public bool Status { get; set; }
        public string? Remarks { get; set; }
        public string? HostURL { get; set; }
        public string? Port { get; set; }
        public string? Identifier { get; set; } = null;
    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Serilog;
using ILogger = Serilog.ILogger;
using System.Net;
using WEB_APP.Extensions;
using DATA.Models.RequestReponseModels;
using DATA.Models.ViewModels;

namespace WEB_APP.Controllers
{
    public class LogsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public LogsController(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        [TypeFilter(typeof(CheckSessionWithRedirect))]
        [TypeFilter(typeof(CheckPermission))]
        public IActionResult Index()
        {
            List<LogData?> logDataslist = new List<LogData?>();
            ViewData["FValue"] = Request.Query["f"];
            return View();
        }
        private List<LogData?> LogViewer(string log_file_name)
        {
            List<LogData?> logDataslist = new List<LogData?>();
            try
            {               
                ViewData["FValue"] = Request.Query["f"];
                string? location = _configuration.GetValue<string>("FilePaths:LogFileRootDirectory");
                using (var fileStream = new FileStream(Path.Combine(location?.ToString() ?? string.Empty, log_file_name ?? string.Empty), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var reader = new StreamReader(fileStream))
                    {
                        ViewData["LogFile"] = reader.ReadToEnd();
                    }
                }
                var jsonData = Convert.ToString(ViewData["LogFile"]);
                var jsonLines = jsonData?.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                var test = jsonLines?.ToList();
                int log_id = 0;
                if(test != null)
                {
                    foreach (var item in test)
                    {
                        LogData logData = new LogData();
                        string[] parts = item.Split(' ');
                        if (parts.Length >= 4)
                        {
                            logData.id = log_id;
                            logData.level = parts[3];
                            logData.date = parts[0] + " " + parts[1];
                            logData.content = string.Join(" ", parts.Skip(4)); ;

                            logDataslist.Add(logData);
                            log_id++;
                        }
                    }
                }               
                return logDataslist;
            }
            catch (Exception ex)
            {
                _logger.Error(WebUtility.HtmlEncode(ex.ToString()));
            }
            return logDataslist;
        }
        public IActionResult GetLogReportData(string logfilename)
        {
            ServerSideDatatableResponseEntity responseEntity = new();
            try
            {
                List<LogData?> logDataslist = new List<LogData?>();                
                ViewData["FValue"] = logfilename;
                var draw = Request.Form["draw"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
                int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");

                logDataslist = LogViewer(logfilename ?? string.Empty);

                if (logDataslist != null && logDataslist.Count > 0)
                {
                    responseEntity.recordsTotal = logDataslist.Count();
                    if (!string.IsNullOrWhiteSpace(searchValue))
                    {
                        logDataslist = logDataslist.Where(x => x!=null && x.content!=null && x.content.Contains(searchValue)).ToList();
                    }
                    var filter_result = logDataslist.OrderByDescending(x => x?.id).Skip(skip).Take(pageSize).ToList();
                    responseEntity.recordsFiltered = logDataslist.Count;
                    responseEntity.data = filter_result;
                    responseEntity.draw = draw;
                }

                return Ok(responseEntity);
            }
            catch (Exception ex)
            {
                _logger.Error(WebUtility.HtmlEncode(ex.ToString()));
            }
            return Ok(responseEntity);
        }
        public IActionResult DownloadFile(string fileName)
        {
            try
            {
                string? location = _configuration.GetValue<string>("FilePaths:LogFileRootDirectory");
                string? filePath = Path.Combine(location ?? string.Empty, fileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return new FileStreamResult(fileStream, "application/octet-stream")
                {
                    FileDownloadName = fileName,
                    EnableRangeProcessing = true
                };
            }
            catch (Exception ex)
            {
                _logger.Error(WebUtility.HtmlEncode(ex.ToString()));
            }
            return BadRequest();
        }
        [HttpPost]
        public IActionResult DeleteFile(string fileName)
        {
            try
            {
                string? location = _configuration.GetValue<string>("FilePaths:LogFileRootDirectory");
                string filePath = Path.Combine(location ?? string.Empty, fileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    return Ok(); // File deleted successfully
                }
                else
                {
                    return NotFound(); // File not found
                }
            }
            catch (Exception ex)
            {
                _logger.Error(WebUtility.HtmlEncode(ex.ToString()));
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult DeleteAllFiles()
        {
            try
            {
                string? location = _configuration.GetValue<string>("FilePaths:LogFileRootDirectory");
                if (!string.IsNullOrEmpty(location))
                {
                    string[] files = Directory.GetFiles(location);
                    foreach (string filePath in files)
                    {
                        System.IO.File.Delete(filePath);
                    }
                    return Ok(); // All files deleted successfully
                }
                else
                {
                    return BadRequest("File location not specified."); // File location not specified
                }
            }
            catch (Exception ex)
            {
                _logger.Error(WebUtility.HtmlEncode(ex.ToString()));
            }
            return BadRequest();
        }
    }
}

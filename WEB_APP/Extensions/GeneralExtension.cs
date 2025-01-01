
using ClosedXML.Excel;
using System.Data;
using System.Reflection;

namespace CORPORATE_DISBURSEMENT_ADMIN.Extensions
{
    public static class GeneralExtension
    {
        /// <summary>
        /// null handled integer conversion extension
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToNullInt32(this object obj)
        {
            int result = 0;
            if (obj != null)
            {
                Convert.ToInt32(obj);
            }
            return result;
        }
        /// <summary>
        /// integer conversion extension
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt32(this object obj)
        {
            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// list to datatable conversion
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
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
        /// <summary>
        /// returns datatable from an excel file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable ImportExceltoDatatable(string filePath)
        {
            DataTable dt = new DataTable();
            // Open the Excel file using ClosedXML.
            // Keep in mind the Excel file cannot be open when trying to read it
            using (XLWorkbook workbook = new XLWorkbook(filePath))
            {
                IXLWorksheet worksheet = workbook.Worksheet(1);
                bool FirstRow = true;
                //Range for reading the cells based on the last cell used.  
                string readRange = "1:1";
                foreach (IXLRow row in worksheet.RowsUsed())
                {
                    //If Reading the First Row (used) then add them as column name  
                    if (FirstRow)
                    {
                        //Checking the Last cellused for column generation in datatable  
                        readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                        foreach (IXLCell cell in row.Cells(readRange))
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        FirstRow = false;
                    }
                    else
                    {
                        //Adding a Row in datatable  
                        dt.Rows.Add();
                        int cellIndex = 0;
                        //Updating the values of datatable  
                        foreach (IXLCell cell in row.Cells(readRange))
                        {
                            dt.Rows[dt.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                            cellIndex++;
                        }
                    }

                }
                File.Delete(filePath);
                return dt;
            }
        }
        /// <summary>
        /// gets filepath by date and number in a folder
        /// </summary>
        /// <param name="fileSavePath"></param>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static string GetFilePathByDateAndNumber(string fileSavePath, string fileExtension)
        {
            string fileName = string.Empty;
            string fileSavePathWithFolderName = string.Empty;
            try
            {

                //string fileSavePath = _configuration.GetValue<string>("FilePath:AuthenticationFormPDFPath");
                if (!Directory.Exists(fileSavePath))
                {
                    Directory.CreateDirectory(fileSavePath);
                }
                string[] files = Directory.GetFiles(fileSavePath);

                if (files.Length == 0 || !files.Contains(fileSavePath + DateTime.Now.ToString("ddMMyyyy") + "_1" + fileExtension))
                {
                    //Directory.CreateDirectory(fileSavePath + DateTime.Now.ToString("ddMMyyyy") + "_1");
                    fileSavePathWithFolderName = fileSavePath + DateTime.Now.ToString("ddMMyyyy") + "_1" + fileExtension;
                }
                else
                {
                    List<string> fragments = new List<string>();
                    foreach (string file in files)
                    {
                        string pathWithoutExtension = Path.GetFileNameWithoutExtension(file);
                        fragments.Add(pathWithoutExtension.Split('_').Last());
                    }
                    if (fragments.Count > 0)
                    {
                        fragments = fragments.OrderByDescending(x => x).ToList();
                        string fragmentName = (fragments.Take(1).FirstOrDefault().ToInt32() + 1).ToString();
                        fileSavePathWithFolderName = fileSavePath + DateTime.Now.ToString("ddMMyyyy") + "_" + fragmentName + fileExtension;
                        //Directory.CreateDirectory(fileSavePathWithFolderName);
                    }
                }
            }
            catch (Exception ex)
            {

                fileName = string.Empty;
            }
            return fileSavePathWithFolderName;
        }
        /// <summary>
        /// parallel tasking method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="maxDegreeOfConcurrency"></param>
        /// <param name="collection"></param>
        /// <param name="taskFactory"></param>
        /// <returns></returns>
        public static async Task RunWithMaxDegreeOfConcurrency<T>(
     int maxDegreeOfConcurrency, IEnumerable<T> collection, Func<T, Task> taskFactory)
        {
            var activeTasks = new List<Task>(maxDegreeOfConcurrency);
            foreach (var task in collection.Select(taskFactory))
            {
                activeTasks.Add(task);
                if (activeTasks.Count == maxDegreeOfConcurrency)
                {
                    await Task.WhenAny(activeTasks.ToArray());
                    //observe exceptions here
                    activeTasks.RemoveAll(t => t.IsCompleted);
                }
            }
            await Task.WhenAll(activeTasks.ToArray()).ContinueWith(t =>
            {
                //observe exceptions in a manner consistent with the above   
            });
        }
    }
}

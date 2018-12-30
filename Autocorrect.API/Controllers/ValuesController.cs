using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autocorrect.API.Data;
using Autocorrect.API.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Autocorrect.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ValuesController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "OK";
            //try
            //{
            //    string fileURL = @"C:\Users\Ervis\source\repos\Autocorrect\Autocorrect.API\fjalet.xlsx";
            //    DataSet excelContent = ParseExcel(fileURL);

            //    List<SpecialWord> allWords = new List<SpecialWord>();
            //    foreach (DataRow dr in excelContent.Tables[0].Rows)
            //    {
            //        Console.WriteLine($"{dr.ItemArray[1].ToString().ToLower()} - {dr.ItemArray[0].ToString().ToLower()}");
            //        allWords.Add(new SpecialWord()
            //        {
            //            WrongWord = dr.ItemArray[1].ToString().ToLower(),
            //            RightWord = dr.ItemArray[0].ToString().ToLower()
            //        });
            //    }

            //    List<SpecialWord> distinctWords = allWords.GroupBy(x => x.WrongWord).Select(y => y.First()).ToList();

            //    foreach (SpecialWord word in distinctWords)
            //    {
            //        //if (!SpecialWordExists(word.WrongWord))
            //        _context.SpecialWords.Add(word);
            //    }

            //    _context.SaveChanges();

            //    string json = JsonConvert.SerializeObject(excelContent);
            //    return json;

            //}
            //catch (Exception e)
            //{
            //    return e.Message;
            //}
        }

        private bool SpecialWordExists(string wrongWord)
        {
            return _context.SpecialWords.Any(e => e.WrongWord == wrongWord);
        }

        public static DataSet ParseExcel(string excelURL)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(excelURL, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var headers = new List<string>();
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        UseColumnDataType = true,
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = false
                        }
                    });
                    return result; //MapDatasetData(result.Tables.Cast<DataTable>().First());
                }
            }
        }

        public static IEnumerable<Dictionary<string, object>> MapDatasetData(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                var row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                yield return row;
            }
        }

    }
}

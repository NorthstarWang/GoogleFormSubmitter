using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using M8OU.Data;
using M8OU.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace M8OU.Controllers
{
    public class FormHistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public FormHistoriesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: FormHistories
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            return View(await _context.FormHistories.Where(i=>i.IdentityUser.Email==user.Email).ToListAsync());
        }
        
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Detail(string Url, int number)
        {
            FormHistory form = await GetFormJavaScriptArray(Url);
            return View(form);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Url, int number)
        {
            return RedirectToAction("Detail", new {Url=Url, number = number});
        }

        private async Task<FormHistory> GetFormJavaScriptArray(string url)
        {
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            
            htmlDocument.LoadHtml(html);

            //SELECT the javascript tag content
            var JS = htmlDocument.DocumentNode.Descendants()
                .Where(n => n.Name == "script"&&(n.InnerText.Contains("FB_PUBLIC_LOAD_DATA_")||n.InnerText.Contains("FB_LOAD_DATA")))
                .First().InnerText;

            //remove variable name
            string[] jsArray = JS.Split(new string[] {"="}, 2, StringSplitOptions.TrimEntries);
            
            //Analyse javascript code
            string jsVariable = jsArray[1].Substring(0, jsArray[1].Length - 1)
                .Replace("\n", "");
            JArray convertedArray = JArray.Parse(jsVariable);
            
            //Fill the json data to class
            FormHistory formHistory = SortFormData(convertedArray);
            
            return formHistory;
        }

        private FormHistory SortFormData(JArray arr)
        {
            //append fetch data to class instances
            FormHistory formHistory = FormHistoryValueAssign(arr);
            
            var questions = arr[1][1];
            List<FormQuestion> questionsList = new List<FormQuestion>();
            foreach (var question in questions)
            {
                FormQuestion formQuestion = new FormQuestion
                {
                    QuestionNumber = Convert.ToInt32(question[0]),
                    QuestionContent = question[1] == null ? "Question" : question[1].ToString(),
                    //Identify the question type base on the number in the json
                    QuestionType = GetQuestionType(Convert.ToInt32(question[3])),
                    Required = Convert.ToInt32(question[4][0][2]) == 1
                };

                //The insertion of options of each question is depending on the type the question is.
                formQuestion = QuestionProcessByType(formQuestion, question);
                questionsList.Add(formQuestion);
            }
            formHistory.FormQuestions = questionsList;
            formHistory.NumberOfQuestion = questionsList.Count();
            
            return formHistory;
        }

        private FormQuestion QuestionProcessByType(FormQuestion formQuestion, JToken question)
        {
            if (!formQuestion.QuestionType.Equals("Other"))
            {
                switch (formQuestion.QuestionType)
                {
                    case "Short answer":
                    case "Paragraph":
                    {
                        //no processing since no option
                        break;
                    }
                    case "Grid":
                    {
                        formQuestion = GridOptionAppend(formQuestion, question[4]);
                        break;
                    }
                    case "Linear Scale":
                    {
                        formQuestion = LinearScaleOptionAppend(formQuestion, question[4][0][1]);
                        break;
                    }
                    default:
                    {
                        //For default question type, the options will be inserted normally
                        formQuestion = DefaultQuestionInsert(formQuestion, question[4][0][1]);
                        break;
                    }
                }
            }
            else
            {
                //Other will not be processed
            }

            return formQuestion;
        }

        private FormHistory FormHistoryValueAssign(JArray arr)
        {
            //append fetch data to class instances
            FormHistory formHistory = new FormHistory();

            var description = arr[1][0].ToString();
            formHistory.Description = description;

            var title = arr[3].ToString();
            formHistory.Name = title;

            var link = "https://docs.google.com/forms/d/" + arr[14].ToString() + "/viewform";
            formHistory.URL = link;

            return formHistory;
        }

        private FormQuestion LinearScaleOptionAppend(FormQuestion formQuestion, JToken question)
        {
            //loop the option under the question, then add to list
            List<FormQuestionOption> questionOptionsList = new List<FormQuestionOption>();
            foreach (var option in question)
            {
                FormQuestionOption formQuestionOption = new FormQuestionOption
                {
                    Content = option[0].ToString()
                };
                questionOptionsList.Add(formQuestionOption);
            }
            formQuestion.FormQuestionOptions = questionOptionsList;
            return formQuestion;
        }

        private FormQuestion GridOptionAppend(FormQuestion formQuestion, JToken question)
        {
            //loop the option under the question, then add to list
            List<FormQuestionOption> questionOptionsList = new List<FormQuestionOption>();

            //The grid json is listed in row-column alignment where each row contains multiple alike columns
            foreach (var row in question)
            {
                //one row = one option
                FormQuestionOption formQuestionOption = new FormQuestionOption();
                List<OptionColumn> cols = new List<OptionColumn>();

                //each row has at least one column name as well as its own name, array[1] contains all columns' names, [2] defines whether it is required and [3] is the row name
                foreach (var col in row[1])
                {
                    OptionColumn optionColumn = new OptionColumn
                    {
                        Name = col[0].ToString()
                    };

                    cols.Add(optionColumn);
                }

                formQuestionOption.Content = row[3].ToString();
                formQuestionOption.OptionColumns = cols;

                questionOptionsList.Add(formQuestionOption);
            }

            formQuestion.FormQuestionOptions = questionOptionsList;
            return formQuestion;
        }

        private FormQuestion DefaultQuestionInsert(FormQuestion formQuestion, JToken question)
        {
            //loop the option under the question, then add to list
            List<FormQuestionOption> questionOptionsList = new List<FormQuestionOption>();
            foreach (var option in question)
            {
                //if the options is others, it will be excluded
                if (Convert.ToInt32(option[4]) != 1)
                {
                    FormQuestionOption formQuestionOption = new FormQuestionOption
                    {
                        Content = option[0].ToString()
                    };
                    questionOptionsList.Add(formQuestionOption);
                }
            }
            formQuestion.FormQuestionOptions = questionOptionsList;
            return formQuestion;
        }

        private string GetQuestionType(int i)
        {
            if (i == 0)
            {
                return "Short answer";
            }else if (i == 1)
            {
                return "Paragraph";
            }
            else if (i == 2)
            {
                return "Multiple choice";
            }
            else if (i == 3)
            {
                return "Drop-down";
            }
            else if (i == 4)
            {
                return "Checkboxes";
            }
            else if (i == 5)
            {
                return "Linear Scale";
            }
            else if (i == 7)
            {
                //Process different way
                return "Grid";
            }
            else
            {
                //Do not process
                return "Other";
            }
        }

        private bool FormHistoryExists(Guid id)
        {
            return _context.FormHistories.Any(e => e.Id == id);
        }
    }
}

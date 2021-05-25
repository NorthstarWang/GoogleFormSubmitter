using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace M8OU.Models
{
    public class FormHistory
    {
        public Guid Id { get; set; }
        public DateTime TimeOfSubmission { get; set; }
        public string Description { get; set; }
        public string URL{ get; set; }
        public string Name { get; set; }
        public int NumberOfSubmission { get; set; }
        public int NumberOfQuestion { get; set; }
        public IdentityUser IdentityUser{ get; set; }
        public List<Report> Reports { get; set; }
        public List<FormQuestion> FormQuestions { get; set; }
    }

    public class FormQuestion
    {
        public Guid Id { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionType { get; set; }
        public bool Required { get; set; }
        public int TotalSubmissionNumber { get; set; }
        public string QuestionContent { get; set; }
        public FormHistory FormHistory { get; set; }
        public List<FormQuestionOption> FormQuestionOptions { get; set; }
    }

    public class FormQuestionOption
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int SubmissionNumber { get; set; }
        public FormQuestion FormQuestion { get; set; }
    }
}

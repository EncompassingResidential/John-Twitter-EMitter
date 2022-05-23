using System.ComponentModel.DataAnnotations;

namespace twitter_emitter_server.Models {

    /* 
     * This word Issue came from the file name / Visual Studio creating a class
     * How to Create a Web API with ASP.NET CORE and .NET 6 (c# for beginners)
     * stated at 8:32 that if you wanted to create the Model first BEFORE
     * you create the DB tables then you use Models in the "Entity Framework Core" Code-first.
     * 
     * If you create the DB first then you are using "Entity Framework Core" Database-first which
     * then you have to mimic the DB in your code.
     */
    public class Issue {
        public int Id { get; set; }

        [Required] // Title
        public string? Title {  get; set; }

        [Required] // Description
        public string? Description { get; set; }
        public Priority Priority { get; set; }
         
        public IssueType IssueType { get; set; }
        public DateTime Create { get; set; }
        public DateTime? Completed { get; set; }
    }

    public enum Priority {
        Low, Medium, High
    }

    public enum IssueType {
        Feature, Bug, Documentation
    }
}

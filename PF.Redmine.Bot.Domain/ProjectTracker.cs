using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PF.Redmine.Bot.Domain
{
    /*public class Project
    {
        public int id { get; set; }
        public string name { get; set; }
    }*/

    public class Tracker
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Status
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Priority
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    /*public class Author
    {
        public int id { get; set; }
        public string name { get; set; }
    }*/

    public class AssignedTo
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class FixedVersion
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class CustomField
    {
        public int id { get; set; }
        public string name { get; set; }
        public object internal_name { get; set; }
        public string value { get; set; }
    }

    public class Issue
    {
        public int id { get; set; }
        public Project project { get; set; }
        public Tracker tracker { get; set; }
        public Status status { get; set; }
        public Priority priority { get; set; }
        public Author author { get; set; }
        public AssignedTo assigned_to { get; set; }
        public FixedVersion fixed_version { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public string start_date { get; set; }
        public string due_date { get; set; }
        public int done_ratio { get; set; }
        public double spent_hours { get; set; }
        public List<CustomField> custom_fields { get; set; }
        public string created_on { get; set; }
        public string updated_on { get; set; }
    }


    public class Author
    {
        public int id { get; set; }
        public string name { get; set; }
        public object easy_external_id { get; set; }
    }

    public class Parent
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Project
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string homepage { get; set; }
        public int status { get; set; }
        public string easy_start_date { get; set; }
        public Author author { get; set; }
        public string created_on { get; set; }
        public string updated_on { get; set; }
        public Parent parent { get; set; }
        public string easy_due_date { get; set; }
    }

    public class Projects
    {
        public List<Project> projects { get; set; }
        public int total_count { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
    }

    public class Issues
    {
        public List<Issue> issues { get; set; }
        public int total_count { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
    }

    public class DisplayIssue
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int status { get; set; }
    }

    public class DisplayProject
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Token
    {
        public string action { get; set; }
        public string value { get; set; }
    }

    public class User
    {
        public int id { get; set; }
        public string login { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string mail { get; set; }
        public string created_on { get; set; }
        public string api_key { get; set; }
        public List<Token> tokens { get; set; }
        public string last_login_on { get; set; }
    }

    public class Users
    {
        public List<User> users { get; set; }
        public int total_count { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
    }

    public class DisplayUser
    {
        public int id { get; set; }
        public string login { get; set; }
        public string name { get; set; }
    }
}

using System;
namespace TodoApp.Business.Models
{
    public class TaskSearchModel
    {
        public string SearchString { get; set; }

        public int StatusFilter { get; set; }

        public DateTime SearchDateFrom { get; set; }

        public DateTime SearchDateTo { get; set; }

    }
}


﻿using System;
using TaskStatus = TodoApp.Common.TaskStatus;

namespace TodoApp.Business.Model
{
    public class TaskModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string UpdatedBy { get; set; }

        public TaskStatus Status { get; set; }
    }
}


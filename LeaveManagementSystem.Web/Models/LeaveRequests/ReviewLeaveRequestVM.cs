﻿using System.ComponentModel;

namespace LeaveManagementSystem.Web.Models.LeaveRequests
{
    public class ReviewLeaveRequestVM : LeaveRequestReadOnlyVM
    {
        public EmployeeListVM Employee { get; set; } = new();

        [DisplayName("Additional Information")]
        public string RequestComments { get; set; }
    }
}
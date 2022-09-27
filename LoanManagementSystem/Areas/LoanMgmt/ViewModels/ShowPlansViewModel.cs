using LoanManagementSystem.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.Areas.LoanMgmt.ViewModels
{
    public class ShowPlansViewModel
    {
        [Display(Name = "Select Loan Category:")]
        [Required(ErrorMessage = "Please select a category for displaying the Plans")]
        public int LoanId { get; set; }

        public ICollection<LoanCategory> LoanCategories { get; set; }
    }
}

using LoanManagementSystem.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.Areas.LoanMgmt.ViewModels
{
    public class ShowPlansViewModel
    {
        //-----Loan Id -------//
        [Display(Name = "Select Loan Category:")]
        [Required(ErrorMessage = "Please select a Loan for displaying the Plans")]
        public int LoanId { get; set; }

        public ICollection<LoanCategory> LoanCategories { get; set; }
    }
}

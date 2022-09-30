using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementSystem.Models
{
    public class Approval
    {
        /// <summary>
        /// Approval Details
        /// </summary>
        //------Approval Id------//
        [Display(Name = "Approval Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApprovalId { get; set; }

        //-------Status----------//
        [Display(Name = "Status")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [RegularExpression(@"(^[a-zA-Z''-'\s]{1,40}$)", ErrorMessage = "Must be Character")]
        [StringLength(50)]
        public string Status { get; set; }

        //-------Approval Dates----------//
        [Display(Name = "Approval Dates")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [DataType(DataType.Date)]
        public DateTime ApprovalDate { get; set; }


        #region Navigation Properties to the Customer  model
        [Display(Name = "Customer Name")]
        public int CustomerId { get; set; }

        [ForeignKey(nameof(Approval.CustomerId))]
        public Customer Customer { get; set; }

        #endregion
      
    }
}

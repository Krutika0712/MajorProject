using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementSystem.Models
{
    [Table(name: "Customers")]
    public class Customer
    {
        //---------Customer Id---------//
        [Display(Name = "Customer Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }


        //-------Full Name----------//
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [RegularExpression(@"(^[a-zA-Z''-'\s]{1,40}$)",ErrorMessage = "Must be Number")]
        [StringLength(50)]
        public string FullName { get; set; }


        //------- Customer Address-------//
        [Display(Name = "Customer Address")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [StringLength(50)]
        public string Address { get; set; }


        //------Phone Number--------//
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "{0} Cannot be Empty")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Must be Digit")]
        public string Phonenumber { get; set; }


        //---- Email------//
        [Display(Name = " Email")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [EmailAddress(ErrorMessage = "{0} is not valid.")]
        public string Email { get; set; }


        //--- Birth Date -------//
        [Display(Name = "Birth Date")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }


        [Display(Name = "Age")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        public int Age { get; set; }


        //------ Gender------------//
        [Display(Name = "Gender")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        public string Gender { get; set; }


        //----- State ------//
        [Display(Name = "State")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [StringLength(50)]
        public string State { get; set; }



        #region Navigation Properties to the Plan model
        [Display(Name = "Loan Plan ")]
        public int PlanId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        [ForeignKey(nameof(Customer.PlanId))]

        public Plan Plan { get; set; }

        #endregion



        #region Navigation Properties to the Customer Model

        public ICollection<Approval> Approvals { get; set; }

        #endregion

    }
}

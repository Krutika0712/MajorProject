﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanManagementSystem.Models
{
    [Table(name: "Plans")]
    public class Plan
    {
        //---------Loan Id---------//
        [Display(Name = "Plan Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlanId { get; set; }

        // -----Loan name ----------//
        [Display(Name = " Plan Name")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [StringLength(80)]
        public string PlanType { get; set; }


        //---------Amount------//
        [Display(Name = "Amount")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        public int Amount { get; set; }

        //----- description-----//
        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [StringLength(350)]
        public string Description { get; set; }

        // -----  Eligibility -----//
        [Display(Name = "Eligibility")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [StringLength(350)]
        public string Eligibility { get; set; }


        //--- Created Date -------//
        [Display(Name = "Created On")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }


        #region Navigation Properties to the Loan Category model

        public int LoanId { get; set; }

        [ForeignKey(nameof(Plan.LoanId))]
        public LoanCategory LoanCategory { get; set; }

        #endregion


        #region Navigation Properties to the Customer Model

        public ICollection<Customer> Customers { get; set; }

        #endregion

    }
}
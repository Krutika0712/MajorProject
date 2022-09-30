using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LoanManagementSystem.Models
{
    [Table(name: "LoanCategories")]
    public class LoanCategory
    {
        /// <summary>
        /// Loan Category Details
        /// </summary>
        // ---- Loan Id -----//
        [Display(Name = "Loan Id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanId { get; set; }


        // ---- Loan Name----//
        [Display(Name = "Loan Name")]
        [Required(ErrorMessage = "{0} cannot be empty!")]
        [StringLength(100)]
        public string LoanName { get; set; }


        #region Navigation Properties to the Plan  Model
        [JsonIgnore]
        public ICollection<Plan> Plans { get; set; }

        #endregion    

    }
}

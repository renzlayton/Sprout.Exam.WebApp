using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public abstract class BaseSaveEmployeeDto
    {
        [Required(ErrorMessage = "Please enter Full Name"), MaxLength(100)]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Please enter TIN"), MaxLength(12),MinLength(12)]

        public string Tin { get; set; }
        [Required(ErrorMessage = "Please enter Birthday")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
                       ApplyFormatInEditMode = true)]
        public DateTime Birthdate { get; set; }
        [Required]
        public int TypeId { get; set; }
        [Required(ErrorMessage = "Please enter Salary")]
        [Range(0.01, 999999999, ErrorMessage = "Salary must be greater than 0.00")]
        public decimal Salary { get; set; }
    }
}

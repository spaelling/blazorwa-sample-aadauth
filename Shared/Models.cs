using System;
using System.ComponentModel.DataAnnotations;

namespace blazor.wa.aadauth.sample
{
    public class Class1
    {
        [Required]
        public int MyInt { get; set; }

        [Required]
        [StringLength(200)]
        public string Text { get; set; }
    }
    public class Class2
    {
        [Required]
        public int MyInt { get; set; }

        [Required]
        [StringLength(200)]
        public string Text { get; set; }
    }   
}
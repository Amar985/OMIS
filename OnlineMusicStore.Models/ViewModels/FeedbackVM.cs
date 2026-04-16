using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMusic.Models.ViewModels
{
    public class FeedbackVM
    {
        [Key]

        public int Id { get; set; }
        [Required]

        public string Name { get; set; }
        [Required]
        public string Feedback { get; set; }
        [Required]
        public int Rating { get; set; }
    }
}

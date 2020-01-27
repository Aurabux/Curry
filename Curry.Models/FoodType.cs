﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Curry.Models
{
    class FoodType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "FoodType Name")]
        public string Name { get; set; }
    }
}

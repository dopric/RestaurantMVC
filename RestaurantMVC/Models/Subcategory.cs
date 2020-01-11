﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantMVC.Models
{
    public class Subcategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name="Subcategory Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name ="Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}

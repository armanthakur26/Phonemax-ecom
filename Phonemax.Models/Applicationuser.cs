﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Xml.Linq;

namespace Phonemax.Models
{
    public class Applicationuser:IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        [Display(Name = "Company")]
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company company { get; set; }
        [NotMapped]
        public string Role { get; set; }
    }
}

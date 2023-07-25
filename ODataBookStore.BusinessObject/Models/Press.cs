﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataBookStore.BusinessObject.Models
{
    public enum Category
    {
        Book,
        Magazine,
        EBook
    }

    public class Press
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;

namespace CodeCoolAPI.DAL.Models
{
    public class Material
    {
        public Author Author { get; set; }
        public int AuthorId { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime PublishTime { get; set; }
        public MaterialType MaterialType { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}
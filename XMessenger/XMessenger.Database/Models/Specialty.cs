﻿namespace XMessenger.Database.Models
{
    public class Specialty : BaseModel<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
    }
}
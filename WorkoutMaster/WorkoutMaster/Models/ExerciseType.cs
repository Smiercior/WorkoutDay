using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace WorkoutMaster.Models
{
    public class ExerciseType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

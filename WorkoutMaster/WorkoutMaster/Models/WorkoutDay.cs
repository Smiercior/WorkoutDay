using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WorkoutMaster.Models
{
    public class WorkoutDay
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<ExerciseEntry> ExerciseEntries { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WorkoutMaster.Models
{
    public class ExerciseEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(WorkoutDay))]
        public int WorkoutDayId { get; set; }

        [ForeignKey(typeof(Exercise))]
        public int ExerciseId { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Set> Sets { get; set; }

        public string Comment { get; set; }

        public string ExerciseName { get; set; }

        public string SetsString { get; set; }
    }
}

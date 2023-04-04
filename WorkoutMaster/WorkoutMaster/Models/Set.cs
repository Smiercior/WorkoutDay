using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WorkoutMaster.Models
{
    public class Set
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(ExerciseEntry))]
        public int ExerciseEntryId { get; set; }

        public double KG { get; set; }

        //public int RepNumber { get; set; }

        public string Reps { get; set; }
    }
}

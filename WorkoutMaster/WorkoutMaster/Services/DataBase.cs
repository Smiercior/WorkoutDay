using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using WorkoutMaster.Models;

namespace WorkoutMaster.Services
{
    public class DataBase
    {
        private readonly SQLiteAsyncConnection _connection;

        public DataBase(string DbPath)
        {
            _connection = new SQLiteAsyncConnection(DbPath);
            _connection.CreateTableAsync<WorkoutDay>();
            _connection.CreateTableAsync<ExerciseEntry>();
            _connection.CreateTableAsync<Exercise>();
            _connection.CreateTableAsync<Set>();
            _connection.CreateTableAsync<ExerciseType>();
        }


        public Task<int> SaveWorkoutDay(WorkoutDay workoutDay)
        {
            return _connection.InsertAsync(workoutDay);
        }

        public async Task<List<WorkoutDay>> GetWorkoutDays()
        {
            List<WorkoutDay> workoutDays = new List<WorkoutDay>();
            workoutDays = await _connection.Table<WorkoutDay>().ToListAsync();

            try
            {
                for (int i = 0; i < workoutDays.Count; i++)
                {
                    if (workoutDays[i] != null)
                    {
                        //workoutDays[i].ExerciseEntries = (await GetExerciseEntries()).FindAll(x => x.WorkoutDayId == workoutDays[i].Id);
                        workoutDays[i].ExerciseEntries = await GetExerciseEntriesForWorkoutDay(workoutDays[i].Id);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            
            return workoutDays;
            //return _connection.Table<WorkoutDay>().ToListAsync();
        }

        // Get WorkoutDays without exercise entries
        public async Task<List<WorkoutDay>> GetWorkoutDaysBasic()
        {
            return await _connection.Table<WorkoutDay>().ToListAsync();
        }

        public async Task<WorkoutDay> GetWorkoutDayById(int ID)
        {
            WorkoutDay workoutDay = (await _connection.Table<WorkoutDay>().ToListAsync()).Find(x => x.Id == ID);  
            workoutDay.ExerciseEntries = await GetExerciseEntriesForWorkoutDay(workoutDay.Id);
            return workoutDay;
        }
        public async Task<WorkoutDay> GetWorkoutDayByDate(DateTime dateTime)
        {
            WorkoutDay workoutDay = (await _connection.Table<WorkoutDay>().ToListAsync()).Find(x => x.Date == dateTime);
            workoutDay.ExerciseEntries = await GetExerciseEntriesForWorkoutDay(workoutDay.Id);
            return workoutDay;
        }
        public async Task<List<WorkoutDay>> GetWorkoutDaysByType(string exerciseType)
        {
            List<WorkoutDay> workoutDays = (await _connection.Table<WorkoutDay>().ToListAsync()).FindAll(x => x.Type == exerciseType);
            foreach(WorkoutDay workoutDay in workoutDays)
            { 
                workoutDay.ExerciseEntries = await GetExerciseEntriesForWorkoutDay(workoutDay.Id);
            }
            return workoutDays;
        }

        public Task<int> UpdateWorkoutDay(WorkoutDay workoutDay)
        {
            return _connection.UpdateAsync(workoutDay);
        }

        public Task<int> SaveExerciseEntry(ExerciseEntry exerciseEntry)
        {
            return _connection.InsertAsync(exerciseEntry);
        }

        public Task<int> UpdateExerciseEntry(ExerciseEntry exerciseEntry)
        {
            return _connection.UpdateAsync(exerciseEntry);
        }

        public async Task<int> DelExerciseEntry(int ID)
        {
            List<Set> sets = (await GetSets()).FindAll(x => x.ExerciseEntryId == ID);
            foreach (Set set in sets)
            {
                await DelSet(set.Id);
            }

            return await _connection.DeleteAsync<ExerciseEntry>(ID);
        }
        
        public async Task<List<ExerciseEntry>> GetExerciseEntries()
        {
            List<ExerciseEntry> exerciseEntries = new List<ExerciseEntry>();
            exerciseEntries = await _connection.Table<ExerciseEntry>().ToListAsync();
            
            try
            {
                for (int i = 0; i < exerciseEntries.Count; i++)
                {
                    if (exerciseEntries[i] != null)
                    {
                        exerciseEntries[i].ExerciseName = (await GetExercises()).Find(x => x.Id == exerciseEntries[i].ExerciseId).Name;
                        exerciseEntries[i].Sets = (await GetSets()).FindAll(x => x.ExerciseEntryId == exerciseEntries[i].Id);
                        double actualWeight = 0.0;

                        foreach (var set in exerciseEntries[i].Sets)
                        {
                            exerciseEntries[i].SetsString += set.KG + " x " + set.Reps + "\n";
                            /*
                            // First set with certain weight
                            if (actualWeight == 0.0)
                            {
                                actualWeight = set.KG;
                                exerciseEntries[i].SetsString = actualWeight.ToString() + " x ";
                            }

                            // Another set with certain weight
                            if(actualWeight != set.KG)
                            {
                                actualWeight = set.KG;

                                // Remove unnecessary ','
                                exerciseEntries[i].SetsString = exerciseEntries[i].SetsString.Substring(0, exerciseEntries[i].SetsString.Length - 2);
                                exerciseEntries[i].SetsString += "\n" + actualWeight.ToString() + " x ";
                            }
                            exerciseEntries[i].SetsString += set.RepNumber.ToString() + ", ";
                            */

                        }

                        // Remove unnecessary ','
                        //exerciseEntries[i].SetsString = exerciseEntries[i].SetsString.Substring(0, exerciseEntries[i].SetsString.Length - 2);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            
            return exerciseEntries;
        }

        public async Task<List<ExerciseEntry>> GetExerciseEntriesForWorkoutDay(int workoutDayId)
        {
            List<ExerciseEntry> exerciseEntries = new List<ExerciseEntry>();
            exerciseEntries = (await _connection.Table<ExerciseEntry>().ToListAsync()).FindAll(x => x.WorkoutDayId == workoutDayId );

            try
            {
                for (int i = 0; i < exerciseEntries.Count; i++)
                {
                    if (exerciseEntries[i] != null)
                    {
                        exerciseEntries[i].ExerciseName = (await GetExercises()).Find(x => x.Id == exerciseEntries[i].ExerciseId).Name;
                        exerciseEntries[i].Sets = (await GetSets()).FindAll(x => x.ExerciseEntryId == exerciseEntries[i].Id);
                        double actualWeight = 0.0;

                        foreach (var set in exerciseEntries[i].Sets)
                        {
                            exerciseEntries[i].SetsString += set.KG + " x " + set.Reps + "\n";
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }

            return exerciseEntries;

        }


        public Task<int> SaveSet(Set set)
        {
            return _connection.InsertAsync(set);
        }

        public Task<List<Set>> GetSets()
        {
            return _connection.Table<Set>().ToListAsync();
        }

        public Task<int> DelSet(int ID)
        {
            return _connection.DeleteAsync<Set>(ID);
        }

        public Task<int> SaveExercise(Exercise exercise)
        {
            return _connection.InsertAsync(exercise);
        }

        public Task<List<Exercise>> GetExercises()
        {
            return _connection.Table<Exercise>().ToListAsync();
        }

        public Task<int> DelExercise(int ID)
        {
            return _connection.DeleteAsync<Exercise>(ID);
        }

        public Task<int> SaveExerciseType(ExerciseType exerciseType)
        {
            return _connection.InsertAsync(exerciseType);
        }
        public Task<List<ExerciseType>> GetExerciseTypes()
        {
            return _connection.Table<ExerciseType>().ToListAsync();
        }

        public Task<int> DelExerciseType(int ID)
        {
            return _connection.DeleteAsync<ExerciseType>(ID);
        }

        public async Task MockData()
        {
            
            await _connection.DropTableAsync<WorkoutDay>();
            await _connection.DropTableAsync<ExerciseEntry>();
            await _connection.DropTableAsync<Exercise>();
            await _connection.DropTableAsync<Set>();
            await _connection.DropTableAsync<ExerciseType>();

            await _connection.CreateTableAsync<WorkoutDay>();
            await _connection.CreateTableAsync<ExerciseEntry>();
            await _connection.CreateTableAsync<Exercise>();
            await _connection.CreateTableAsync<Set>();
            await _connection.CreateTableAsync<ExerciseType>();

            try
            {
                
            }
            catch (Exception ex22)
            {

            }

            WorkoutDay workoutDay = new WorkoutDay { Date = new DateTime(1979, 07, 28), ExerciseEntries = new List<ExerciseEntry>(), Type = "Upper Body" };
            await SaveWorkoutDay(workoutDay);

            Exercise ex = new Exercise { Name = "Wyciskanie na klate", Description = "Wyciskamy na klatę" };
            Exercise ex2 = new Exercise { Name = "Biceps na maszynie", Description = "Powoli opuszczamy" };

            await SaveExercise(ex);
            await SaveExercise(ex2);


            //ex = (await GetExercises()).Find(x => x.Name == "Wyciskanie na klate");
            //ex2 = (await GetExercises()).Find(x => x.Name == "Biceps na maszynie");

            //workoutDay = (await GetWorkoutDays()).Find(x => x.Date == new DateTime(1979, 07, 28));
            workoutDay = await GetWorkoutDayByDate(new DateTime(1979, 07, 28));
            ExerciseEntry exerciseEntry1 = new ExerciseEntry { Comment = "/ up teraz 90x2,2,2,2 /" };
            ExerciseEntry exerciseEntry2 = new ExerciseEntry { };
            exerciseEntry1.WorkoutDayId = workoutDay.Id;
            exerciseEntry2.WorkoutDayId = workoutDay.Id;

            exerciseEntry1.ExerciseId = 1;
            exerciseEntry2.ExerciseId = 2;


            await SaveExerciseEntry(exerciseEntry1);
            await SaveExerciseEntry(exerciseEntry2);

            exerciseEntry1 = (await GetExerciseEntries()).Find(x => x.Id == 1);
            exerciseEntry2 = (await GetExerciseEntries()).Find(x => x.Id == 2);


            Set set = new Set ();
            Set set2 = new Set ();
            Set set3 = new Set(), set4 = new Set(), set5 = new Set();
            Set set6 = new Set();Set set7 = new Set(); Set set8 = new Set();


            set = new Set { Reps = "4", KG = 90, ExerciseEntryId = exerciseEntry1.Id };
            set2 = new Set { Reps = "5,7", KG = 100, ExerciseEntryId = exerciseEntry1.Id };
            set3 = new Set { Reps = "8,9", KG = 90, ExerciseEntryId = exerciseEntry1.Id };

            set4 = new Set { Reps = "9,8,7", KG = 30, ExerciseEntryId = exerciseEntry2.Id };

            await SaveSet(set);
            await SaveSet(set2);
            await SaveSet(set3);
            await SaveSet(set4);

            ExerciseType exerciseType = new ExerciseType { Name = "Upper body", Description = "Only use upper body muscles"};
            ExerciseType exerciseType2 = new ExerciseType { Name = "Lower body", Description = "Only use lower body muscles" };
            await SaveExerciseType(exerciseType);
            await SaveExerciseType(exerciseType2);
        }
    }
}

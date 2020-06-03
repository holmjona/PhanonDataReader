using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace PhanonDataReader.Models {
    public class LessonHolder {
        private int _ID;
        private Lesson _Lesson;
        private List<Exercise> _Exercises = null;
        public int ExtraLinesPerExercise = 1;

        [JsonPropertyName("id")]
        public int ID {
            get { return _ID; }
            set { _ID = value; }
        }

        [JsonPropertyName("lesson")]
        public Lesson Lesson {
            get { return _Lesson; }
            set { _Lesson = value; }
        }


        [JsonPropertyName("exercises")]
        public List<Exercise> Exercises {
            get {
                if (_Exercises == null) _Exercises = new List<Exercise>();
                return _Exercises;
            }
            set { _Exercises = value; }
        }

        /// <summary>
        /// Get column width needed to display solution.
        /// </summary>
        public int ColumnsNeeded {
            get {
                int maxWidth = 0;
                foreach (Exercise ex in Exercises) {
                    string[] instLines = ex.Instructions.Split('\n');
                    string[] codeLines = ex.Test_Code.Split('\n');
                    string[] soluLines = ex.Solution_Code.Split('\n');

                    int instWidth = instLines.Max(l => l.Length);
                    int codeWidth = codeLines.Max(l => l.Length);
                    int soluWidth = soluLines.Max(l => l.Length);

                    int maxLines = Math.Max(Math.Max(instWidth, codeWidth), soluWidth);
                    if (maxLines > maxWidth) maxWidth = maxLines;
                }
                return maxWidth;
            }
        }
        /// <summary>
        /// Count rows needed to show all code
        /// </summary>
        public int RowsNeeded {
            get {
                int maxHeight = 0;
                foreach (Exercise ex in Exercises) {
                    string[] instLines = ex.Instructions.Split('\n');
                    string[] codeLines = ex.Test_Code.Split('\n');
                    string[] soluLines = ex.Solution_Code.Split('\n');

                    maxHeight += instLines.Length;
                    maxHeight += codeLines.Length;
                    maxHeight += soluLines.Length;
                    maxHeight += ExtraLinesPerExercise;

                }
                return maxHeight;
            }
        }
    }
}

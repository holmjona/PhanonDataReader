using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhanonDataReader.Models {
    public class Exercise {
        private int _ID;
        private string _Instructions;
        private string _Code;
        private string _Solution;

        [JsonPropertyName("id")]
        public int ID {
            get { return _ID; }
            set { _ID = value; }
        }

        [JsonPropertyName("instructions")]
        public string Instructions {
            get { return _Instructions; }
            set {
                string[] lines = CleanUpValue(value);
                bool startBlock = true;
                for (int i = 0; i < lines.Length; i++) {
                    string line = lines[i].TrimEnd();
                    if (line == "```") {
                        if (startBlock) {
                            line += "html"; // place on the end of first / starting blocks for formatting.
                        }
                        startBlock = !startBlock; // toggle back and forth
                    }
                    lines[i] = line;
                }
                _Instructions = String.Join('\n', lines).TrimEnd();

            }
        }

        private string[] CleanUpValue(string strToClean) {
            string retString = strToClean;
            retString = retString.Replace('\r', '\n');
            while (retString.Contains("\n\n")) {
                retString = retString.Replace("\n\n", "\n");
            }
            return retString.Split('\n');
        }

        [JsonPropertyName("test_code")]
        public string Test_Code {
            get { return _Code; }
            set {
                string[] lines = CleanUpValue(value);
                List<String> newLines = new List<string>();
                bool inStudentStarterCode = false;
                bool inFakePythonComment = false;
                bool addNewLineForReadability = false;
                bool skipLine = false;
                for (int i = 0; i < lines.Length; i++) {
                    string line = lines[i].TrimEnd();
                    string lineNoSpace = line.Replace(" ", "");
                    if (lineNoSpace == "") skipLine = true;
                    if (lineNoSpace.ToUpper() == "###BEGIN_STUDENT") inStudentStarterCode = true;
                    if (lineNoSpace.ToUpper() == "###END_STUDENT") {
                        inStudentStarterCode = false;
                        addNewLineForReadability = true;
                    }
                    // remove fake block comments
                    if (!inStudentStarterCode && lineNoSpace == @"""""""") { // three quotes == fake python "block comment"
                        inFakePythonComment = !inFakePythonComment; // toggle back and forth
                        skipLine = true;
                        if (inFakePythonComment)
                            addNewLineForReadability = true;
                    }
                    if (inFakePythonComment) {
                        skipLine = true;
                    }
                    if (lineNoSpace.StartsWith("#phanon")) skipLine = true;
                    if (!skipLine) {
                        // only inlcude lines not to skip.
                        newLines.Add(line);
                    }
                    if (addNewLineForReadability)
                        newLines.Add("");
                    // reset values for next line.
                    skipLine = false;
                    addNewLineForReadability = false;
                }
                lines = newLines.ToArray();
                _Code = String.Join('\n', lines);
            }
        }


        [JsonPropertyName("solution_code")]
        public string Solution_Code {
            get { return _Solution; }
            set { _Solution = String.Join('\n', CleanUpValue(value)).TrimEnd(); }
        }

    }
}

// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $("#btnRead").click(getMatrix);
    $("#btnReadExercises").click(parseExercises);
    $("#txtFilter").keyup(filterTable);
});

function getMatrix() {
    var tbl = $("#tblOutput");
    tbl.empty();
    var courseId = $("#txtCourseID").val();
    var showAverages = $("#chkAverages").is(":checked");
    $.ajax({
        // this file needs contain all exercises from the Progress Map Page
        url: "../data/students.json"
        , success: function (data) {
            var studs = data.progress_map;
            var studsSorted = sorted(studs);
            for (var name in studsSorted) {
                var tr = $("<tr>");
                var tdName = $("<td>").text(name);
                tr.append(tdName);

                var complCount = 0;
                var lessCount = 0;
                var week;
                for (var mod of studs[name]) {
                    var modID = mod.id;
                    var modName = getModuleName(modID);
                    //if (modName.indexOf("Project") > -1) { // pRojects
                    //    lessons = studObj.lessons;
                    //    for (var lndx in lessons) {
                    //        var lesson = lessons[lndx];
                    //        var lessName = getLessonName(lesson.id);
                    //        var content = $("<span>").text(modName);
                    //        var link = $("<a>");
                    //        link.text(lessName);
                    //        link.attr("href", makeLink(courseId, lesson.id, lesson.user_id));
                    //        link.attr("target", "_blank");
                    //        var tdID = $("<td>").append(content, link);
                    //        tdID.addClass("project");
                    //        if (!lesson.started) tdID.addClass("notstarted");
                    //        if (lesson.completed) tdID.addClass("completed");
                    //        tr.append(tdID);
                    //    }
                    //}

                    
                    lessons = mod.lessons;
                    if (showAverages) {
                        var modTD = $("<td>");
                        var completedLessons = 0;
                        var moduleCompleted = true;
                        for (var lesson of lessons) {
                            moduleCompleted = lesson.completed && true;
                            if (lesson.completed) {
                                completedLessons++;
                                complCount++;
                            }
                        }
                        if (moduleCompleted) {
                            modTD.addClass("completed");
                        }
                        var average = completedLessons / lessons.length;
                        var perc = average * 10
                        var perSpan = $("<p>").text(perc.toFixed(2))
                        modTD.append($("<span>").text(modName));
                        modTD.append(perSpan);
                        tr.append(modTD);
                    } else {
                        for (var lesson of lessons) {
                            var lessName = getLessonName(lesson.id);
                            var content = $("<span>").text(modName);
                            var link = $("<a>");
                            link.text(lessName);
                            link.attr("href", makeLink(courseId, lesson.id, lesson.user_id));
                            link.attr("target", "_blank");
                            var tdID = $("<td>").append(content, link);
                            tdID.addClass("project");
                            if (!lesson.started) tdID.addClass("notstarted");
                            if (lesson.completed) {
                                tdID.addClass("completed");
                                complCount++;
                            }
                            tr.append(tdID);
                        }
                    }
                    lessCount += lessons.length;
                    
                }
                var completed = $("<span>").text(complCount +"/"+ lessCount)
                tdName.append(completed);

                tbl.append(tr);
            }
        }
    });
}

function getModuleName(id) {

    for (var mndx in modules) {
        var mod = modules[mndx];
        if (mod.id == id) {
            return mod.module.title;
        }
    }
    return "not found";
}

function getLessonName(id) {

    for (var mndx in modules) {
        var mod = modules[mndx];
        for (var lndx in mod.lessons) {
            var less = mod.lessons[lndx];
            if (less.id == id) { // this is intentional
                return less.title;
            }
        }
    }
    return "not found";
}

function makeLink(courseId, exerciseId, studentId) {
    var lnk = "http://phanon.herokuapp.com/#/course/" + courseId
        + "/user_lesson_review/" + exerciseId
        + "/user/" + studentId;
    return lnk;
    //: S.default.push("/course/" + t.props.courseId + "/submissions/" + e.id + "/student/" + r.user_id
}

function parseExercises() {
    var url = $("#txtUrl").val();
    var out = $("#output");
    $.ajax({
        url: url
        , dataType: "json"
        , success: function (data) {
            var exercises = data.exercises;
            for (var exer in exercises) {
                var inst = $("<p>").val(exer.instructions);
                var code = $("<p>").val(exer.test_code);
                var solu = $("<p>").val(exer.solution_code);

                out.append(inst, code, solu);
            }
        }
    });

}

function sorted(obj) {
    var arr = [];
    var nmArr = [];
    var div = $("#divOutput");
    for (var nm in obj) {
        nmArr.push(nm);
    }
    nmArr.sort();
    for (var nm of nmArr) {
        arr[nm] = obj[nm];
    }
    return arr;
}




function filterTable(evt) {
    var tbl = $("#tblOutput");
    var searchText = $("#txtFilter").val();
    for (row of tbl.children()) {
        if (searchText !== "") {
            var studName = row.cells[0].innerText;
            var sNameLower = studName.toLowerCase();
            var searcher = searchText.toLowerCase();
            if (sNameLower.indexOf(searcher) > -1) {
                $(row).removeClass("hidden");
            } else {
                $(row).addClass("hidden");
            }
        } else {
            $(row).removeClass("hidden");
        }
    }

}
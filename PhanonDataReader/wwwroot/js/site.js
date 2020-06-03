// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $("#btnRead").click(getMatrix);
    $("#btnReadExercises").click(parseExercises);
});

function getMatrix() {
    $.ajax({
        url: "../data/students.json"
        , success: function (data) {
            var tbl = $("#tblOutput");
            var studs = data.progress_map;
            for (var name in studs) {
                var tr = $("<tr>");
                var tdName = $("<td>").text(name);
                tr.append(tdName);
                for (var wndx in studs[name]) {
                    studObj = studs[name][wndx];
                    var modID = studObj.id;
                    var modName = getModuleName(modID);
                    if (modName.indexOf("Project") > -1) {
                        lessons = studObj.lessons;
                        for (var lndx in lessons) {
                            var lesson = lessons[lndx];
                            var lessName = getLessonName(lesson.id);
                            var content = $("<span>").text(modName);
                            var link = $("<a>");
                            link.text(lessName);
                            link.attr("href", makeLink(92, lesson.id, lesson.user_id));
                            link.attr("target", "_blank");
                            var tdID = $("<td>").append(content, link);
                            tdID.addClass("project");
                            if (!lesson.started) tdID.addClass("notstarted");
                            if (lesson.completed) tdID.addClass("completed");
                            tr.append(tdID);
                        }
                    }
                }

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
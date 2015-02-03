$(document).ready(function () {

    $("#ddlDepart").change(function () {
        if ($("#ddlDepart").val() != 0) {
            $('#ddlCourses').empty();
        }
        var dep = $("#ddlDepart").val();
        if (dep != 0) {
            $.ajax({
                type: 'POST',
                url: '/Courses/GetYears',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ d: dep }),
                dataType: "json",
                async: false,
                success: function (result) {
                    $("#ddlYears").empty();
                    for (var r in result) {
                        var select = document.getElementById("ddlYears");
                        var option = document.createElement("option");
                        option.text = result[r].Name;
                        option.value = result[r].Id;
                        select.add(option);
                    }
                    $("#ddlYears").fadeIn(1000);
                },
                error: function (result) {
                    alert("Error on loading years");
                }
            });
        }
    });

    $("#ddlYears").change(function () {
        var dep = $("#ddlYears").val();
        if (dep != 0 && dep != 1 && dep != 2 && dep != 3 && dep != 4) {
            $("#semester").fadeIn(1000);
            $.ajax({
                type: 'POST',
                url: '/Courses/GetCoursesBySpecialization',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ d: dep }),
                dataType: "json",
                async: false,
                success: function (result) {
                    $("#ddlCourses").empty();
                    $("#addRemoveCourses").fadeIn(1000);
                    for (var r in result) {
                        var select = document.getElementById("ddlCourses");
                        var option = document.createElement("option");
                        option.text = result[r].Name;
                        option.value = result[r].SubjectCode;
                        select.add(option);
                    }
                },
                error: function (result) {
                    alert("Error on loading Courses");
                }
            });
        }
    });

$("#ddlSemester").change(function () {
        var dep = $("#ddlYears").val();
        var sem = $("#ddlSemester").val();
    });

$('#SearchButton').click(function () {
    var code = $('#Search').val();
    $.ajax({
        type: 'POST',
        url: '/Courses/SearchCourse',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ CourseCode: code }),
        dataType: "json",
        async: false,
        success: function (result) {
            if (result != null) {
                $('#ddlCourses').empty();
                $("#addRemoveCourses").fadeIn(1000);
                $("#semester").fadeIn(1000);
                var select = document.getElementById("ddlCourses");
                var option = document.createElement("option");
                option.text = result[0].Name;
                option.value = result[0].SubjectCode;
                select.add(option);
            }
            else {
                //alert("לא נמצא קורס מתאים");
                ShowMessageBox("לא נמצא קורס מתאים", "בדוק את קוד הקורס ונסה שנית");
            }
        }
        ,
        error: function (result) {
            //alert("לא נמצא קורס מתאים");
            ShowMessageBox("לא נמצא קורס מתאים", "בדוק את קוד הקורס ונסה שנית");
        }
    });
});

var exists = "";

    $('#btn-add').click(function () {
        exists = "false";       
        $('#ddlCourses option:selected').each(function () {
        exist($(this).text());
        if (exists == "false") {
            $('#select-to').append("<option value='" + $(this).val() + "'>" + $(this).text() + "</option>");
            $('#coursesDetails').append("<p id='" + $(this).attr('Name') + "'>" + $(this).text() + "</p>");
            }
        });
    });

    function exist(selected) {
        $('#select-to option').each(function () {
            if ($(this).text() == selected)
                exists = "true";
        })
    }

    $('#btn-remove').click(function () {
        $('#select-to option:selected').each(function () {
            $('#' + $(this).attr('Name')).remove();
            $(this).remove();
        });
    });

    $('#sunday').change(function () {
        if ($('#sunday').is(':checked')) {
            $('#dDays').append("<p id='dsunday'>" + $(this).val() + " בלי לימודים " + "</p>");
        }
        else {
            $('#dsunday').remove();
        }
    });

    $('#monday').change(function () {
        if ($('#monday').is(':checked')) {
            $('#dDays').append("<p id='dmonday'>" + $(this).val() + " בלי לימודים " + "</p>");
        }
        else {
            $('#dmonday').remove();
        }
    });

    $('#tuesday').change(function () {
        if ($('#tuesday').is(':checked')) {
            $('#dDays').append("<p id='dtuesday'>" + $(this).val() + " בלי לימודים " + "</p>");
        }
        else {
            $('#dtuesday').remove();
        }
    });

    $('#wednesday').change(function () {
        if ($('#wednesday').is(':checked')) {
            $('#dDays').append("<p id='dwednesday'>" + $(this).val() + " בלי לימודים " + "</p>");
        }
        else {
            $('#dwednesday').remove();
        }
    });

    $('#thursday').change(function () {
        if ($('#thursday').is(':checked')) {
            $('#dDays').append("<p id='dthursday'>" + $(this).val() + " בלי לימודים " + "</p>");
        }
        else {
            $('#dthursday').remove();
        }
    });

    $('#friday').change(function () {
        if ($('#friday').is(':checked')) {
            $('#dDays').append("<p id='dfriday'>" + $(this).val() + " בלי לימודים " + "</p>");
        }
        else {
            $('#dfriday').remove();
        }
    });



    $('#stTen').change(function () {
        if ($('#stTwelve').is(':checked')) {
            $(this).prop('checked', false);
        }
        else {
            if ($('#stTen').is(':checked')) {
                $('#dHours').append("<p id='dstTen'>" + $(this).val() + "</p>");
            }
            else {
                $('#dstTen').remove();
            }           
        }
    });
    $('#stTwelve').change(function () {
        if ($('#stTen').is(':checked')) {
            $(this).prop('checked', false);
        }
        else {
            if ($('#stTwelve').is(':checked')) {
                $('#dHours').append("<p id='dstTwelve'>" + $(this).val() + "</p>");
            }
            else {
                $('#dstTwelve').remove();
            }
        }
    });
    $('#endFour').change(function () {
        if ($('#endSix').is(':checked')) {
            $(this).prop('checked', false);
        }
        else {
            if ($('#endFour').is(':checked')) {
                $('#dHours').append("<p id='dendFour'>" + $(this).val() + "</p>");
            }
            else {
                $('#dendFour').remove();
            }
        }
    });
    $('#endSix').change(function () {
        if ($('#endFour').is(':checked')) {
            $(this).prop('checked', false);
        }
        else {
            if ($('#endSix').is(':checked')) {
                $('#dHours').append("<p id='dendSix'>" + $(this).val() + "</p>");
            }
            else {
                $('#dendSix').remove();
            }
        }
    });    
});


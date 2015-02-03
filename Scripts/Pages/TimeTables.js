$(document).ready(function () {
    var max = 10;
    var k = 0;
    var TT;
    $('#tab3').on('click', function () {
        var chosenCourses = [];
        $('#select-to').find('option').each(function () {
            chosenCourses.push({ "SubjectCode": $(this).val(), "Name": $(this).text() });
        })

        var days = [];
        days.push({ "Day": "Sunday", "Checked": $('#sunday').prop('checked') });
        days.push({ "Day": "Monday", "Checked": $('#monday').prop('checked') });
        days.push({ "Day": "Tuesday", "Checked": $('#tuesday').prop('checked') });
        days.push({ "Day": "Wednesday", "Checked": $('#wednesday').prop('checked') });
        days.push({ "Day": "Thursday", "Checked": $('#thursday').prop('checked') });
        days.push({ "Day": "Friday", "Checked": $('#friday').prop('checked') });

        var numOfDays = $('#numDays').val();

        var hourLimits = [];
        hourLimits.push({ "StartTime": $('#sundayStartLimit').val(), "EndTime": $('#sundayEndLimit').val(), "Day": "Sunday", "Checked": $('#sunday').prop('checked') });
        hourLimits.push({ "StartTime": $('#mondayStartLimit').val(), "EndTime": $('#mondayEndLimit').val(), "Day": "Monday", "Checked": $('#monday').prop('checked') });
        hourLimits.push({ "StartTime": $('#tuesdayStartLimit').val(), "EndTime": $('#tuesdayEndLimit').val(), "Day": "Tuesday", "Checked": $('#tuesday').prop('checked') });
        hourLimits.push({ "StartTime": $('#wednesdayStartLimit').val(), "EndTime": $('#wednesdayEndLimit').val(), "Day": "Wednesday", "Checked": $('#wednesday').prop('checked') });
        hourLimits.push({ "StartTime": $('#thursdayStartLimit').val(), "EndTime": $('#thursdayEndLimit').val(), "Day": "Thursday", "Checked": $('#thursday').prop('checked') });
        hourLimits.push({ "StartTime": $('#fridayStartLimit').val(), "EndTime": $('#fridayEndLimit').val(), "Day": "Friday", "Checked": $('#friday').prop('checked') });

        var semester = $('#ddlSemester').val();
        var sortBy = $('input:radio[name=sort]:checked').val();
        //$('#content').html('<img src="../Content/themes/base/images/ajax-loader.gif" />');
        $.ajax({
            type: 'POST',
            url: '/Courses/GetTimeTables',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ Courses: chosenCourses, Days: days, HourLimits: hourLimits, NumOfDays: numOfDays, Semester: semester, SortBy: sortBy }),
            dataType: "json",
            traditional: true,
            async: false,
            success: function (result) {
                if (result == null) {
                    $("#LoadingImage").hide();
                    $('#carousel').hide();
                    $('#Title').hide();
                    //alert('לא נמצאו מערכות מתאימות');
                    //$('#alert').show();
                    //$('#alert').html("");    
                    ShowMessageBox("לא נמצאו מערכות מתאימות", "שנה את המגבלות ונסה שנית");
                    //$('#alert').append("<h3 style=''margin-right:50px;>אנא חזור אחורנית ושנה את בחירותיך</h3>");
                }
                else {
                    k = 0;
                    TT = result;
                    $('#Title').show();
                    $('#alert').hide();
                    $('#carousel').show();
                    $('#Sum').empty();
                    $('#Sum').append(TT.length);
                    $('#top').hide();
                    nextTT(0);
                }
            }
        });
    });

        $('#Next').on('click', function () {
            nextTT(1);
        });

        $('#Next10').on('click', function () {
            nextTT(10);
        });

        $('#Prev').on('click', function () {
            nextTT(-1);
        });

        $('#Prev10').on('click', function () {
            nextTT(-10);
        });

        function nextTT(ind){
            var result = TT;
            switch (ind) {
                case (0):
                    k = 0;
                    break;
                case (1):
                    if (k < result.length - 1)
                        k++;
                    break;
                case (10):
                    if (k < result.length - 11)
                        k += 10;                                     
                    else
                        k = result.length - 1;
                    break;
                case (-1):
                    if (k > 0)
                        k--;
                    break;
                case (-10):
                    if (k > 10)
                        k -= 10;
                    else
                        k = 0;
                    break;
            }
        
            $('#AllTT').empty();
            $('#Index').empty();
            $('#Index').append(k + 1);

            $('#AllTT').append('<li><div id="hours"><div class="hours" id="0000"></div><div class="hours" id="0800">08:00</div><div class="hours" id="0900">09:00</div><div class="hours" id="1000">10:00</div><div class="hours" id="1100">11:00</div><div class="hours" id="1200">12:00</div><div class="hours" id="1300">13:00</div><div class="hours" id="1400">14:00</div><div class="hours" id="1500">15:00</div><div class="hours" id="1600">16:00</div><div class="hours" id="1700">17:00</div><div class="hours" id="1800">18:00</div><div class="hours" id="1900">19:00</div><div class="hours" id="2000">20:00</div><div class="hours" id="2100">21:00</div><div class="hours" id="2200">22:00</div></div><div id="daya' + k + '" class="days"></div><div id="dayb' + k + '" class="days"></div><div id="dayc' + k + '" class="days"></div><div id="dayd' + k + '" class="days"></div><div id="daye' + k + '" class="days"></div><div id="dayf' + k + '" class="days"></div></li>');

            $("#daya" + k).append('<div class="" id="08432505" title=' + result[0].Sunday.Courses[0].SubjectDescription + ' style="overflow: hidden; height: 35px; width:100%; border: 1px solid;"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[0].Sunday.Courses[0].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right;"></span></div>');
            $("#dayb" + k).append('<div class="" id="08432505" title=' + result[0].Monday.Courses[0].SubjectDescription + ' style="overflow: hidden; height: 35px; width:100%; border: 1px solid;"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[0].Monday.Courses[0].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right;"></span></div>');
            $("#dayc" + k).append('<div class="" id="08432505" title=' + result[0].Tuesday.Courses[0].SubjectDescription + ' style="overflow: hidden; height: 35px; width:100%; border: 1px solid;"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[0].Tuesday.Courses[0].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right;"></span></div>');
            $("#dayd" + k).append('<div class="" id="08432505" title=' + result[0].Wednesday.Courses[0].SubjectDescription + ' style="overflow: hidden; height: 35px; width:100%; border: 1px solid;"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[0].Wednesday.Courses[0].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right;"></span></div>');
            $("#daye" + k).append('<div class="" id="08432505" title=' + result[0].Thursday.Courses[0].SubjectDescription + ' style="overflow: hidden; height: 35px; width:100%; border: 1px solid;"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[0].Thursday.Courses[0].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right;"></span></div>');
            $("#dayf" + k).append('<div class="" id="08432505" title=' + result[0].Friday.Courses[0].SubjectDescription + ' style="overflow: hidden; height: 35px; width:100%; border: 1px solid;"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[0].Friday.Courses[0].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right;"></span></div>');

            for (var i = 1; i < result[k].Sunday.Courses.length; i++) {
                var newColor = '#' + (0x1000000 + (Math.random()) * 0xffffff).toString(16).substr(1, 6);
                if (result[k].Sunday.Courses[i].GroupCode == -1) {
                    for (var j = 1; j <= result[k].Sunday.Courses[i].NumConLessons; j++) {
                        $("#daya" + k).append('<div class="empty" id=' + result[k].Sunday.Courses[i].GroupCode + ' title="" style=" height: 36px; width:100%; border: 1px solid"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[k].Sunday.Courses[i].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right;">' + result[k].Sunday.Courses[i].Class + ' ' + result[k].Sunday.Courses[i].Lecturer + '</span></div>');
                    }
                }
                else {
                    $("#daya" + k).append('<div class="ttClass" id=' + result[k].Sunday.Courses[i].GroupCode + ' title="כיתה: ' + result[k].Sunday.Courses[i].Class + ', ' + result[k].Sunday.Courses[i].OccupationDescription + ', קוד קבוצה: ' + result[k].Sunday.Courses[i].GroupCode + '" style="background-color:' + result[k].Sunday.Courses[i].Color + '; overflow: hidden; height:' + (36.7 * result[k].Sunday.Courses[i].NumConLessons) + 'px; width:100%; border: 1px solid"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[k].Sunday.Courses[i].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right; font-size: 11px;">' + result[k].Sunday.Courses[i].Class + ' ' + result[k].Sunday.Courses[i].Lecturer + '</span></div>');
                }
            }
            for (var i = 1; i < result[k].Monday.Courses.length; i++) {
                var newColor = '#' + (0x1000000 + (Math.random()) * 0xffffff).toString(16).substr(1, 6);
                if (result[k].Monday.Courses[i].GroupCode == -1) {
                    for (var j = 1; j <= result[k].Monday.Courses[i].NumConLessons; j++) {
                        $("#dayb" + k).append('<div class="empty" id=' + result[k].Monday.Courses[i].GroupCode + ' title="" style=" height:36px; width:100%; border: 1px solid"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[k].Monday.Courses[i].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right;">' + result[k].Monday.Courses[i].Class + ' ' + result[k].Monday.Courses[i].Lecturer + '</span></div>');
                    }
                }
                else {
                    $("#dayb" + k).append('<div class="ttClass" id=' + result[k].Monday.Courses[i].GroupCode + ' title="כיתה: ' + result[k].Monday.Courses[i].Class + ', ' + result[k].Monday.Courses[i].OccupationDescription + ', קוד קבוצה: ' + result[k].Monday.Courses[i].GroupCode + '" style="background-color:' + result[k].Monday.Courses[i].Color + '; overflow: hidden; height:' + (36.7 * result[k].Monday.Courses[i].NumConLessons) + 'px; width:100%; border: 1px solid"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[k].Monday.Courses[i].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right; font-size: 11px;">' + result[k].Monday.Courses[i].Class + ' ' + result[k].Monday.Courses[i].Lecturer + '</span></div>');
                }
            }
            for (var i = 1; i < result[k].Tuesday.Courses.length; i++) {
                var newColor = '#' + (0x1000000 + (Math.random()) * 0xffffff).toString(16).substr(1, 6);
                if (result[k].Tuesday.Courses[i].GroupCode == -1) {
                    for (var j = 1; j <= result[k].Tuesday.Courses[i].NumConLessons; j++) {
                        $("#dayc" + k).append('<div class="empty" id=' + result[k].Tuesday.Courses[i].GroupCode + ' title="" style=" height:36px; width:100%; border: 1px solid"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[k].Tuesday.Courses[i].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right;">' + result[k].Tuesday.Courses[i].Class + ' ' + result[k].Tuesday.Courses[i].Lecturer + '</span></div>');
                    }
                }
                else {
                    $("#dayc" + k).append('<div class="ttClass" id=' + result[k].Tuesday.Courses[i].GroupCode + ' title="כיתה: ' + result[k].Tuesday.Courses[i].Class + ', ' + result[k].Tuesday.Courses[i].OccupationDescription + ', קוד קבוצה: ' + result[k].Tuesday.Courses[i].GroupCode + '" style="background-color:' + result[k].Tuesday.Courses[i].Color + '; overflow: hidden; height:' + (36.7 * result[k].Tuesday.Courses[i].NumConLessons) + 'px; width:100%; border: 1px solid"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[k].Tuesday.Courses[i].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right; font-size: 11px;">' + result[k].Tuesday.Courses[i].Class + ' ' + result[k].Tuesday.Courses[i].Lecturer + '</span></div>');
                }
            }
            for (var i = 1; i < result[k].Wednesday.Courses.length; i++) {
                var newColor = '#' + (0x1000000 + (Math.random()) * 0xffffff).toString(16).substr(1, 6);
                if (result[k].Wednesday.Courses[i].GroupCode == -1) {
                    for (var j = 1; j <= result[k].Wednesday.Courses[i].NumConLessons; j++) {
                        $("#dayd" + k).append('<div class="empty" id=' + result[k].Wednesday.Courses[i].GroupCode + ' title="" style=" height:36px; width:100%; border: 1px solid"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[k].Wednesday.Courses[i].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right;">' + result[k].Wednesday.Courses[i].Class + ' ' + result[k].Wednesday.Courses[i].Lecturer + '</span></div>');
                    }
                }
                else {
                    $("#dayd" + k).append('<div class="ttClass" id=' + result[k].Wednesday.Courses[i].GroupCode + ' title="כיתה: ' + result[k].Wednesday.Courses[i].Class + ', ' + result[k].Wednesday.Courses[i].OccupationDescription + ', קוד קבוצה: ' + result[k].Wednesday.Courses[i].GroupCode + '" style="background-color:' + result[k].Wednesday.Courses[i].Color + '; overflow: hidden; height:' + (36.7 * result[k].Wednesday.Courses[i].NumConLessons) + 'px; width:100%; border: 1px solid"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[k].Wednesday.Courses[i].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right; font-size: 11px;">' + result[k].Wednesday.Courses[i].Class + ' ' + result[k].Wednesday.Courses[i].Lecturer + '</span></div>');
                }
            }
            for (var i = 1; i < result[k].Thursday.Courses.length; i++) {
                var newColor = '#' + (0x1000000 + (Math.random()) * 0xffffff).toString(16).substr(1, 6);
                if (result[k].Thursday.Courses[i].GroupCode == -1) {
                    for (var j = 1; j <= result[k].Thursday.Courses[i].NumConLessons; j++) {
                        $("#daye" + k).append('<div class="empty" id=' + result[k].Thursday.Courses[i].GroupCode + ' title="" style=" height:36px; width:100%; border: 1px solid"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[k].Thursday.Courses[i].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right;">' + result[k].Thursday.Courses[i].Class + ' ' + result[k].Thursday.Courses[i].Lecturer + '</span></div>');
                    }
                }
                else {
                    $("#daye" + k).append('<div class="ttClass" id=' + result[k].Thursday.Courses[i].GroupCode + ' title="כיתה: ' + result[k].Thursday.Courses[i].Class + ', ' + result[k].Thursday.Courses[i].OccupationDescription + ', קוד קבוצה: ' + result[k].Thursday.Courses[i].GroupCode + '" style="background-color:' + result[k].Thursday.Courses[i].Color + '; overflow: hidden; height:' + (36.7 * result[k].Thursday.Courses[i].NumConLessons) + 'px; width:100%; border: 1px solid"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[k].Thursday.Courses[i].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right; font-size: 11px;">' + result[k].Thursday.Courses[i].Class + ' ' + result[k].Thursday.Courses[i].Lecturer + '</span></div>');
                }
            }
            for (var i = 1; i < result[k].Friday.Courses.length; i++) {
                var newColor = '#' + (0x1000000 + (Math.random()) * 0xffffff).toString(16).substr(1, 6);
                if (result[k].Friday.Courses[i].GroupCode == -1) {
                    for (var j = 1; j <= result[k].Friday.Courses[i].NumConLessons; j++) {
                        $("#dayf" + k).append('<div class="empty" id=' + result[k].Friday.Courses[i].GroupCode + ' title="" style=" height:36px; width:100%; border: 1px solid"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[k].Friday.Courses[i].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right;">' + result[k].Friday.Courses[i].Class + ' ' + result[k].Friday.Courses[i].Lecturer + '</span></div>');
                    }
                }
                else {
                    $("#dayf" + k).append('<div class="ttClass" id=' + result[k].Friday.Courses[i].GroupCode + ' title="כיתה: ' + result[k].Friday.Courses[i].Class + ', ' + result[k].Friday.Courses[i].OccupationDescription + ', קוד קבוצה: ' + result[k].Friday.Courses[i].GroupCode + '" style="background-color:' + result[k].Friday.Courses[i].Color + '; overflow: hidden; height:' + (36.7 * result[k].Friday.Courses[i].NumConLessons) + 'px; width:100%; border: 1px solid"><div class="ttClassColor"></div><span style="font-weight: bold; font-size: 12px;">' + result[k].Friday.Courses[i].SubjectDescription + '<br></span><span style="font-weight: lighter; direction: rtl; text-align: right; float: right; font-size: 11px;">' + result[k].Friday.Courses[i].Class + ' ' + result[k].Friday.Courses[i].Lecturer + '</span></div>');
                }
            }

            $('.ttClass').css({ 'line-height': '1.50em', 'margin-left': '-1px', 'margin-top': '-1px', 'border-width': '1px', 'border-style': 'solid', 'overflow': 'hidden', 'border-radius': '6px 6px 6px 6px', 'opacity': '0.8', 'z-index': '5' });
            $('.empty').css({ 'line-height': '1.50em', 'margin-left': '-1px', 'margin-top': '-1px', 'border-width': '1px', 'border-style': 'solid', 'overflow': 'hidden', 'z-index': '5', 'background-color': 'white' });
            //$('#logo').hide();

            var script = document.createElement('script');
            script.type = 'text/javascript';
            script.src = "../Scripts/Pages/jquery.jcarousel.min.js";
            document.body.appendChild(script);

            var script = document.createElement('script');
            script.type = 'text/javascript';
            script.src = "../Scripts/Pages/jcarousel.connected-carousels.js";
            document.body.appendChild(script);   
        };
    
});

function ShowDialog(modal) {
    $("#overlay").show();
    $("#dialog").fadeIn(300);

    if (modal) {
        $("#overlay").unbind("click");
    }
    else {
        $("#overlay").click(function () {
            HideDialog();
        });
    }
}



function HideDialog() {
    $("#overlay").hide();
    $("#dialog").fadeOut(300);
}

function ShowMessageBox(headerMessage, bodyMessage) {
    $("#PopUpHeader").html(headerMessage);
    $('#PopUpMessege').html(bodyMessage);
    ShowDialog(false);

}
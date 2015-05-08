$(document).ready(function () {
    $('.tabs .tab-links a').on('click', function (e) {
        var currentAttrValue = $(this).attr('href');

        // Show/Hide Tabs
        $('.tabs ' + currentAttrValue).show().siblings().hide();

        // Change/remove current tab to active
        $(this).parent('li').addClass('active').siblings().removeClass('active');

        if (currentAttrValue == "#tab2") {
            $('#content').css({ 'background-color': '#575c54', 'width': '60%' });
            //$('#footer').show();
            //$('#logo').show();
            //$('.tab-content').css("padding-bottom", "20%");
        }

        if (currentAttrValue == "#tab1") {
            $('#content').css({ 'background-color': '#575c54', 'width': '60%' });
           // $('#footer').show();
            //$('#logo').show();
            //$('.tab-content').css("padding-bottom", "20%");
        }

        e.preventDefault();
    });
});



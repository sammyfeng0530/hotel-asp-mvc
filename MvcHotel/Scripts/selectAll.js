$('#selectAll').click(function () {
    //alert(this.checked);
    if ($(this).is(':checked')) {
        $('input[name="checkitem"]').each(function () {
            //此处如果用attr，会出现第三次失效的情况
            $(this).prop("checked", true);
        });
    } else {
        $('input[name="checkitem"]').each(function () {
            $(this).removeAttr("checked", false);
        });
        //$(this).removeAttr("checked");
    }

});
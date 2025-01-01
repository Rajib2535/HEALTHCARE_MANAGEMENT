function GetDropdown(url, selector, type) {  
    console.log('Url: ', url);  
    $.ajax({
        type: 'GET',
        url: url,
        async: false,
        success: function (result) {
            console.log('dropdown result: ', result);
            var options = $(selector);
            if (type != "") {
                options.append($("<option />").val(0).text('--- Select ' + type + ' ---'));
            }           
            //don't forget error handling!
            $.each(result, function (index, item) {
                options.append($("<option />").val(item.value).text(item.text));
            });
        }
    });
}
function GetDropdownWithEmptySelector(url, selector, type) {
    console.log('Url: ', url);
    $.ajax({
        type: 'GET',
        url: url,
        async: false,
        success: function (result) {
            console.log('dropdown result: ', result);
            var options = $(selector);
            if (type != "") {
                options.append($("<option />").val('').text('--- Select ' + type + ' ---'));
            }
            //don't forget error handling!
            $.each(result, function (index, item) {
                options.append($("<option />").val(item.value).text(item.text));
            });
        }
    });
}
function ViewDetail() {
    var acc = document.getElementsByClassName("accordion");
    for (var i = 0; i < acc.length; i++) {
        acc[i].addEventListener("click", function () {
            this.classList.toggle("active");
            var panel = this.nextElementSibling;
            panel.classList.toggle("panel-active")
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + "px";
            }
        });
    }
}
//function TestAjax() {
//    $(document).ready(function () {
//        $('#form').submit(function (e) {
//            e.preventDefault();
//            var form = $(this);
//            $.ajax({
//                type: "POST",
//                url: form.attr('action'),
//                data: form.serialize(),
//                success: function (data) {
//                    // Code to clear existing table data
//                    $('.panel td').empty();
//                    // Loop through returned data and append to table
//                    $.each(data, function (index, item) {
//                        var row = '<tr>' +
//                            '<td>' + item.Name + '</td>' +
//                            '<td>' + item.Column2 + '</td>' +
//                            '<td>' + item.Column3 + '</td>' +
//                            '</tr>';
//                        $('.panel td').append(row);
//                    });
//                },
//                error: function () {
//                    // Code logic on error
//                }
//            });
//        });
//    });
//}

var chkShowSelect = document.getElementById("chkShowSelect");
var selOption = document.getElementById("selOption");

chkShowSelect.addEventListener("change", function () {
    if (chkShowSelect.checked) {
        selOption.style.display = "block";
    } else {
        selOption.style.display = "none";
    }
});
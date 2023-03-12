//function ViewDetail() {
//    var acc = document.getElementsByClassName("accordion");
//    for (var i = 0; i < acc.length; i++) {
//        acc[i].addEventListener("click", function () {
//            this.classList.toggle("active");
//            var panel = this.nextElementSibling;
//            panel.classList.toggle("panel-active")
//            if (panel.style.maxHeight) {
//                panel.style.maxHeight = null;
//            } else {
//                panel.style.maxHeight = panel.scrollHeight + "px";
//            }
//        });
//    }
//}

function UpdateSetting(id) {
    $('#update-' + id).submit()
}

var chkShowSelect = document.getElementById("chkShowSelect");
var selOption = document.getElementById("selOption");

chkShowSelect.addEventListener("change", function () {
    if (chkShowSelect.checked) {
        selOption.style.display = "block";
    } else {
        selOption.style.display = "none";
    }
});
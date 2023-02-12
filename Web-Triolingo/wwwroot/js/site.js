// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(() => {
    if ($('.boxscroll')) $('.boxscroll').height($(window).height() - 350);
    $(window).resize(() => {
        if ($('.boxscroll') )$('.boxscroll').height($(window).height()-350)
    })
})
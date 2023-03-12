// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(() => {
    if ($('.boxscroll')) $('.boxscroll').height($(window).height() - 300);
    if ($('.boxscroll1')) $('.boxscroll1').height($(window).height() - 400);
    if ($('.boxscroll12')) $('.boxscroll12').height(($(window).height() - 450)/2);
    $(window).resize(() => {
        if ($('.boxscroll')) $('.boxscroll').height($(window).height() - 300)
        if ($('.boxscroll1')) $('.boxscroll1').height($(window).height() - 400);
        if ($('.boxscroll12')) $('.boxscroll12').height(($(window).height() - 450) / 2);
    })
})
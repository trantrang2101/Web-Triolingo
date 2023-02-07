function test() {
    console.log('haizz')
    $.ajax({
        method: "POST",
        data: { Email: "John", Password: "Boston" },
        dataType: "JSON",
        complete: {
            console.log('haizz ok')
        }
    })
}
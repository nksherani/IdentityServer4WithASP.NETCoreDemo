$("#agree").change((e) => {
    if (e.target.checked) {
        $("#btnLogin").prop('disabled', false);
    }
    else {
        $("#btnLogin").prop('disabled', true);
    }
});

$("#clicksearch").click((e) => {
    console.log("Before click");
    window.location.href = 'GetUsers?name=' + $("#search").val();
    console.log("After click");
});
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    

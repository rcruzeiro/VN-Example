// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(document).ready(function () {
    $(function () {
        $('#btnSubmit').click(function (event) {
            event.preventDefault();

            $.ajax({
                type: "GET",
                url: "/order/createorder",
                statusCode: {
                    200: function (data) {
                        createOrder(data);
                    }
                }
            });
        });
    });

    function createOrder(behavior) {
        var payload = {
            ip: behavior.ip,
            pageName: behavior.pageName,
            userAgent: behavior.userAgent[0],
        };
        console.log(JSON.stringify(payload));
        $.ajax({
            type: "POST",
            url: "/api/v1.0/behavior",
            data: JSON.stringify(payload),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            statusCode: {
                201: function () {
                    window.location.replace("http://localhost:8081/order/confirmation");
                }
            }
        });
    }
});
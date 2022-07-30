(function ($) {
    "use strict"
    //basic bar chart
    $.ajax({
        type: "POST",
        url: '/Admin/Home/RevenueForTheWeek',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var dataT = {
                defaultFontFamily: 'Poppins',
                labels: ["Thứ hai", "Thứ ba", "Thứ tư", "Thứ năm", "Thứ sáu", "Thứ bảy", "Chủ nhật"],
                datasets: [
                    {
                        label: "Tổng tiền",
                        data: data.data,
                        borderColor: 'rgba(26, 51, 213, 1)',
                        borderWidth: "0",
                        backgroundColor: 'rgba(26, 51, 213, 1)'
                    }
                ]
            };
            const barChart_1 = document.getElementById("barChart_1").getContext('2d');
            barChart_1.height = 100;
            new Chart(barChart_1, {
                type: 'bar',
                data: dataT,
                options: {
                    legend: false,
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }],
                        xAxes: [{
                            // Change here
                            barPercentage: 0.5
                        }]
                    }
                }
            });
        }
    });
    $.ajax({
        type: "POST",
        url: '/Admin/Home/RevenueForDay',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#revenueforday').text(data.revenueforday +' VNĐ');
        }
    });
    $.ajax({
        type: "POST",
        url: '/Admin/Home/FeedbackCount',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            var feebbacktotal = data.reply + data.noreply;
            $('#feedbackcount').text(feebbacktotal);
            $('#reply').text(data.reply * 100 / feebbacktotal + '%');
            $('#noreply').text(data.noreply * 100 / feebbacktotal + '%');

            $('#info-circle-card').circleProgress({
                value: data.reply / (data.reply + data.noreply),
                size: 100,
                fill: {
                    gradient: ["#a389d5"]
                }
            });

        }
    });
    $.ajax({
        type: "POST",
        url: '/Admin/Home/ProductCount',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#productcount').text(data.productcount);
        }
    });
    $.ajax({
        type: "POST",
        url: '/Admin/Home/ArticleCount',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#articlecount').text(data.articlecount);
        }
    });
    $.ajax({
        type: "POST",
        url: '/Admin/Home/CustomerCount',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (data) {
            $('#customercount').text(data.customercount);
        }
    });
})(jQuery);
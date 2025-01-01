function LoadTransactionChart(url) {
    var ticksStyle = {
        fontColor: '#495057',
        fontStyle: 'bold'
    }
    var mode = 'index';
    var intersect = true;
    debugger;
    $.ajax({
        type: 'POST',
        url: url,
        success: function (res) {
            debugger;
            var labelData = [];
            var valueData_prepaid = [];
            var valueData_postpaid = [];
            res.forEach(function (e, i) {
                labelData.push(e.month_name);
                valueData_prepaid.push(e.prepaid_transaction_amount_sum);
                valueData_postpaid.push(e.postpaid_transaction_amount_sum);
            });
            var sum = sumArray(valueData_prepaid) + sumArray(valueData_postpaid);
            $('#spnTotalAmount').text(sum.toFixed(2));
            var $transactionChart = $('#transaction-chart');
            // eslint-disable-next-line no-unused-vars
            var salesChart = new Chart($transactionChart, {
                type: 'bar',
                data: {
                    labels: labelData,
                    datasets: [{
                        label: 'PREPAID',
                        data: valueData_prepaid,
                        backgroundColor: '#28a745',
                        borderColor: '#007bff',
                    },
                    {
                        label: 'POSTPAID',
                        data: valueData_postpaid,
                        backgroundColor: '#ffc107',
                        borderColor: '#007bff',
                    }]
                },
                options: {
                    maintainAspectRatio: false,
                    //tooltips: {
                    //    mode: mode,
                    //    intersect: intersect
                    //},
                    //hover: {
                    //    mode: mode,
                    //    intersect: intersect
                    //},
                    legend: {
                        display: true
                    },
                    scales: {
                        yAxes: [{
                            // display: false,
                            gridLines: {
                                display: true,
                                lineWidth: '4px',
                                color: 'rgba(0, 0, 0, .2)',
                                zeroLineColor: 'transparent'
                            },
                            ticks: $.extend({
                                beginAtZero: true,

                                // Include a dollar sign in the ticks
                                callback: function (value) {
                                    if (value >= 1000) {
                                        value /= 1000
                                        value += 'k'
                                    }

                                    return  value
                                }
                            }, ticksStyle)
                        }],
                        xAxes: [{
                            display: true,
                            gridLines: {
                                display: true
                            },
                            ticks: ticksStyle
                        }]
                    }
                }
            })
        }
    })
}
const sumArray = arr => arr.reduce((a, b) => a + Number(b), 0);

$("#transactionReportTable").DataTable({
    "responsive": true,
    lengthChange: true,
    autoWidth: false,
    order:[]
});
function LoadPaidAndInitiatedOrderCountPieChart(url) {
    var ticksStyle = {
        fontColor: '#495057',
        fontStyle: 'bold'
    };
    var mode = 'index';
    var intersect = true;
    debugger;
    $.ajax({
        type: 'POST',
        url: url,
        success: function (res) {
            
            const ctx = document.getElementById('PaidAndInitiatedOrderCountPieChart').getContext('2d');
            var labelData = Object.keys(res);
            var valueData = Object.values(res);
            var sum = sumArray(valueData);
            $('#spnTotalOrderCount').text(sum);
            const myChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: labelData,
                    datasets: [{
                        //label: '# of Votes',
                        data: valueData,
                        backgroundColor: [
                            'rgb(40, 167, 69)',
                            'rgb(255, 193, 7)',
                        ],
                        borderColor: 'rgba(200, 200, 200, 0.75)',
                        hoverBorderColor: 'rgba(200, 200, 200, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            display: true,
                            position: 'right',
                        },
                        title: {
                            display: true,
                            text: 'Recharge Status - Today'
                        }
                    }
                },
            });
        }
    })
}
function LoadPrepaidAndPostpaidOrderCountPieChart(url) {
    var ticksStyle = {
        fontColor: '#495057',
        fontStyle: 'bold'
    };
    var mode = 'index';
    var intersect = true;
    debugger;
    $.ajax({
        type: 'POST',
        url: url,
        success: function (res) {

            const ctx = document.getElementById('PrepaidAndPostpaidOrderCountPieChart').getContext('2d');
            var labelData = Object.keys(res);
            var valueData = Object.values(res);
            var sum = sumArray(valueData);
            $('#spnTotalPrepaidAndPostpaidOrderCount').text(sum);
            const myChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: labelData,
                    datasets: [{
                        //label: '# of Votes',
                        data: valueData,
                        backgroundColor: [
                            'rgb(40, 167, 69)',
                            'rgb(255, 193, 7)',
                        ],
                        borderColor: 'rgba(200, 200, 200, 0.75)',
                        hoverBorderColor: 'rgba(200, 200, 200, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            display: true,
                            position: 'right',
                        },
                        title: {
                            display: true,
                            text: 'Recharge Status - Today'
                        }
                    }
                },
            });
        }
    })
}







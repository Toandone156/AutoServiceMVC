var orderChart = document.getElementById("orderChart");
var productChart = document.getElementById("productChart");
var statusChart = document.getElementById("statusChart");
var BuildedOrderChart;

function buildOrderChart(month) {
    var data;
    $.ajax({
        url: '/Admin/Home/GetOrderStatisticByMonth',
        type: 'GET',
        data: { month: month },
        success: function (response) {
            data = response.data;
        },
        error: function (xhr, status, error) {
            console.log("Fail to call api")
        },
        async: false
    });

    const config = {
        type: 'line',
        data: data,
        options: {
            plugins: {
                filler: {
                    propagate: false,
                }
            },
            maintainAspectRatio: false,
            layout: {
                padding: {
                    left: 0,
                    right: 0,
                    top: 0,
                    bottom: 10
                }
            },
            scales: {
                y: {
                    min: 0,
                    ticks: {
                        stepSize: 1,
                    },
                },
            },
            interaction: {
                intersect: false,
            }
        }
    }
    BuildedOrderChart = new Chart(orderChart, config);
}

function buildProductChart() {
    var data;

    $.ajax({
        url: '/Admin/Home/GetTop10Products',
        type: 'GET',
        data: {},
        success: function (response) {
            data = response.data;
        },
        error: function (xhr, status, error) {
            console.log("Fail to call api")
        },
        async: false
    });

    const config = {
        type: 'doughnut',
        data: data
    }

    let BuildedProductChart = new Chart(productChart, config);
}

function buildStatusChart() {
    var data;

    $.ajax({
        url: '/Admin/Home/GetOrderStatusStatistic',
        type: 'GET',
        data: {},
        success: function (response) {
            data = response.data;
        },
        error: function (xhr, status, error) {
            console.log("Fail to call api")
        },
        async: false
    });

    const config = {
        type: 'bar',
        data: data,
        options: {
            indexAxis: 'y',
            responsive: true,
            plugins: {
                legend: {
                    display: false
                }
            }
        },
    }

    let BuildedProductChart = new Chart(statusChart, config);
}

document.querySelectorAll(".orderMonth").forEach(item => {
    item.addEventListener("click", e => {
        e.preventDefault();

        BuildedOrderChart.destroy();
        buildOrderChart(item.getAttribute("data-value"));
    })
})

buildOrderChart(new Date().getMonth() + 1)
buildProductChart()
buildStatusChart()
const ctx = document.getElementById('myChart');
const myChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: [],
        datasets: [{
            label: [],
            data: [],
            borderColor: '#FF6384',
            borderWidth: 1
        }]
    },
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
});

const ctxu = document.getElementById('graficoUmidade');
const graficoUmidade = new Chart(ctxu, {
    type: 'line',
    data: {
        labels: [],
        datasets: [{
            label: [],
            data: [],
            borderWidth: 1
        }]
    },
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
});

async function montarGrafico() {

    myChart.clear();
    graficoUmidade.clear();

    myChart.data.labels = [];
    myChart.data.datasets[0].data = [];
    myChart.data.datasets[0].label = 'temperatura';

    graficoUmidade.data.labels = [];
    graficoUmidade.data.datasets[0].data = [];
    graficoUmidade.data.datasets[0].label = 'umidade';

    pontosGrafico.forEach(function (item, i) {

        myChart.data.labels[i] = item.data_Medicao;
        myChart.data.datasets[0].data[i] = item.temperatura;

        graficoUmidade.data.labels[i] = item.data_Medicao;
        graficoUmidade.data.datasets[0].data[i] = item.umidade;

    });

    myChart.update();
    graficoUmidade.update();
}
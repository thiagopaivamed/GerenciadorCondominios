function PegarMeses(dados) {
    var labels = [];
    var tamanho = dados.length;
    var indice = 0;

    while (indice < tamanho) {
        labels.push(dados[indice].meses);
        indice = indice + 1;
    }
    
    return labels;
}

function PegarValores(dados) {
    var valores = [];
    var tamanho = dados.length;
    var indice = 0;

    while (indice < tamanho) {
        valores.push(dados[indice].valores);
        indice = indice + 1;
    }
    
    return valores;
}
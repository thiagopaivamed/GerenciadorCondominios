function ConfirmarExclusao(id, nome, controller) {
    $('.modal').modal({
        dismissble: true
    });

    $('#modal').modal('open');
    $(".nome").text(nome);
    const url = "/" + controller + "/Delete";
    
    $(".btnExcluir").on('click', function () {
        $.ajax({
            method: "POST",
            url: url,
            data: { id: id },
            success: function (data) {
                location.reload(true);
            }
        })
    })
}
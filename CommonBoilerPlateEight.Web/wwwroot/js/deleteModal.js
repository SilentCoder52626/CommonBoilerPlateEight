$(document).ready(function () {
    $('.bs-delete-modal').on('show.bs.modal', function (e) {
        var itemId = $(e.relatedTarget).data('id');
        $(e.currentTarget).find('input[name="Id"]').val(itemId);
    });
});

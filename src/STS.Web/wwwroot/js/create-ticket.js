(function () {
    var showModal = document.querySelector('.show-create-btn');
    var dialog = document.querySelector('.create-dialog');
    var cancelBtn = document.querySelector('.cancel-btn');

    showModal.addEventListener('click', function () {
        if (!dialog.open) {
            dialog.showModal();
        }
    });

    cancelBtn.addEventListener('click', function () {
        dialog.close('Cancel');
    });
})();
